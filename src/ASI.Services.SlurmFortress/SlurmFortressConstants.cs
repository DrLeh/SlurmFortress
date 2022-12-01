namespace ASI.Services.SlurmFortress;

internal static class SlurmFortressConstants
{
    //these paths should not include /api. for babou they wouldn't have /api
    // so this allows us to config to e.g. myapp.com/api or babou.com/myapp/
    public const string SlurmsBasePath = "/myentities";
}
