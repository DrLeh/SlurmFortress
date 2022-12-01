using Microsoft.AspNetCore.Components;

namespace SlurmFortress.Web.Game;

public class GameComponent : ComponentBase
{
    [Inject]
    public GameState State { get; set; }
    [Inject]
    public ILoggerFactory LoggerFactory { get; set; }
    public ILogger Logger => LoggerFactory.CreateLogger<GameComponent>();
    


    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        Logger.LogInformation(State.ToString());
        State.StateChanged += (o, t) =>
        {
            StateHasChanged();
        };
        return base.OnAfterRenderAsync(firstRender);
    }
}