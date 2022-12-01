using SlurmFortress.Core.Models;

namespace SlurmFortress.Web.Game;

public class GameState
{
    private readonly GameTimer _timer;
    private readonly ILogger<GameState> _log;

    public GameState(GameTimer timer,ILogger<GameState> log)
    {
        _timer = timer;
        _log = log;
        _timer.TimerChanged += Tick;
    }

    public void Tick(object? sender, TimerEventArgs e)
    {
        Queen.Produce();
        _log.LogInformation($"Queen slurms: {Queen.Children.Count()}");
        StateChanged?.Invoke(this, new());
    }

    public QueenSlurm Queen { get; private set; } = new QueenSlurm();

    public List<Slurm> AllSlurms => Queen.Children;

    public event EventHandler<EventArgs> StateChanged;
}


