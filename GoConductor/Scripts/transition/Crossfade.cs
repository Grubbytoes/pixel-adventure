using System.Collections.Generic;
using Godot;

namespace GoConductorPlugin.addons.go_conductor__.transition;

/// <summary>
/// Once started, fades one or more tracks in or out. Tracks faded out are stopped once the transition is over
///
/// This is ALL IT DOES, any updates to currently playing must be done by the parent
/// </summary>
public partial class Crossfade : MusicTransition
{
    public Crossfade(GcMusicNode parent, float duration) : base(parent, duration)
    {
    }

    public override void Start()
    {
        base.Start();
        
        // Add all outgoing tracks to the tween
        foreach (var t in Outgoing)
        {
            TransitionTween.Parallel().TweenProperty(t, "Gain", -30, Duration);
        }
        
        // Add all incoming tracks to the tween
        foreach (var t in Incoming)
        {
            // * lower track volume
            // * start the track
            // * Add volume tweener
            t.Gain = -30f; // Sometimes the best solution is the simplest...!
            t.Play();
            TransitionTween.Parallel().TweenProperty(t, "Gain", 0, Duration);
        }
        
        // Let 'er rip!!!
        CloseTween();
        TransitionTween.Play();
    }
}