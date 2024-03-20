using System.Collections.Generic;
using Godot;

namespace GoConductor;

public partial class MusicConductor : MultiMusicPlayer
{
    private List<GcMusicNode> TracksCurrentlyPlaying { get; set; }
    
    // Basically the first element in TracksCurrentlyPlaying is the lead
    public GcMusicNode LeadTrack
    {
        get {
            if (TracksCurrentlyPlaying.Count < 1)
            {
                return null;
            }
            return TracksCurrentlyPlaying[0];
        }
        private set => TracksCurrentlyPlaying.Insert(0, value);
    }

    public override float PlaybackPosition
    {
        get => LeadTrack.PlaybackPosition;
        set
        {
            foreach (GcMusicNode t in TracksCurrentlyPlaying)
            {
                t.PlaybackPosition = value;
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        TracksCurrentlyPlaying = new List<GcMusicNode>();
        LeadTrack = GetChild(0) as MusicTrack;
    }

    /// <summary>
    /// Cues the track if it is not playing, cues out if it is
    /// </summary>
    /// <param name="newTrack">Name of the track</param>
    /// <returns>True if track successfully found and acted upon</returns>
    public override bool Cue(GcMusicNode newTrack)
    {
        // Try cueing the track
        bool success = CueIn(newTrack);
        
        // If that didn't work, maybe its already playing
        if (!success)
        {
            success = CueOut(newTrack);
        }
        
        // If either was successful, success should be true
        return success;
    }

    private bool CueOut(GcMusicNode track)
    {
        int trackOutIdx = TracksCurrentlyPlaying.IndexOf(track);

        if (trackOutIdx < 0)
        {
            return false;
        }
        
        track.Stop();
        TracksCurrentlyPlaying.RemoveAt(trackOutIdx);
        return true;
    }

    private bool CueIn(GcMusicNode track)
    {
        // Track not found or track already playing
        if (track == null || TracksCurrentlyPlaying.Contains(track) )
        {
            return false;
        }
        
        // Append track to currently playing, so we can see it later
        TracksCurrentlyPlaying.Add(track);
        
        // Do we need to play the track?
        if (Playing)
        {
            track.PlayFrom(PlaybackPosition);
        }
        else
        {
            // I changed this from playhead, if it breaks in the future
            track.PlaybackPosition = PlaybackPosition;
        }

        return true;
    }

    public override IEnumerable<GcMusicNode> GetVisibleTracks()
    {
        return TracksCurrentlyPlaying;
    }

    /// <summary>
    /// Adds the track to the arrangement, if it is found
    /// </summary>
    /// <param name="trackName">Name of track to play</param>
    /// <returns>True if track successfully found and added to arrangement</returns>
    public bool CueInName(string trackName)
    {
        GcMusicNode newTrack = GetTrack(trackName);
        return CueIn(newTrack);
    }

    public bool CueInIdx(int idx)
    {
        var track = GetTrack(idx);
        return CueIn(track);
    }

    /// <summary>
    /// Removes the track, by name, from the arrangement
    /// </summary>
    /// <param name="trackName">Track name</param>
    /// <returns>True if found and removed from arrangement successfully</returns>
    public bool CueOutName(string trackName)
    {
        GcMusicNode track = GetTrack(trackName);
        return CueOut(track);
    }

    public bool CueOutIdx(int idx)
    {
        var track = GetTrack(idx);
        return CueOut(track);
    }

    public override void Play()
    {
        base.Play();
        foreach (var t in TracksCurrentlyPlaying)
        {
            t.Play();
        }
    }

    public override void Pause()
    {
        base.Pause();
        foreach (GcMusicNode t in TracksCurrentlyPlaying)
        {
            t.Pause();
        }
    }

    public override void Stop()
    {
        base.Stop();
        foreach (GcMusicNode t in TracksCurrentlyPlaying)
        {
            t.Stop();
        }
    }

    public override void PlayFrom(float position)
    {
        foreach (var t in TracksCurrentlyPlaying)
        {
            t.PlayFrom(position);
        }
        base.Play();
    }
}