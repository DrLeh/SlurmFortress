namespace SlurmFortress.Web.Game.Time;

public class TimerEventArgs : EventArgs
{
    public TimerEventArgs(long ticks)
    {
        Ticks = ticks;
    }

    public long Ticks { get; private set; }
}

public class GameTimer
{
    private GameTimer()
    {

    }
    public static GameTimer Instance { get; private set; } = new();

    public event EventHandler<TimerEventArgs> TimerChanged;

    public float _currentTime;
    public float CurrentTime
    {
        get
        {
            lock (locker)
                return _currentTime;
        }
    }
    private static int TickRate = 1000;
    private long LastTicks;
    public long CurrentTicks
    {
        get
        {
            lock (locker)
                return (long)(_currentTime / TickRate);
        }
    }

    private static object locker = new();
    public void Tick(object? state) => Increment();
    public void Increment()
    {
        lock (locker)
            _currentTime++;
    }

    public bool SetTime(float time)
    {
        lock (locker)
        {
            LastTicks = CurrentTicks;
            _currentTime = time;
            var ticked = CurrentTicks != LastTicks;
            if (ticked)
                TimerChanged?.Invoke(this, new(CurrentTicks));
            return ticked;
        }
    }
}
