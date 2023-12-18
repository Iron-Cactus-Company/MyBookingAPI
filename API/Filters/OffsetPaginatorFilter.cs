using API.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class OffsetPaginatorFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<OffsetPaginator>().Any())
        {
            var paginateAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<OffsetPaginator>()
                .FirstOrDefault();

            var defaultLimit = paginateAttribute != null ? paginateAttribute.PageSize : 20;
            var maxLimit = paginateAttribute != null ? paginateAttribute.MaxSize : 20;
            
            var limit = context.HttpContext.Request.Query.ContainsKey("limit")
                ? int.Parse(context.HttpContext.Request.Query["limit"])
                : defaultLimit;
            if (limit > maxLimit)
                limit = maxLimit;

            var page = context.HttpContext.Request.Query.ContainsKey("page")
                ? int.Parse(context.HttpContext.Request.Query["page"])
                : 1;

            context.HttpContext.Items["limit"] = limit;
            context.HttpContext.Items["page"] = page;
        }

        await next();
    }
}