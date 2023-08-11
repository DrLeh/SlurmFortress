using Fluxor;

namespace SlurmFortress.Web.Game.State;

public record BreedState
{
}

public class BreedFeature : Feature<BreedState>
{
    public override string GetName() => GetType().Name;

    protected override BreedState GetInitialState()
    {
        return new BreedState
        {
        };
    }
}
public record BreedAction();

public static class BreedReducers
{
    [ReducerMethod(typeof(BreedAction))]
    public static BreedState Breed(BreedState state)
    {
        return state with
        {
        };
    }
}

public class BreedEffects
{
    public BreedEffects()
    {

    }

    //[EffectMethod]
    //public async Task BreedAsync(BreedFeature action, IDispatcher dispatcher)
    //{
    //    //denote that the page is loading
    //    //dispatcher.Dispatch(new BreedLoadingAction(true));
    //    //try
    //    //{
    //    //    //perform the Breed by making an HTTP call
    //    //    var res = await _BreedClient.BreedAsync(new BreedRequest
    //    //    {
    //    //        UserName = action.UserName,
    //    //        Password = action.Password,
    //    //        KickOut = true
    //    //    });
    //    //    //store the access token in a service where it can be accessed later
    //    //    _webTokenProvider.SetToken(res.AccessToken);
    //    //    //denote that Breed succeeded
    //    //    dispatcher.Dispatch(new BreedSuccessAction());
    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    //denote that Breed failed
    //    //    dispatcher.Dispatch(new BreedFailedAction());
    //    //}
    //    ////denote that page is done loading
    //    //dispatcher.Dispatch(new BreedLoadingAction(false));
    //}
}