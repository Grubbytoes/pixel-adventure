namespace GoConductor;

/// <summary>
///     As in "That which can be affected by a transition"
/// </summary>
public class Transitionoid : ITrackController
{
    // Transitionoid containing a generic MusicController
    public Transitionoid(ITrackController target)
    {
        Target = target;
        TerminalTrack = null;
    }

    // Transitionoid containing a Track - ie a guaranteed 'leaf' with no further children
    public Transitionoid(MusicTrack target)
    {
        Target = target;
        TerminalTrack = target;
    }

    public ITrackController Target { set; get; }
    public MusicTrack TerminalTrack { set; get; }

    public void Play()
    {
        Target.Play();
    }

    public void Pause()
    {
        Target.Pause();
    }

    public void Stop()
    {
        Target.Stop();
    }

    public void Restart()
    {
        Target.Restart();
    }

    public void TogglePause()
    {
        Target.TogglePause();
    }

    public void PlayFrom(float position)
    {
        Target.PlayFrom(position);
    }
}