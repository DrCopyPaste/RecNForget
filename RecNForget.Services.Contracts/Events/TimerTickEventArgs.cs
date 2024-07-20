using System;

namespace RecNForget.Services.Contracts.Events;

public class TimerTickEventArgs
{
    public TimeSpan CurrentTimeSpan { get; private set; }

    public TimerTickEventArgs(TimeSpan currentTimeSpan)
    {
        CurrentTimeSpan = currentTimeSpan;
    }
}
