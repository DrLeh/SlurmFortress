using Microsoft.AspNetCore.Mvc.Testing;

namespace SlurmFortress.Web.Api.Test;

public class WebAppFactoryTest : WebApplicationFactory<Startup>
{
    /// <summary>
    /// Asserts that the web api can be created successfully
    /// </summary>
    [Fact]
    public void Dependencies_resolved()
    {
        Func<IServiceProvider> func = () => Services;
        func.Should().NotThrow();
    }
}
