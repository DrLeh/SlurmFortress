namespace SlurmFortress.Algolia.Indexing.Exceptions;

public class IndexStrategyNotFoundException : Exception
{
    public IndexStrategyNotFoundException(string typeName)
        : base($"Could not find IndexOperationStrategy for type for {typeName}")
    {

    }
}

public sealed class IndexStrategyNotFoundException<T> : IndexStrategyNotFoundException
{
    public IndexStrategyNotFoundException()
        : base(typeof(T).Name)
    {

    }
}
