using API.Service;
using Application.BusinessProfileActions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    //Add extension method for adding all needed services to builder
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        //Add DB
        services.AddDbContext<DataContext>(opt => 
        {
            // opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            
        });
        //Should be one time thing, all mediators will be registered
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetMany.Handler).Assembly));
        //Add automapper for update requests
        //services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddScoped<PermissionHelper>();
        services.AddScoped<NotificationService>();
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CORSPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        return services;
    }
}