namespace GoConductor;

/// <summary>
/// As in "That which can be affected by a transition"
/// </summary>
public class Transitionoid: IMusicController
{
    public IMusicController Target { set; get; }
    public MusicTrack TerminalTrack { set; get; }

    // Transitionoid containing a generic MusicController
    public Transitionoid(IMusicController target)
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
}