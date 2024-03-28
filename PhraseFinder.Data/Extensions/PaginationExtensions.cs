namespace PhraseFinder.Data.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> items, int page, int pageSize)
    {
        return items.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static int GetTotalPages<T>(this IQueryable<T> items, int pageSize)
    {
        return (int)Math.Ceiling((double)items.Count() / pageSize);
    }
}