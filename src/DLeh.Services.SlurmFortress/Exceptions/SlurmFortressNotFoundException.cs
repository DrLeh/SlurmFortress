namespace DLeh.Services.SlurmFortress;

public class SlurmFortressNotFoundException : SlurmFortressException
{
    public SlurmFortressNotFoundException(string path) : base($"404 Not Found: {path}") { }
}
