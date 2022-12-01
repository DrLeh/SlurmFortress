using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Graph;
using SlurmFortress.Core;
using System.Security.Claims;

namespace SlurmFortress.Web.Security;

/// <summary>
/// Custom Account claims factory class
/// </summary>
public class CustomAccountFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
{
    private readonly ILogger<CustomAccountFactory> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;

    public CustomAccountFactory(IAccessTokenProviderAccessor accessor,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<CustomAccountFactory> logger)
        : base(accessor)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    /// <summary>
    /// Checks for group claims and converts to appRole claims if present.
    /// If group overage claim exists then calls Graph API to check if
    /// user is assigned to authorized groups from config
    /// </summary>
    public async override ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);
        if (initialUser.Identity != null && initialUser.Identity.IsAuthenticated)
        {
            var userIdentity = (ClaimsIdentity)initialUser.Identity;

            var roleGroups = new Dictionary<string, string>();
            configuration.Bind("AuthorizationGroups", roleGroups);

            if (roleGroups.Any())
            {
                var groupIds = roleGroups.Keys.AsEnumerable();
                IEnumerable<string> memberGroups;

                if (account.AdditionalProperties.Any(x => x.Key == "hasgroups" || (x.Key == "_claim_names" && x.Value.ToString() == "{\"groups\":\"src1\"}")))
                {
                    var graphClient = ActivatorUtilities
                        .CreateInstance<GraphServiceClient>(serviceProvider);

                    var batchSize = 20;

                    var tasks = new List<Task<IDirectoryObjectCheckMemberGroupsCollectionPage>>();
                    foreach (var groupsBatch in groupIds.Batch(batchSize))
                    {
                        tasks.Add(graphClient.Me.CheckMemberGroups(groupsBatch).Request().PostAsync());
                    }
                    await Task.WhenAll(tasks);

                    memberGroups = tasks.SelectMany(x => x.Result.ToList());
                }
                else
                {
                    memberGroups = account.Groups.Where(x => groupIds.Contains(x));
                }

                if (memberGroups != null)
                {
                    var claims = memberGroups.Select(groupGuid => new Claim("appRole", roleGroups[groupGuid]));
                    if (claims != null)
                    {
                        userIdentity.AddClaims(claims);
                    }
                }
            }
        }

        return initialUser;
    }
}
