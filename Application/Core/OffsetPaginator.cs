namespace Application.Core;

public class OffsetPaginator<T> where T : class
{
    public async List<T> paginate(DataContext context, int pageNumber, int limit)
    {
        var currentPosition = pageNumber*limit;

        return await _context
            .OrderBy(item => item.Id)
            .Skip(currentPosition)
            .Take(limit)
            .ToListAsync();
    }
}