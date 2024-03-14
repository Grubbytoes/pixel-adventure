using Godot;

namespace GoConductorPlugin.addons.go_conductor__.transition;

public partial class TempoCrossfade : MusicTransition
{
    private int OutgoingBusIdx { set; get; }
    private int IncomingBusIdx { set; get; }
    private bool TracksSetToBusses;
    
    public TempoCrossfade(GcMusicNode parent, float duration) : base(parent, duration)
    {
    }

    public override void Start()
    {
        SetupBusses();
        base.Start();
    }

    public override void Kill()
    {
        base.Kill();
        TeardownBusses();
    }

    private void SetupBusses()
    {
        AudioServer.AddBus();
        AudioServer.AddBus();

        OutgoingBusIdx = AudioServer.BusCount - 1;
        IncomingBusIdx = AudioServer.BusCount - 2;
    }

    private void TeardownBusses()
    {
        foreach (var t in Outgoing)
        {
            t.ResetBus();
        }

        foreach (var t in Incoming)
        {
            t.ResetBus();
        }
        
        AudioServer.RemoveBus(OutgoingBusIdx);
        AudioServer.RemoveBus(IncomingBusIdx);
    }

    private void SetTracksToBusses()
    {
        if (TracksSetToBusses) {return;}

        foreach (var t in Outgoing)
        {
            t.SetTemporaryBus(OutgoingBusIdx);
        }

        foreach (var t in Incoming)
        {
            t.SetTemporaryBus(IncomingBusIdx);
        }

        TracksSetToBusses = true;
    }
}