using System.Runtime.CompilerServices;

namespace ASI.Services.SlurmFortress;

public class SlurmFortressBadRequestException : SlurmFortressException
{
    public SlurmFortressBadRequestException(string? message, [CallerMemberName] string? methodName = null)
        : base($"SlurmFortress 400 Bad Request at '{methodName}': {message}") { }

    public SlurmFortressBadRequestException(string? message, Exception exception, [CallerMemberName] string? methodName = null)
        : base($"SlurmFortress 400 Bad Request at '{methodName}': {message ?? "no message"}", exception) { }

    public SlurmFortressBadRequestException(string path, string? message, Exception exception)
        : base($"SlurmFortress 400 Bad Request at '{path}': {message ?? "no message"}", exception) { }
}
