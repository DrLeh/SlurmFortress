namespace SlurmFortress.Web.Pages.Game;

public partial class SlurmCounter
{

    protected override async Task OnInitializedAsync()
    {
        GameState.StateChanged += (o, t) =>
        {
            Logger.LogDebug("SlurmCounter State Changed");
            StateHasChanged();
        };
    }

}