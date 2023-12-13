using System.Text.Json.Serialization;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = 
        JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddApplicationServices(builder.Configuration);


builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});



// app.UseHttpsRedirection();
// app.UseAuthorization();
// app.MapControllers();

//Create scope with using, which means that when everything is done in the scope is gonna be deleted from RAM (Garbage collector called)
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;



try{
    //Update the DB if needed = not exists => create, SQL updated => update
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    // await context.Database.EnsureCreatedAsync();
}catch (Exception e){
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "Error during migration or data seeding");
}

app.Run();