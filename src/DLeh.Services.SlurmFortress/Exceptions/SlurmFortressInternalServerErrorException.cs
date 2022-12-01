using System.Runtime.CompilerServices;

namespace DLeh.Services.SlurmFortress;

public class SlurmFortressInternalServerErrorException : SlurmFortressException
{
    public SlurmFortressInternalServerErrorException(string path, string message)
        : base($"SlurmFortress  500 Internal Server Error at '{path}': {message}") { }

    public SlurmFortressInternalServerErrorException(string path, string message, Exception exception)
        : base($"SlurmFortress  500 Internal Server Error at '{path}': {message}", exception) { }

    public SlurmFortressInternalServerErrorException(string? message, Exception exception, [CallerMemberName] string? methodName = null)
        : base($"SlurmFortress  500 Internal Server Error at '{methodName}': {message}", exception) { }
}
