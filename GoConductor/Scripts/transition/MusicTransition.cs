using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GoConductorPlugin.addons.go_conductor__.transition;


/// <summary>
/// The base class for anything that involves stopping one track and starting another. All music switches should have
/// one as a child.
/// </summary>
public abstract partial class MusicTransition : Node
{
    [Signal]
    public delegate void DoneEventHandler();

    public MusicTransition(GcMusicNode parent, float duration)
    {
        Parent = parent;
        Duration = duration;
        Incoming = new HashSet<MusicTrack>();
        Outgoing = new HashSet<MusicTrack>();
    }

    /// <summary>
    /// The set of all track come in in the transition
    /// </summary>
    protected HashSet<MusicTrack> Incoming { private set; get; }

    /// <summary>
    /// The set of all tracks being stopped by this transition
    /// </summary>
    protected HashSet<MusicTrack> Outgoing { private set; get; }

    /// <summary>
    /// The node which called the transition
    /// </summary>
    public GcMusicNode Parent { get; private set; }

    /// <summary>
    /// How long the transition should last for
    /// </summary>
    public float Duration { get; set; }

    protected Tween TransitionTween { get; set; }

    public virtual void Start()
    {
        // Warn if being started with empty incoming or outgoing set
        if (Incoming.Count < 1)
        {
            GD.PushWarning("Transition being started with no incoming track(s) set");
        }
        if (Outgoing.Count < 1)
        {
            GD.PushWarning("Transition being started with no outgoing track(s) set");
        }
        
        // Create a tween as a child of the parent
        TransitionTween = Parent.CreateTween();
    }

    /// <summary>
    /// To be called after all desired tweeners have been added to the tween, adds the callback.
    /// You CAN continue to append tweeners - but they will be after effects, this marks the point at which the transition
    /// is considered done and the closing signal is emmited.
    /// </summary>
    protected void CloseTween()
    {
        // Add callbacks
        TransitionTween.TweenCallback(Callable.From(TransitionDone));
    }

    /// <summary>
    /// Kills the transition tween
    /// </summary>
    public virtual void Kill()
    {
        TransitionTween?.Kill();
        // Remember to stop all the outgoing tracks so they don't linger as ghosts...!
        foreach (var t in Outgoing)
        {
            t.Stop();
        }
    }

    private void TransitionDone()
    {
        // Stop all outgoing tracks
        foreach (var t in Outgoing)
        {
            t.Stop();
        }
        
        // Send the signal, baby!
        GD.Print("DONE");
        EmitSignal(MusicTransition.SignalName.Done);
    }

    /// <summary>
    /// Adds a new track to the set of incoming tracks, so long as it is not already in either set
    /// </summary>
    /// <param name="musicNode">The track to add</param>
    /// <returns>True if track added</returns>
    public virtual bool AddIncomingTrack(GcMusicNode musicNode)
    {
        // Check if the new node is a single track
        if (musicNode is MusicTrack track)
        {
            // Guard against duplicate tracks
            if (Incoming.Contains(track) || Outgoing.Contains(track)) return false;
            
            // Add the track
            Incoming.Add(track);
            return true;
        }
        
        // Else see if it is multi-track
        if (musicNode is MultiMusicPlayer multiTrack)
        {
            return AddIncomingTrack(multiTrack);
        }
        
        // Otherwise fail
        return false;
    }

    /// <summary>
    /// Adds a new track to the set of outgoing tracks, so long as it is not already in either set
    /// </summary>
    /// <param name="musicNode">The track to add</param>
    /// <returns>True if track added</returns>
    public virtual bool AddOutgoingTrack(GcMusicNode musicNode)
    {
        // Check if the new node is a single track
        if (musicNode is MusicTrack track)
        {
            // Guard against duplicate tracks
            if (Incoming.Contains(track) || Outgoing.Contains(track)) return false;
            
            // Add the track
            Outgoing.Add(track);
            return true;
        }
        
        // Else see if it is multi-track
        if (musicNode is MultiMusicPlayer multiTrack)
        {
            GD.Print("BEEP");
            return AddOutgoingTrack(multiTrack);
        }
        
        // Otherwise fail
        return false;
    }

    /// <summary>
    /// Adds all child tracks of a multi-music player node, those that are not already in either set.
    /// Note that even if this method returns false some track may have been added, it's just a sign that something may
    /// not have worked the way you intended - more of a warning that an outright failure
    /// </summary>
    /// <param name="musicTrack">The parent of all new tracks to add</param>
    /// <returns>True if all new tracks added okay</returns>
    public virtual bool AddIncomingTrack(MultiMusicPlayer musicTrack)
    {
        bool addedOk = true;

        foreach (GcMusicNode t in musicTrack.GetVisibleTracks())
        {
            addedOk = (addedOk && AddIncomingTrack(t));
        }

        return addedOk;
    }

    /// <summary>
    /// Adds all child tracks of a multi-music player node, those that are not already in either set.
    /// See warning for NewIncomingTrack
    /// </summary>
    /// <param name="musicTrack">The parent of all new tracks to add</param>
    /// <returns>True if all new tracks added okay</returns>
    public virtual bool AddOutgoingTrack(MultiMusicPlayer musicTrack)
    {
        bool addedOk = true;

        foreach (GcMusicNode t in musicTrack.GetVisibleTracks())
        {
            addedOk = (addedOk && AddOutgoingTrack(t));
        }

        return addedOk;
    }
}