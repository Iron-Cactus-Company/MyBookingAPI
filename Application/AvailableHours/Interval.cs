namespace Application.AvailableHours;

public class Interval
{
    public Interval(long start, long end)
    {
        _start = start;
        _end = end;
    }
    
    private long _start;
    public long Start
    {
        get => _start;
        set { if (value <= _end) _start = value; }
    }
    
    private long _end;
    public long End
    {
        get => _end;
        set { if (value >= _start) _end = value; }
    }

    public bool IsLongerThan(long duration) => (End - Start) >= duration;
    public bool IsShorterThan(long duration) => (End - Start) < duration;
    public long GetDuration() => (End - Start);

    //Determine there the given interval is placed relating to this
    public IntervalPlaceType PlaceInterval(Interval interval)
    {
        // | - param interval point, . this interval point, //// - intervals overlap mark, ---- - line
        
        //param interval is outside of this
        // ---|///|--.------.----- or ---.---.--|///|--
        if (interval.End <= Start || interval.Start >= End)
            return IntervalPlaceType.Outside;
        
        //param interval is inside of this
        // ---.--|///|---.---
        if (interval.Start > Start && interval.End < End)
            return IntervalPlaceType.Inside;

        //param interval is partially overlapping this on the right side
        // ---.----|////.///|----
        if (interval.Start > Start && interval.End >= End)
            return IntervalPlaceType.RightOverlap;
        
        //param interval is partially overlapping this on the left side
        // ---|///.///|----.---
        if (interval.Start <= Start && interval.End < End)
            return IntervalPlaceType.LeftOverlap;

        //param interval overlaps the whole this interval
        // ---|//////.//////.////|---
        return IntervalPlaceType.Overlap;
    }

    /*
        The method removes overlapping part from this interval
        If intervals are not overlapping just return this interval
        If param interval is inside of this interval, it returns two new parts of this interval, which are not overlapping
        If param interval is only partially overlapping this interval, e.g. from start to middle => returns part of this interval from middle to end
        If param interval completely overlaps this interval nothing will be returned
    */
    public (Interval first, Interval second) RemoveOverlapping(Interval intervalToRemove)
    {
        var place = PlaceInterval(intervalToRemove);

        if (place == IntervalPlaceType.Outside)
            return (this, null);
        
        if (place == IntervalPlaceType.Inside)
            return (new Interval(Start, intervalToRemove.Start), new Interval(intervalToRemove.End, End));

        if (place == IntervalPlaceType.RightOverlap)
            return (new Interval(Start, intervalToRemove.Start), null);

        if (place == IntervalPlaceType.LeftOverlap)
            return (new Interval(intervalToRemove.End, End), null);

        return (null, null);
    }
    
    public Interval GetOverlapping(Interval overlapToFind)
    {
        var place = PlaceInterval(overlapToFind);

        if (place == IntervalPlaceType.Outside)
            return null;
        
        if (place == IntervalPlaceType.Inside)
            return overlapToFind;

        if (place == IntervalPlaceType.RightOverlap)
            return new Interval(overlapToFind.Start, End);

        if (place == IntervalPlaceType.LeftOverlap)
            return new Interval(Start, overlapToFind.End);

        return this;
    }
    
    
    public override string ToString()
    {
        return Start + " - " + End;
    }
}