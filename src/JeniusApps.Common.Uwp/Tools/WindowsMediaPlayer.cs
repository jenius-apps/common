using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

#nullable enable

namespace JeniusApps.Common.Tools.Uwp;

public class WindowsMediaPlayer : IMediaPlayer
{
    private readonly MediaPlayer _player;
    private Timer? _fadeInTimer;
    private double _fadeInSingleStep;
    private double _fadeInTargetVolume;
    private long _fadeInStart;
    private long _fadeInEnd;
    private long _startEndDiff;

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

        _fadeInTimer = new Timer();
        var now = DateTime.Now;
        _fadeInStart = now.Ticks;
        _fadeInEnd = now.AddMilliseconds(fadeInDuration).Ticks;
        _startEndDiff = _fadeInEnd - _fadeInStart;
        _fadeInTimer.Elapsed += OnFadeInTimerTick;
        _fadeInSingleStep = fadeInTargetVolume.Value / fadeInDuration;
        _fadeInTargetVolume = fadeInTargetVolume.Value;
        _player.Volume = 0;
        _player.Play();
        _fadeInTimer.Start();
    }

    private void OnFadeInTimerTick(object sender, ElapsedEventArgs e)
    {
        var currentTicks = e.SignalTime.Ticks - _fadeInStart;
        double percent = (double)currentTicks / _startEndDiff;
        Debug.WriteLine($"#################### {currentTicks} / {_startEndDiff} = {percent}");

        if (percent >= 1)
        {
            if (_fadeInTimer is { } timer)
            {
                timer.Elapsed -= OnFadeInTimerTick;
                timer.Stop();
            }

            _player.Volume = _fadeInTargetVolume;
            _fadeInTimer = null;
            Debug.WriteLine($"#################### stopped");
        }
        else
        {
            _player.Volume = _fadeInTargetVolume * percent;
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
