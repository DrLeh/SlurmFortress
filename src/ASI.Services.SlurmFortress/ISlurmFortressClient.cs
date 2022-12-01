namespace ASI.Services.SlurmFortress;

public interface ISlurmFortressClient
{
    ISlurmsClient Slurms { get; }
}
