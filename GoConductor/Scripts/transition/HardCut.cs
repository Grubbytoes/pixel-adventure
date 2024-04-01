namespace GoConductor;

/// <summary>
///     Easiest thing in the world, just straight stops one track and starts the other
/// </summary>
public partial class HardCut : MusicTransition
{
    public HardCut(GcNode parent, float duration) : base(parent, duration)
    {
    }

    public override void Start()
    {
        base.Start();

        // Stop outgoing tracks
        foreach (var t in Outgoing) t.Stop();

        // Start incoming tracks
        foreach (var t in Incoming) t.Play();

        // Done
        CloseTween();
    }
}