using Domain;
using Microsoft.EntityFrameworkCore;
//using System.Linq.Dynamic.Core;

namespace Application.Core;

public class OffsetPaginator<T> where T : class, IEntityWithId
{
    public IQueryable<T> Paginate(IQueryable<T> dbSet, int pageNumber, int limit)
    {
        var currentPosition = (pageNumber-1)*limit >= 0 ? (pageNumber-1)*limit : 0;

        return dbSet
            .OrderBy(item => item.Id)
            .Skip(currentPosition)
            .Take(limit);
    }

    public (int pageNumber, int limit) DeterminePageNumberAndSize(ReadOptions? options)
    {
        int defaultLimit = 20;
        if (options is null)
        {
            return (1, defaultLimit);
        }
        
        var pageNumber = options.PageNumber is null ? 1 : (int)options.PageNumber;
        var limit = options.Limit is null ? defaultLimit : (int)options.Limit;
        
        return (pageNumber, limit);
    }
}