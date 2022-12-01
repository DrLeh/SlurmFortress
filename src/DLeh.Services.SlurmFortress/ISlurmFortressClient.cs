namespace DLeh.Services.SlurmFortress;

public interface ISlurmFortressClient
{
    ISlurmsClient Slurms { get; }
}
