namespace ASI.Services.SlurmFortress;

public class SlurmFortressClient : ISlurmFortressClient
{
    public SlurmFortressClient(ISlurmsClient myEntities)
    {
        Slurms = myEntities;
    }

    public ISlurmsClient Slurms { get; }
}
