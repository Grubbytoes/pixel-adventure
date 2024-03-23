using System;
using System.Collections.Generic;
using Godot;

namespace GoConductor;

public abstract partial class MultiMusicPlayer : GcMusicNode
{
    /// <summary>
    /// Finds the track of the given name
    /// </summary>
    /// <param name="trackName">The name of the track</param>
    /// <returns>The track, or null if it cannot be found</returns>
    protected GcMusicNode GetTrack(string trackName)
    {
        var found = GetNode(trackName);

        if (found is GcMusicNode track)
        {
            return track;
        }

        return null;
    }

    /// <summary>
    /// Finds the track of the given index. Note that indexing INCLUDES any NON track children of the conductor, if any
    /// exists.
    /// </summary>
    /// <param name="idx"></param>
    /// <returns>The track, or null if the index is out of bounds, or points to a non track node</returns>
    protected GcMusicNode GetTrack(int idx)
    {
        if (idx < GetChildCount() && GetChild(idx) is GcMusicNode track)
        {
            return track;
        }

        return null;
    }

    /// <summary>
    /// Finds the given track by name, and calls the Cue method on it
    /// </summary>
    /// <param name="trackName"></param>
    /// <returns>True if found</returns>
    public bool CueName(string trackName)
    {
        var track = GetTrack(trackName);
        return track != null && Cue(track);
    }

    /// <summary>
    /// Finds the given track by index, and calls the Cue method on it
    /// </summary>
    /// <param name="idx"></param>
    /// <returns>True if found</returns>
    public bool CueIdx(int idx)
    {
        var track = GetTrack(idx);
        return track != null && Cue(track);
    }

    public abstract bool Cue(GcMusicNode trackName);
    public abstract IEnumerable<GcMusicNode> GetVisibleTracks();
}