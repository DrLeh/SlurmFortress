namespace ASI.Contracts.SlurmFortress.Search;

[Serializable]
public sealed class SearchCriteriaView
{
    public long? Id { get; set; }
    /// <summary>
    /// Paging start
    /// </summary>
    public int? From { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    public int? Size { get; set; }
    /// <summary>
    /// Search term for wildcard filtering
    /// </summary>
    public string? Term { get; set; }
    public string? Type { get; set; }
    /// <summary>
    /// Ids to exlude from results
    /// </summary>
    public string? ExcludeList { get; set; }
    public string? Status { get; set; }
    public bool AggregationsOnly { get; set; }
    public string? SortBy { get; set; }
    /// <summary>
    /// Only return results that user is able to edit
    /// </summary>
    public bool EditOnly { get; set; }

    public Dictionary<string, SearchFilterView>? Filters { get; set; }

}

public class SearchFilterView
{
    public List<string>? Terms { get; set; }
    public string? Behavior { get; set; }
    public string? TermTransformation { get; set; }
}