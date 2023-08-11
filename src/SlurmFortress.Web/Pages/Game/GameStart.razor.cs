using Fluxor;
using Microsoft.AspNetCore.Components;

namespace SlurmFortress.Web.Pages.Game;

public partial class GameStart
{
    [Inject]
    public GameTicker Ticker { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }

}