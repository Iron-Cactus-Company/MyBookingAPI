using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Core;

public class GuidHandler
{
    public static bool IsGuidNull(Guid? guid)
    {
        return string.IsNullOrEmpty(guid.ToString());
    }
    
    public static async Task<bool> IsEntityExists<T>(Guid? entityId, DataContext context) where T : class
    {
        if (IsGuidNull(entityId))
            return false;
        
        return await context.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == entityId);
    }
}