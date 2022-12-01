namespace SlurmFortress.Web;

public class SlurmFortressWebConfig
{
    public string EnvironmentName { get; set; } = "DEV";
    public bool IsProd => EnvironmentName == "PROD";
    public string[] Scopes { get; set; } = Array.Empty<string>();
}
