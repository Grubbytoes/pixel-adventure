using Godot;

namespace GoConductor;

public abstract partial class GcNode : Node, ITrackController
{
    private float _playbackPosition;

    /// <summary>
    ///     Where in time the arrangement is at currently
    /// </summary>
    public virtual float PlaybackPosition
    {
        get
        {
            GD.PushWarning("UNIMPLEMENTED PLAYBACK POSITION GET");
            return _playbackPosition;
        }
        set => _playbackPosition = value;
    }

    /// <summary>
    ///     The point in time at which the arrangement was last played from or paused at.
    ///     Ie, where the arrangement should be playing from next time play is hit
    /// </summary>
    public bool Playing { get; set; }

    protected float PlayHead { get; set; }

    public virtual void Play()
    {
        Playing = true;
    }

    public virtual void Pause()
    {
        Playing = false;
    }

    public virtual void Stop()
    {
        Playing = false;
        PlayHead = 0;
    }

    public void Restart()
    {
        Stop();
        Play();
    }

    /// <summary>
    ///     Pauses if playing, plays if paused
    /// </summary>
    public void TogglePause()
    {
        if (Playing)
            Pause();
        else
            Play();
    }


    public virtual void PlayFrom(float position)
    {
        PlayHead = position;
        Playing = false;
        Play();
    }

    /// <summary>
    ///     Alias for TogglePause
    /// </summary>
    public void TogglePlay()
    {
        TogglePause();
    }

    protected void DebugPrint(string message)
    {
        var output = Name + ": - " + message;
        GD.Print(output);
    }
}