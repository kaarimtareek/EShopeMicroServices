namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity> : List<TEntity> where TEntity : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedResult(IEnumerable<TEntity> items, long count, int pageIndex, int pageSize)
    {
        TotalCount = count;
        PageIndex = pageIndex;
        PageSize = pageSize;
        AddRange(items);
    }
}