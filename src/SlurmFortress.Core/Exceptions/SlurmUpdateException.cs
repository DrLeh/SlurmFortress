namespace SlurmFortress.Core.Helpers.Exceptions;

public sealed class SlurmUpdateException : Exception
{
    public string Name { get; }

    public SlurmUpdateException(string name)
    {
        Name = name;
    }
}
