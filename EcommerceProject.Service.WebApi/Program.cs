using EcommerceProject.Service.WebApi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
//Create Entity Framework Migration
//dotnet ef migrations add CreateInitialScheme --project EcommerceProject.Persistence --startup-project EcommerceProject.Service.WebApi --output-dir Migrations --context ApplicationDbContext
//Update Database
//dotnet ef database update --project EcommerceProject.Persistence --startup-project EcommerceProject.Service.WebApi --context ApplicationDbContext
builder.Services.AddPersistenceServices(builder.Configuration); 
builder.Services.AddApplicationServices();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddFeature(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddVersioning();
//http://localhost:5000/healthchecks-ui#/healthchecks
builder.Services.AddHealthCheck(builder.Configuration);
//http://localhost:5000/watchdog
builder.Services.AddWatchDog(builder.Configuration);
//http://localhost:8001/redis-stack/browser
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddRateLimiting(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        //builder a swagger endpoint for each discovered API Version
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseWatchDogExceptionLogger();
app.UseCors("policyApiEcommerce");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseEndpoints(endpoints => { });
app.MapControllers();
app.MapHealthChecksUI();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseWatchDog(conf =>
{
    conf.WatchPageUsername = builder.Configuration["WatchDog:WatchPageUsername"];
    conf.WatchPagePassword = builder.Configuration["WatchDog:WatchPagePassword"];
});

app.Run();