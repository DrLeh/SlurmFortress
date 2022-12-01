using System.Runtime.Serialization;

namespace ASI.Services.SlurmFortress;

public class SlurmFortressException : Exception
{
    public SlurmFortressException()
    {
    }

    public SlurmFortressException(string? message) : base(message)
    {
    }

    public SlurmFortressException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SlurmFortressException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
