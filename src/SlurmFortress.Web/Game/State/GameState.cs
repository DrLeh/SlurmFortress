using Fluxor;
using log4net;
using Microsoft.AspNetCore.Components;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Web.Game.State;

public class GameState
{
    public QueenSlurm Queen { get; private set; } = new QueenSlurm();

    public List<Slurm> AllSlurms => Queen.Children;
}

public class GameFeature : Feature<GameState>
{
    public override string GetName() => GetType().Name;

    protected override GameState GetInitialState()
    {
        return new GameState
        {
        };
    }
}

public static class GameReducers
{
    [ReducerMethod(typeof(TickAction))]
    public static GameState Tick(GameState state)
    {
        state.Queen.Spawn();
        return state;
    }

    [ReducerMethod(typeof(BreedAction))]
    public static GameState Breed(GameState state)
    {
        state.Queen.Spawn();
        return state;
    }
}