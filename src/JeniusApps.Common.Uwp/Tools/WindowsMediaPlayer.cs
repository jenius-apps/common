using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public class WindowsMediaPlayer : IMediaPlayer
{
    private readonly MediaPlayer _player;
    private readonly System.Timers.Timer _timer = new();
    private double _fadeInTargetVolume;
    private double _fadeOutStartingVolume;
    private long _fadeStart;
    private long _fadeEnd;
    private long _startEndDiff;
    private bool _fadeIn;

    private CancellationTokenSource _fadeCts = new();

    public event EventHandler<TimeSpan>? PositionChanged;

    public WindowsMediaPlayer(bool disableSystemControls = false)
    {
        var player = new MediaPlayer();
        if (disableSystemControls)
        {
            player.CommandManager.IsEnabled = false;
        }
        _player = player;
        _player.PlaybackSession.PositionChanged += OnPlaybackPositionChanged;
        _timer.Elapsed += OnFadeTimerTick;
        _timer.Interval = 30;
    }

    /// <inheritdoc/>
    public TimeSpan Duration => _player.PlaybackSession.NaturalDuration;

    /// <inheritdoc/>
    public double Volume
    {
        get => _player.Volume;
        set => _player.Volume = value;
    }

    /// <inheritdoc/>
    public void Play(double fadeInTargetVolume, double fadeDuration)
    {
        _fadeCts.Cancel();
        _fadeCts = new CancellationTokenSource();

        if (fadeInTargetVolume <= 0 || fadeDuration <= 0)
        {
            _player.Play();
            return;
        }

        _fadeIn = true;
        var now = DateTime.Now;
        _fadeStart = now.Ticks;
        _fadeEnd = now.AddMilliseconds(fadeDuration).Ticks;
        _startEndDiff = _fadeEnd - _fadeStart;
        _fadeInTargetVolume = fadeInTargetVolume;
        _player.Volume = 0;
        _player.Play();
        _timer.Start();
    }

    private void OnFadeTimerTick(object sender, ElapsedEventArgs e)
    {
        if (_fadeCts.IsCancellationRequested)
        {
            _timer.Stop();
            return;
        }

        var currentTicks = e.SignalTime.Ticks - _fadeStart;
        double percent = (double)currentTicks / _startEndDiff;
        Debug.WriteLine($"#################### {currentTicks} / {_startEndDiff} = {percent}");

        if (percent >= 1)
        {
            _timer.Stop();
            _player.Volume = _fadeIn ? _fadeInTargetVolume : 0;
            Debug.WriteLine($"#################### stopped");

            if (!_fadeIn)
            {
                _player.Pause();
                _player.Volume = _fadeOutStartingVolume;
            }
        }
        else
        {
            _player.Volume = _fadeIn
                ? _fadeInTargetVolume * percent
                : _fadeOutStartingVolume * (1 - percent);
            Debug.WriteLine($"#################### volume {_player.Volume}");
        }
    }

    /// <inheritdoc/>
    public void Pause(double fadeOutDuration)
    {
        _fadeCts.Cancel();
        _fadeCts = new CancellationTokenSource();

        if (fadeOutDuration <= 0)
        {
            _player.Pause();
            return;
        }

        _fadeIn = false;
        var now = DateTime.Now;
        _fadeStart = now.Ticks;
        _fadeEnd = now.AddMilliseconds(fadeOutDuration).Ticks;
        _startEndDiff = _fadeEnd - _fadeStart;
        _fadeOutStartingVolume = _player.Volume;
        _timer.Start();
    }

    public void Play()
    {
        _fadeCts.Cancel();
        _player.Play();
    }

    public void Pause()
    {
        _fadeCts.Cancel();
        _player.Pause();
    }

    /// <inheritdoc/>
    public void Dispose() => _player.Dispose();

    /// <inheritdoc/>
    public bool SetUriSource(Uri uriSource, bool enableGaplessLoop = false)
    {
        try
        {
            var mediaSource = MediaSource.CreateFromUri(uriSource);
            AssignSource(mediaSource, enableGaplessLoop);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> SetSourceAsync(string pathToFile, bool enableGaplessLoop = false)
    {
        try
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(pathToFile);
            var mediaSource = MediaSource.CreateFromStorageFile(file);
            AssignSource(mediaSource, enableGaplessLoop);
        }
        catch
        {
            return false;
        }

        return true;
    }

    private void AssignSource(MediaSource source, bool enableGaplessLoop)
    {
        _player.Source = enableGaplessLoop
            ? LoopEnabledPlaybackList(source)
            : source;
    }

    private MediaPlaybackList LoopEnabledPlaybackList(MediaSource source)
    {
        // This code here (combined with a wav source file) allows for gapless playback!
        var item = new MediaPlaybackItem(source);
        var playbackList = new MediaPlaybackList() { AutoRepeatEnabled = true };
        playbackList.Items.Add(item);
        return playbackList;
    }

    private void OnPlaybackPositionChanged(MediaPlaybackSession sender, object args)
    {
        if (sender is null)
        {
            return;
        }

        PositionChanged?.Invoke(sender, sender.Position);
    }
}
