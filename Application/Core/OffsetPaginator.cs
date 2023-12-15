using Microsoft.EntityFrameworkCore;

namespace Application.Core;

public class OffsetPaginator<T> where T : class
{
    public async Task<List<T>> paginate(DbSet<T> dbSet, int pageNumber, int limit)
    {
        var currentPosition = pageNumber*limit;

        return await dbSet
            //.OrderBy(item => item.Id)
            .Skip(currentPosition)
            .Take(limit)
            .ToListAsync();
    }
}