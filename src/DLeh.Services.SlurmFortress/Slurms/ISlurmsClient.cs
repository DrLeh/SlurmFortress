namespace DLeh.Services.SlurmFortress;

public interface ISlurmsClient
{
    Task<SlurmView> AddSlurmAsync(SlurmView view);
    Task<SlurmView> UpdateSlurmAsync(SlurmView view);
    Task<SlurmView> GetSlurmAsync(Guid id);
    Task<SearchResultView<SlurmSearchView>> SearchAsync(SearchCriteriaView request);
    Task<SlurmView> GetSampleEntityAsync();
}
