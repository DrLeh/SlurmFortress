using Refit;
using System.Runtime.CompilerServices;

namespace ASI.Services.SlurmFortress;

public class SlurmsClient : ISlurmsClient
{
    private readonly ISlurmsApi _api;

    public SlurmsClient(ISlurmsApi api)
    {
        _api = api;
    }

    private async Task<T> RunAndValidate<T>(Func<Task<T>> request, [CallerMemberName] string? methodName = null)
    {
        try
        {
            var res = await request();
            return res;
        }
        catch (ApiException e) when (e.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            throw new SlurmFortressBadRequestException(e.Content, e, methodName);
        }
        catch (ApiException e) when (e.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            throw new SlurmFortressInternalServerErrorException(e.Content, e, methodName);
        }
        catch (ApiException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw;
        }
    }

    public virtual async Task<SlurmView> AddSlurmAsync(SlurmView view)
    {
        return await RunAndValidate(async () =>
        {
            var res = await _api.AddSlurmAsync(view).ConfigureAwait(false);
            return res;
        }).ConfigureAwait(false);
    }

    public virtual async Task<SlurmView> UpdateSlurmAsync(SlurmView view)
    {
        return await RunAndValidate(async () =>
        {
            var res = await _api.UpdateSlurmAsync(view.Id, view);
            return res;
        }).ConfigureAwait(false);
    }

    public virtual async Task<SlurmView> GetSlurmAsync(Guid id)
    {
        return await RunAndValidate(async () =>
        {
            var res = await _api.GetSlurmAsync(id);
            return res;
        }).ConfigureAwait(false);
    }

    public virtual async Task<SearchResultView<SlurmSearchView>> SearchAsync(SearchCriteriaView request)
    {
        return await RunAndValidate(async () =>
        {
            var res = await _api.SearchAsync(request);
            return res;
        }).ConfigureAwait(false);
    }

    public virtual async Task<SlurmView> GetSampleEntityAsync()
    {
        return await RunAndValidate(async () =>
        {
            var res = await _api.GetSampleEntityAsync();
            return res;
        }).ConfigureAwait(false);
    }
}
