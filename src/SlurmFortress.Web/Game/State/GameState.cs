using Fluxor;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Web.Game.State;

public record GameState
{
    public QueenSlurm Queen { get; private set; } = new QueenSlurm();

    public List<Slurm> AllSlurms => Queen.Children;

    /// <summary>
    /// Have some new change so that we get a new state
    /// </summary>
    public Guid UpdateId { get; set; }
    public GameState New() => this with
    {
        UpdateId = Guid.NewGuid()
    };
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

public record GameUpdateAction();

public static class GameReducers
{
    [ReducerMethod(typeof(GameUpdateAction))]
    public static GameState Update(GameState state)
    {
        return state.New();
    }

    [ReducerMethod(typeof(TickAction))]
    public static GameState Tick(GameState state)
    {
        state.Queen.Spawn();
        return state.New();
    }

    [ReducerMethod(typeof(BreedAction))]
    public static GameState Breed(GameState state)
    {
        state.Queen.Spawn();
        return state.New();
    }
}
//public class GameEffects
//{
//    public GameEffects()
//    {

//    }

//    [EffectMethod]
//    public async Task Breed(BreedAction action, GameState state, IDispatcher dispatcher)
//    {
//        state.Queen.Spawn();
//        dispatcher.Dispatch(new GameUpdateAction());
//    }
//}