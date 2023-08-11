using Fluxor;

namespace SlurmFortress.Web.Game.State;

public record TickState
{
    public long Ticks { get; set; }
}

public class TickFeature : Feature<TickState>
{
    public override string GetName() => "TickFeature";

    protected override TickState GetInitialState()
    {
        return new TickState
        {
            Ticks = 0
        };
    }
}
public record TickAction();

public static class TickReducers
{
    [ReducerMethod(typeof(TickAction))]
    public static TickState Tick(TickState state)
    {
        return state with
        {
            Ticks = state.Ticks + 1
        };
    }
}

public class TickEffects
{
    public TickEffects()
    {

    }

    //[EffectMethod]
    //public async Task TickAsync(TickFeature action, IDispatcher dispatcher)
    //{
    //    //denote that the page is loading
    //    //dispatcher.Dispatch(new TickLoadingAction(true));
    //    //try
    //    //{
    //    //    //perform the Tick by making an HTTP call
    //    //    var res = await _TickClient.TickAsync(new TickRequest
    //    //    {
    //    //        UserName = action.UserName,
    //    //        Password = action.Password,
    //    //        KickOut = true
    //    //    });
    //    //    //store the access token in a service where it can be accessed later
    //    //    _webTokenProvider.SetToken(res.AccessToken);
    //    //    //denote that Tick succeeded
    //    //    dispatcher.Dispatch(new TickSuccessAction());
    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    //denote that Tick failed
    //    //    dispatcher.Dispatch(new TickFailedAction());
    //    //}
    //    ////denote that page is done loading
    //    //dispatcher.Dispatch(new TickLoadingAction(false));
    //}
}