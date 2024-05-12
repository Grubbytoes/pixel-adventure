using System.Collections.Generic;
using Godot;

namespace GoConductor;

public partial class MusicSwitch : MultiTrackPlayer
{
    private MusicTransition _transition;

    [Export] public float TransitionTime = 1f;
    public GcNode CurrentlyPlaying { get; private set; }
    public bool RestartOnRecue { get; set; }


    public override float PlaybackPosition
    {
        get => CurrentlyPlaying.PlaybackPosition;
        set => CurrentlyPlaying.PlaybackPosition = value;
    }

    public override void Play()
    {
        base.Play();
        CurrentlyPlaying.Play();
    }

    public override void Pause()
    {
        base.Pause();
        CurrentlyPlaying.Pause();
    }

    public override void Stop()
    {
        base.Stop();
        CurrentlyPlaying.Stop();
    }

    public override void PlayFrom(float position)
    {
        CurrentlyPlaying.PlayFrom(position);
    }

    public override bool Cue(GcNode newTrack)
    {
        // Exit if new track is null
        if (newTrack == null) return false;

        // Exit if same track and not restart on recue
        if (newTrack.Equals(CurrentlyPlaying) && !RestartOnRecue) return false;

        // If we're cueing from a state of pause, then our life is easy
        // But we are unlikely to be so lucky
        if (Playing)
        {
            // Abort the transition if there is one in progress, and instance a new one
            _transition?.Kill();
            _transition = new Crossfade(this, TransitionTime);

            // Add the tracks to be tweened
            _transition.AddIncomingTrack(newTrack);
            _transition.AddOutgoingTrack(CurrentlyPlaying);

            // May god have mercy on our souls
            _transition.Start();
        }

        // Set the marker to the new track
        CurrentlyPlaying = newTrack;

        // If we got this far, then we succeeded, wooo!!
        return true;
    }

    /// <summary>
    ///     Returns which ever track is set to currently playing, all others should be hidden
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<GcNode> GetVisibleTracks()
    {
        return new[] { CurrentlyPlaying };
    }
}