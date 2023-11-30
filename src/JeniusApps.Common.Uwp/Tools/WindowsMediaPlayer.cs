using System;
using System.Diagnostics;
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
    private readonly Timer _timer = new();
    private double _fadeTargetVolume;
    private long _fadeStart;
    private long _fadeEnd;
    private long _startEndDiff;
    private bool _fadeIn;

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
    public void Play(double? fadeInTargetVolume = null, double fadeInDuration = 0)
    {
        if (fadeInTargetVolume is null || fadeInTargetVolume <= 0 || fadeInDuration <= 0)
        {
            _player.Play();
            return;
        }

        _fadeIn = true;
        var now = DateTime.Now;
        _fadeStart = now.Ticks;
        _fadeEnd = now.AddMilliseconds(fadeInDuration).Ticks;
        _startEndDiff = _fadeEnd - _fadeStart;
        _fadeTargetVolume = fadeInTargetVolume.Value;
        _player.Volume = 0;
        _player.Play();
        _timer.Start();
    }

    private void OnFadeTimerTick(object sender, ElapsedEventArgs e)
    {
        var currentTicks = e.SignalTime.Ticks - _fadeStart;
        double percent = (double)currentTicks / _startEndDiff;
        Debug.WriteLine($"#################### {currentTicks} / {_startEndDiff} = {percent}");

        if (percent >= 1)
        {
            _timer.Stop();
            _player.Volume = _fadeIn ? _fadeTargetVolume : 0;
            Debug.WriteLine($"#################### stopped");

            if (!_fadeIn)
            {
                _player.Pause();
            }
        }
        else
        {
            _player.Volume = _fadeIn
                ? _fadeTargetVolume * percent
                : _fadeTargetVolume * (1 - percent);
            Debug.WriteLine($"#################### volume {_player.Volume}");
        }
    }

    /// <inheritdoc/>
    public void Pause(double fadeOutDuration = 0)
    {
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
        _fadeTargetVolume = _player.Volume;
        _timer.Start();
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
