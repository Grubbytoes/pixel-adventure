using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace GoConductor;

[Tool]
public partial class MusicConductor : MultiTrackPlayer
{
    [Export] public Array<bool> StupidBoolArray = new();

    /// <summary>
    ///     A list of all the tracks currently playing
    /// </summary>
    private List<GcNode> TracksCurrentlyPlaying { get; set; }

    /// <summary>
    ///     A pointer to the track from which all others will take their timing cues - defined here as the first element
    ///     in the list TracksCurrentlyPlaying
    /// </summary>
    public GcNode LeadTrack
    {
        get
        {
            if (TracksCurrentlyPlaying.Count < 1) return null;
            return TracksCurrentlyPlaying[0];
        }
        private set => TracksCurrentlyPlaying.Insert(0, value);
    }

    /// <summary>
    ///     The position in time the arrangement is currently at. Is read from LeadTrack defined above. When written the
    ///     is set for ALL TracksCurrentlyPlaying.
    /// </summary>
    public override float PlaybackPosition
    {
        get
        {
            if (LeadTrack?.PlaybackPosition is { } p)
                return p;
            return 0f;
            ;
        }
        set
        {
            foreach (var t in TracksCurrentlyPlaying) t.PlaybackPosition = value;
        }
    }

    public override void _Ready()
    {
        TracksCurrentlyPlaying = new List<GcNode>();

        // Making sure stupid bool array is the right size
        var childCount = GetChildCount();
        var arrayCount = StupidBoolArray.Count;
        while (arrayCount < childCount)
        {
            arrayCount += 1;
            StupidBoolArray.Add(false);
        }

        while (arrayCount > childCount)
        {
            arrayCount -= 1;
            StupidBoolArray.RemoveAt(arrayCount);
        }

        LeadTrack = GetChild(0) as MusicTrack;
    }

    /// <summary>
    ///     Cues the track if it is not playing, cues out if it is
    /// </summary>
    /// <param name="newTrack">Name of the track</param>
    /// <returns>True if track successfully found and acted upon</returns>
    public override bool Cue(GcNode newTrack)
    {
        // Try cueing the track
        var success = CueIn(newTrack);

        // If that didn't work, maybe its already playing
        if (!success) success = CueOut(newTrack);

        // If either was successful, success should be true
        return success;
    }

    /// <summary>
    ///     Removes the given track from TracksCurrentlyPlaying, stopping any audio in the process
    /// </summary>
    /// <param name="track">The track to remove</param>
    /// <returns>True if track found and cued out</returns>
    private bool CueOut(GcNode track)
    {
        var trackOutIdx = TracksCurrentlyPlaying.IndexOf(track);

        // Update bool array
        StupidBoolArray[track.GetIndex()] = false;

        if (trackOutIdx < 0) return false;

        track.Stop();
        TracksCurrentlyPlaying.RemoveAt(trackOutIdx);
        return true;
    }

    /// <summary>
    ///     Adds the given to TracksCurrentlyPlaying, playing it in time with the arrangement if the conductor is playing.
    ///     If no other tracks are playing when this track is added, it becomes the LeadTrack.
    /// </summary>
    /// <param name="track">The track to play</param>
    /// <returns>True if cued in successfully</returns>
    private bool CueIn(GcNode track)
    {
        // Track not found or track already playing
        if (track == null || TracksCurrentlyPlaying.Contains(track)) return false;

        // Update bool array
        StupidBoolArray[track.GetIndex()] = true;


        // Append track to currently playing, so we can see it later
        TracksCurrentlyPlaying.Add(track);

        // Do we need to play the track?
        if (Playing)
            track.PlayFrom(PlaybackPosition);
        else
            // I changed this from playhead, if it breaks in the future
            track.PlaybackPosition = PlaybackPosition;

        return true;
    }

    public override IEnumerable<GcNode> GetVisibleTracks()
    {
        return TracksCurrentlyPlaying;
    }

    /// <summary>
    ///     Adds the track to the arrangement, if it is found by name
    /// </summary>
    /// <param name="trackName">Name of track to play</param>
    /// <returns>True if track successfully found and added to arrangement</returns>
    public bool CueInName(string trackName)
    {
        var newTrack = GetTrack(trackName);
        return CueIn(newTrack);
    }

    /// <summary>
    ///     Cues the track in, if it can be found, by index
    /// </summary>
    /// <param name="idx">Track index</param>
    /// <returns>True if found and cued in</returns>
    public bool CueInIdx(int idx)
    {
        var track = GetTrack(idx);
        return CueIn(track);
    }

    /// <summary>
    ///     Removes the track, by name, from the arrangement
    /// </summary>
    /// <param name="trackName">Track name</param>
    /// <returns>True if found and removed from arrangement successfully</returns>
    public bool CueOutName(string trackName)
    {
        var track = GetTrack(trackName);
        return CueOut(track);
    }

    /// <summary>
    ///     Removes the track form the arrangement, if found, by it's index
    /// </summary>
    /// <param name="idx">Track index</param>
    /// <returns>True if found and cued out</returns>
    public bool CueOutIdx(int idx)
    {
        var track = GetTrack(idx);
        return CueOut(track);
    }

    public override void Play()
    {
        base.Play();
        foreach (var t in TracksCurrentlyPlaying) t.Play();
    }

    public override void Pause()
    {
        base.Pause();
        foreach (var t in TracksCurrentlyPlaying) t.Pause();
    }

    public override void Stop()
    {
        base.Stop();
        foreach (var t in TracksCurrentlyPlaying) t.Stop();
    }

    public override void PlayFrom(float position)
    {
        foreach (var t in TracksCurrentlyPlaying) t.PlayFrom(position);
        base.Play();
    }
}