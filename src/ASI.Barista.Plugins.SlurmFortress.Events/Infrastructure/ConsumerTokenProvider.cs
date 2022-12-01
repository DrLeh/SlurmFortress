//using ASI.Services.Http.Security;
//using ASI.Services.Security;
//using Microsoft.Extensions.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using SlurmFortress.Core.Context;
//using SlurmFortress.Core.Data;

//namespace ASI.Barista.Plugins.SlurmFortress.Events;

//public class ConsumerTokenProvider : IAsiTokenProvider
//{
//    private readonly IAsiLoginClient _loginClient;
//    private readonly ITenantContext _tenantContext;

//    public ConsumerTokenProvider(IAsiLoginClient loginClient, ITenantContext tenantContext)
//    {
//        _loginClient = loginClient;
//        _tenantContext = tenantContext;
//    }

//    public string GetToken()
//    {
//        var res = _loginClient.LoginWithClientCredentials(_tenantContext.TenantId)
//            .ConfigureAwait(false)
//            .GetAwaiter().GetResult();

//        return res.AccessToken;
//    }
//}
