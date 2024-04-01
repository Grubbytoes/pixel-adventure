namespace GoConductor;

/// <summary>
///     Once started, fades one or more tracks in or out. Tracks faded out are stopped once the transition is over
///     This is ALL IT DOES, any updates to currently playing must be done by the parent
/// </summary>
public partial class Crossfade : MusicTransition
{
    public Crossfade(GcNode parent, float duration) : base(parent, duration)
    {
    }

    public override void Start()
    {
        base.Start();

        // Add all outgoing tracks to the tween
        foreach (var t in Outgoing)
            // If the transitionoid is a terminal track, we can add a property tweener
            if (t.TerminalTrack is { } tt)
                TransitionTween.Parallel().TweenProperty(tt, "Gain", -42, Duration);

        // Add all incoming tracks to the tween
        foreach (var t in Incoming)
        {
            if (t.TerminalTrack is { } tt)
            {
                // * lower track volume
                // * start the track
                // * Add volume tweener
                tt.Gain = -42f; // Sometimes the best solution is the simplest...
                TransitionTween.Parallel().TweenProperty(tt, "Gain", 0, Duration);
            }

            // Regardless, we will want to play the track
            t.Play();
        }

        // Let 'er rip!!!
        CloseTween();
        TransitionTween.Play();
    }
}