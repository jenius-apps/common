﻿using System;
using System.Threading.Tasks;

namespace JeniusApps.Common.Tools;

/// <summary>
/// An abstraction over the system media player.
/// </summary>
public interface IMediaPlayer
{
    /// <summary>
    /// Raised when the position of the active playback item has changed.
    /// </summary>
    event EventHandler<TimeSpan> PositionChanged;

    /// <summary>
    /// Media player's volume.
    /// </summary>
    double Volume { get; set; }

    /// <summary>
    /// Duration of the current playback item.
    /// </summary>
    TimeSpan Duration { get; }
    
    /// <summary>
    /// Pauses the media player.
    /// </summary>
    void Pause(double fadeOutDuration = 0);

    /// <summary>
    /// Plays the media player.
    /// </summary>
    void Play(double? fadeInTargetVolume = null, double fadeInDuration = 0);

    /// <summary>
    /// Sets the media player's source using the given file path.
    /// </summary>
    /// <param name="pathToFile">Path of the source file.</param>
    /// <param name="enableGaplessLoop">If true, the media source will be configured for gapless playback loop.</param>
    /// <returns>True if setting the source was successful. False, otherwise.</returns>
    Task<bool> SetSourceAsync(string pathToFile, bool enableGaplessLoop = false);

    /// <summary>
    /// Sets the media player's source using the given URI.
    /// </summary>
    /// <param name="uriSource">The URI to load into the media player.</param>
    /// <param name="enableGaplessLoop">If true, the media source will be configured for gapless playback loop.</param>
    /// <returns>True if setting the source was successful. False, otherwise.</returns>
    bool SetUriSource(Uri uriSource, bool enableGaplessLoop = false);

    /// <summary>
    /// Releases resources associated with the media player.
    /// </summary>
    void Dispose();
}
