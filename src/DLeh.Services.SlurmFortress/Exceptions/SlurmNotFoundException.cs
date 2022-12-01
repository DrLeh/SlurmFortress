namespace DLeh.Services.SlurmFortress;

public class SlurmNotFoundException : SlurmFortressException
{
    public SlurmNotFoundException(long id) : base($"Entity Id {id} Not Found") { }
}
