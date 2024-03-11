namespace PhraseFinder.Domain.Extensions;

public static class PaginationExtensions
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page, int elementsPerPage)
    {
        return source.Skip((page - 1) * elementsPerPage).Take(elementsPerPage);
    }

    public static int GetTotalPages<T>(this IEnumerable<T> source, int elementsPerPage)
    {
        return (int)Math.Ceiling(source.Count() / (double)elementsPerPage);
    }
}