using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using SlurmFortress.Web.Game.State;

namespace SlurmFortress.Web.Game;

public class GameComponent : FluxorComponent
{
    [Inject]
    public IState<GameState> GameState { get; set; }

    [Inject]
    public IState<TickState> TickState { get; set; }
 
    [Inject]
    public IDispatcher Dispatcher { get; set; } 

    [Inject]
    public ILoggerFactory LoggerFactory { get; set; } 
    public ILogger Logger => LoggerFactory.CreateLogger<GameComponent>();



    //protected override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    if (firstRender)
    //        return base.OnAfterRenderAsync(firstRender);

    //    TickState.StateChanged += (o, t) =>
    //    {
    //        StateHasChanged();
    //    };
    //    return base.OnAfterRenderAsync(firstRender);
    //}

    protected override Task OnInitializedAsync()
    {
        TickState.StateChanged += (o, t) =>
        {
            StateHasChanged();
        };
        return base.OnInitializedAsync();
    }
}