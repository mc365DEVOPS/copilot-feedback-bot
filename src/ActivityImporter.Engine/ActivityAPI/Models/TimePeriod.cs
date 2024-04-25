﻿namespace ActivityImporter.Engine.ActivityAPI.Models;

/// <summary>
/// Data class used to define a time period
/// TODO: There may be a system type that could be used instead
/// </summary>
public class TimePeriod
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public TimePeriod(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public bool InRange(DateTime dt)
    {
        return Start <= dt && End >= dt;
    }

    public override string ToString()
    {
        return string.Format("{0} -> {1}", Start, End);
    }


    /// <summary>
    /// Enumerates the period of time were retrieving metadata for bearing in mind the configuration
    /// and the maximum chunk size and earliest date supported by the API
    /// </summary>
    /// <returns></returns>
    public static List<TimePeriod> GetScanningTimeChunksFrom(DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new ArgumentOutOfRangeException();
        }

        // We can only extract up to 1 day at a time
        var chunkSize = new TimeSpan(24, 0, 0);

        if (chunkSize.TotalSeconds == 0)
        {
            throw new InvalidOperationException("Chunk size time-span is 0");
        }

        // Enumerate the chunks of time
        var start = from;
        DateTime end;

        var timeChunks = new List<TimePeriod>();

        while (start < to)
        {
            // Set the end of the chunk
            end = start.Add(chunkSize);
            if (end > to)
            {
                end = to;
            }

            // Return the value
            timeChunks.Add(new TimePeriod(start, end));


            // Move forwards
            start = end;
        }

        // Hack: remove most recent time-chunk as it's likely too small a window, and will generate an error in Activity API
        timeChunks.RemoveAt(timeChunks.Count - 1);

        return timeChunks;
    }
}
