using System;

namespace RecNForget.Services.Contracts.Events;

public class TimerStateToggleEventArgs
{
    public bool TimerIsRunning { get; private set; }

    public TimerStateToggleEventArgs(bool timerIsRunning)
    {
        TimerIsRunning = timerIsRunning;
    }
}
