namespace GoConductorPlugin.addons.go_conductor__.transition;

// TODO Doesn't work
public partial class FadeOutFadeIn : MusicTransition
{
    public FadeOutFadeIn(GcMusicNode parent, float duration) : base(parent, duration)
    {
    }

    public override void Start()
    {
        base.Start();

        // Microsoft says directly using enumerators is bad but I can't think of a tidier way, may change later
        // We want the first of each not to be parallel, then all the subsequent ones to be parallel w/ the first
        var outE = Outgoing.GetEnumerator();
        var inE = Incoming.GetEnumerator();
        
        MusicTrack t;
        
        // Fade tracks out - add to tween
        if (outE.MoveNext())
        {
            t = outE.Current;
            TransitionTween.TweenProperty(t, "Gain", -30, Duration);
        }
        while (outE.MoveNext())
        {
            t = outE.Current;
            TransitionTween.Parallel().TweenProperty(t, "Gain", -30, Duration);
        }
        
        // Fade tracks in, lower volume first, add to tween
        if (inE.MoveNext())
        {
            t = inE.Current;
            t.Gain = -30;
            TransitionTween.TweenProperty(t, "Gain", 0, Duration);
        }
        while (inE.MoveNext())
        {
            t = inE.Current;
            t.Gain = -30;
            TransitionTween.Parallel().TweenProperty(t, "Gain", 0, Duration);
        }
        
        // Done
        CloseTween();
        outE.Dispose();
        inE.Dispose();
    }
}