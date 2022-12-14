using Refit;
using static DLeh.Services.SlurmFortress.SlurmFortressConstants;

namespace DLeh.Services.SlurmFortress;

/// <summary>
/// Refit client definition
/// </summary>
public interface ISlurmsApi
{
    [Get(SlurmsBasePath)]
    Task<SlurmView> GetSampleEntityAsync();

    [Post(SlurmsBasePath + "/")]
    Task<SlurmView> AddSlurmAsync(SlurmView view);

    [Put(SlurmsBasePath + "/{id}")]
    Task<SlurmView> UpdateSlurmAsync(Guid id, SlurmView view);

    [Get(SlurmsBasePath + "/{id}")]
    Task<SlurmView> GetSlurmAsync(Guid id);

    [Get(SlurmsBasePath + "/search")]
    Task<SearchResultView<SlurmSearchView>> SearchAsync(SearchCriteriaView request);
}
