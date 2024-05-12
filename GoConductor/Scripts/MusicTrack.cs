using Godot;

namespace GoConductor;

public partial class MusicTrack : GcNode
{
    private string _busBuff;
    private float _gain;

    [Export] public float Attack = 0.2f;
    [Export] public bool AutoPlay;
    [Export] public bool Loop = true;
    [Export] public AudioStream Track;

    private Tween VolumeTween { get; set; }
    private AudioStreamPlayer AudioPlayer { get; set; }
    private float FinalTrackVolume { get; set; }

    public override float PlaybackPosition
    {
        get => AudioPlayer.GetPlaybackPosition();
        set => AudioPlayer.Seek(value);
    }

    /// <summary>
    ///     This is what we use to access volume, rather than prying into the AudioStreamPlayer's 'volume_db'
    ///     Should be tidier this way... he says...
    /// </summary>
    public float Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            AudioPlayer.VolumeDb = _gain + FinalTrackVolume;
        }
    }

    public override void Play()
    {
        // If we are already playing, return
        if (Playing) return;

        // We're ready to go
        base.Play();
        StartAudioPlayer();
    }

    public override void Pause()
    {
        base.Pause();
        PlayHead = AudioPlayer.GetPlaybackPosition();
        HaltAudioPlayer();
    }

    public override void Stop()
    {
        base.Stop();
        HaltAudioPlayer();
    }

    // Stops Audio player after a short volume tween, nothing more, nothing less
    // doesn't touch any variables, nothing
    private void HaltAudioPlayer()
    {
        if (VolumeTweenInUse())
        {
            AudioPlayer.StreamPaused = true;
            return;
        }

        VolumeTween = CreateTween();
        VolumeTween.TweenProperty(AudioPlayer, "volume_db", FinalTrackVolume - 30, Attack);
        VolumeTween.TweenCallback(Callable.From(() => AudioPlayer.StreamPaused = true));
    }

    private void StartAudioPlayer()
    {
        // Make sure the volume's okay
        AudioPlayer.VolumeDb = FinalTrackVolume;

        // Tween?
        if (!VolumeTweenInUse() && PlayHead > 0)
        {
            AudioPlayer.VolumeDb -= 30;
            VolumeTween = CreateTween();
            VolumeTween.TweenProperty(AudioPlayer, "volume_db", FinalTrackVolume, Attack);
        }

        // Start her up!
        AudioPlayer.Play(PlayHead);
    }

    private void TrackEnd()
    {
        // If loop, play it again, sam
        if (Loop)
            AudioPlayer.Play();
        // Otherwise, we stop
        else
            Stop();
    }

    /// <summary>
    ///     Assigns the track to a temporary bus
    ///     Whichever bus the track was already assigned to will be saved in a buffer, and reset to next time ResetBus()
    ///     is called.
    /// </summary>
    /// <param name="idx">The bus index</param>
    public void SetTemporaryBus(int idx)
    {
        // Save current bus
        _busBuff = AudioPlayer.Bus;

        // Get the new bus
        var newBusName = AudioServer.GetBusName(idx);
        AudioPlayer.Bus = newBusName;
    }

    /// <summary>
    ///     Resets the bus to that which is stored in the buffer, if one is
    ///     If the buffer is empty, it returns false
    /// </summary>
    /// <returns>True if successful</returns>
    public bool ResetBus()
    {
        if (_busBuff != null) return false;

        AudioPlayer.Bus = _busBuff;
        _busBuff = null;
        return true;
    }

    private bool VolumeTweenInUse()
    {
        return VolumeTween is not null && !VolumeTween.IsValid();
    }

    public override void _Ready()
    {
        // Instance and add the audio player
        AudioPlayer = new AudioStreamPlayer();
        AudioPlayer.Stream = Track;
        AudioPlayer.Bus = SettingsReader.MusicBus;
        AddChild(AudioPlayer);

        // Read the final volume from the AudioPlayer TODO I think we'll have to change this later
        FinalTrackVolume = AudioPlayer.VolumeDb;
        // Attaching signals
        AudioPlayer.Finished += TrackEnd;

        // Finally, are we on autoplay?
        if (AutoPlay) AudioPlayer.Play();
    }
}