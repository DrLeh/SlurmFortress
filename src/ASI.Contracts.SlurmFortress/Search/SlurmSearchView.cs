namespace ASI.Contracts.SlurmFortress.Search;

public sealed class SlurmSearchView
{
    public long Id { get; set; }
    //This will have a null value for now because we cannot set it at this time, but by denoting it as non-nullable,
    // it signifies that the fulfilment of this contract will ensure this value is set
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
