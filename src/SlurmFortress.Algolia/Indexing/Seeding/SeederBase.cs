using ASI.Services.Algolia.Client;
using ASI.Services.Search.Indexing;
using Microsoft.Extensions.Logging;
using SlurmFortress.Core.Data;
using SlurmFortress.Core.Models;
using System.Diagnostics;

namespace SlurmFortress.Algolia.Indexing.Seeding;

public abstract class SeederBase<T> : ISeeder<T>
    where T : class, IEntity
{
    protected readonly IDataAccess _dataAccess;
    protected readonly ISearchIndexProvider _algoliaIndexProvider;
    protected readonly ILogger<SeederBase<T>>? _logger;
    protected readonly ISearchTransactionProvider _searchTransactionProvider;
    protected readonly ISearchIndexNameProvider _searchIndexNameProvider;

    public abstract IndexType Type { get; }

    //private ISearchIndex? _index;
    //public virtual ISearchIndex Index => _index ??= _algoliaIndexProvider.GetIndex<TSearch>();

    public virtual int PageSize => 1000;

    public SeederBase(
        IDataAccess dataAccess,
        ISearchIndexProvider algoliaIndexProvider,
        ISearchTransactionProvider searchTransactionProvider,
        ISearchIndexNameProvider searchIndexNameProvider,
        ILoggerFactory loggerFactory
        )
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        _algoliaIndexProvider = algoliaIndexProvider ?? throw new ArgumentNullException(nameof(algoliaIndexProvider));
        _searchTransactionProvider = searchTransactionProvider ?? throw new ArgumentNullException(nameof(searchTransactionProvider));
        _searchIndexNameProvider = searchIndexNameProvider ?? throw new ArgumentNullException(nameof(searchIndexNameProvider));
        _logger = loggerFactory?.CreateLogger<SeederBase<T>>();
    }

    public abstract IQueryable<T> BaseQuery(AlgoliaSeedOptions options);

    public abstract IQueryable<T> LoadQuery(int page, AlgoliaSeedOptions options);

    /// <summary>
    /// Perform any other transformations on the loaded data
    /// </summary>
    /// <param name="records"></param>
    public virtual void PostLoadStep(List<T> records) { }

    public virtual void Seed(AlgoliaSeedOptions options)
    {
        //_logger?.LogWarning($"Deleting all objects from {IndexName} index");
        DeleteExistingRecords(options);
        SeedData(options);
    }

    /// <summary>
    /// Seeds the entire data set specified by the options. Para
    /// </summary>
    /// <param name="options"></param>
    protected virtual void SeedData(AlgoliaSeedOptions options)
    {
        var paging = GetPagingData(options);
        var totalPages = paging.pageCount;
        var fullStop = Stopwatch.StartNew();

        var pOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        var pageRange = Enumerable.Range(options.StartPage, paging.pageCount);
        Parallel.ForEach(pageRange, pOptions, page =>
        {
            TrySeedPage(options, page, paging.pageSize, paging.pageCount);
        });

        fullStop.Stop();
        _logger?.LogInformation($"Finished seeding {Type} in {fullStop.Elapsed:g}");
    }

    protected virtual void TrySeedPage(AlgoliaSeedOptions options, int page, int pageSize, int totalPages, int maxAttempts = 5)
    {
        try
        {
            SeedPage(options, page, pageSize, totalPages);
        }
        catch (Exception e)
        {
            //if you're getting repeated exceptions, try lowering the page size
            _logger?.LogError(e, $"Exception occurred seeding {Type} Page: {page}");
        }
    }

    /// <summary>
    /// Override this method if you need full control over how to load the data and send it to Algolia
    /// </summary>
    protected virtual void SeedPage(AlgoliaSeedOptions options, int page, int pageSize, int totalPages)
    {
        var stop = Stopwatch.StartNew();
        _logger?.LogInformation($"Started seeding page: {page}/{totalPages}");
        var records = LoadQuery(page, options)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        PostLoadStep(records);

        var trans = _searchTransactionProvider.CreateTransaction();
        foreach (var record in records)
            trans.Add(record);

        trans.Commit();
        stop.Stop();
        _logger?.LogInformation($"Finished seeding page {page} in {stop.Elapsed:g}");
    }

    /// <summary>
    /// Delete all records for the given type
    /// </summary>
    protected virtual void DeleteExistingRecords(AlgoliaSeedOptions options)
    {
        //if (options.TenantId == 0)
        //{
        //var delResponse = Index.ClearObjects();
        //_logger?.LogWarning($"Deleted all objects from {IndexName} index");
        //example of how you could do tenanted filtering to reseed only one tenant
        //}
        //else
        //{
        //    var delResponse = Index.DeleteBy(new Query
        //    {
        //        Filters = $"tenantId:{options.TenantId}"
        //    });
        //    _logger.LogWarning($"Deleted all objects from {IndexName} index for tenant: {options.TenantId}");
        //}
    }

    /// <summary>
    /// Gets the amount of pages to run given the page size of the options or the type,
    /// and the number of records for the type in the database
    /// </summary>
    protected virtual (int pageSize, int pageCount) GetPagingData(AlgoliaSeedOptions options)
    {
        var pageSize = options.PageSize == 0 ? PageSize : options.PageSize;
        var queryBase = BaseQuery(options);
        var total = queryBase.Count();
        var totalPages = (int)Math.Ceiling(total / (double)PageSize);
        return (pageSize, totalPages - options.StartPage);
    }
}
