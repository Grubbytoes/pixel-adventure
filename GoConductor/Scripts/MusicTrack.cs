using System;
using Godot;

namespace GoConductorPlugin.addons.go_conductor__;

public partial class MusicTrack : GcMusicNode
{
    private float _gain;
    private string busBuff;
    
    private Tween VolumeTween { get; set; }
    private AudioStreamPlayer AudioPlayer { get; set; }
    private float FinalTrackVolume { get; set; }
    [Export] public float Attack = 0.2f;
    [Export] public bool Loop = true;

    public override float PlaybackPosition
    {
        get => AudioPlayer.GetPlaybackPosition();
        set => AudioPlayer.Seek(value);
    }
    
    /// <summary>
    /// This is what we use to access volume, rather than prying into the AudioStreamPlayer's 'volume_db'
    /// Should be tidier this way... he says...
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
        if (Playing) {return;}
        
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
        VolumeTween.TweenCallback(Callable.From(()=> AudioPlayer.StreamPaused = true));
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

    // TODO
    private void TrackEnd()
    {
        // If loop, play it again, sam
        if (Loop)
        {
            AudioPlayer.Play();
        }
        // Otherwise, we stop
        else
        {
            Stop();
        }
    }

    /// <summary>
    /// Assigns the track to a temporary bus
    ///
    /// Whichever bus the track was already assigned to will be saved in a buffer, and reset to next time ResetBus()
    /// is called.
    /// </summary>
    /// <param name="idx">The bus index</param>
    public void SetTemporaryBus(int idx)
    {
        // Save current bus
        busBuff = AudioPlayer.Bus;
        
        // Get the new bus
        string newBusName = AudioServer.GetBusName(idx);
        AudioPlayer.Bus = newBusName;
    }

    /// <summary>
    /// Resets the bus to that which is stored in the buffer, if one is
    ///
    /// If the buffer is empty, it returns false
    /// </summary>
    /// <returns>True if successful</returns>
    public bool ResetBus()
    {
        if (busBuff != null) return false;

        AudioPlayer.Bus = busBuff;
        busBuff = null;
        return true;
    }

    private bool VolumeTweenInUse()
    {
        return (VolumeTween is not null && !VolumeTween.IsValid());
    }

    public override void _Ready()
    {
        AudioPlayer = (AudioStreamPlayer)GetChild(0);
        FinalTrackVolume = AudioPlayer.VolumeDb;
        AudioPlayer.Finished += TrackEnd;
    }
}