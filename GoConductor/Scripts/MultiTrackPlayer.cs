using System.Collections.Generic;

namespace GoConductor;

public abstract partial class MultiTrackPlayer : GcNode
{
    /// <summary>
    ///     Finds the track of the given name
    /// </summary>
    /// <param name="trackName">The name of the track</param>
    /// <returns>The track, or null if it cannot be found</returns>
    protected GcNode GetTrack(string trackName)
    {
        var found = GetNode(trackName);

        if (found is GcNode track) return track;

        return null;
    }

    /// <summary>
    ///     Finds the track of the given index. Note that indexing INCLUDES any NON track children of the conductor, if any
    ///     exists.
    /// </summary>
    /// <param name="idx"></param>
    /// <returns>The track, or null if the index is out of bounds, or points to a non track node</returns>
    protected GcNode GetTrack(int idx)
    {
        if (idx < GetChildCount() && GetChild(idx) is GcNode track) return track;

        return null;
    }

    /// <summary>
    ///     Finds the given track by name, and calls the Cue method on it
    /// </summary>
    /// <param name="trackName"></param>
    /// <returns>True if found</returns>
    public bool CueName(string trackName)
    {
        var track = GetTrack(trackName);
        return track != null && Cue(track);
    }

    /// <summary>
    ///     Finds the given track by index, and calls the Cue method on it
    /// </summary>
    /// <param name="idx"></param>
    /// <returns>True if found</returns>
    public bool CueIdx(int idx)
    {
        var track = GetTrack(idx);
        return track != null && Cue(track);
    }

    public abstract bool Cue(GcNode trackName);
    public abstract IEnumerable<GcNode> GetVisibleTracks();
}