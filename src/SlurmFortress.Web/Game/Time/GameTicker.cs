using Fluxor;
using SlurmFortress.Web.Game.State;

namespace SlurmFortress.Web.Game.Time;

/// <summary>
/// Triggers a <see cref="TickAction"/> on every <see cref="GameTimer.TimerChanged"/> event."/>
/// </summary>
public class GameTicker
{
    private readonly GameTimer _timer;

    public IDispatcher Dispatcher
    {
        get; set;
    }
    public GameTicker(GameTimer timer, IDispatcher dispatcher)
    {
        _timer = timer;
        Dispatcher = dispatcher;
        _timer.TimerChanged += Tick;
    }

    public void Tick(object? sender, TimerEventArgs e)
    {
        Dispatcher.Dispatch(new TickAction());
    }
}