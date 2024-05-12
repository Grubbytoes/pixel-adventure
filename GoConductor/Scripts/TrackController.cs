namespace GoConductor;

public interface ITrackController
{
    public void Play();

    public void Pause();

    public void Stop();

    public void Restart();

    public void TogglePause();

    public void PlayFrom(float position);
}