using EcommerceProject.Service.WebApi;
using EcommerceProject.Service.WebApi.Helpers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

string myPolicy = "policyApiEcommerce";
var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
builder.Services.RegisterServices();
builder.Services.AddSingleton<IConfiguration>(Configuration);

var appSettingsSection = Configuration.GetSection("Config");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

builder.Services.AddAuthentication(appSettings);
builder.Services.AddFeature(appSettings);
builder.Services.AddSwagger();
builder.Services.AddVersioning();
builder.Services.AddMapper();
builder.Services.AddValidator();
//http://localhost:5000/healthchecks-ui#/healthchecks
builder.Services.AddHealthCheck(Configuration);


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
//app.UseHttpsRedirection();
app.UseCors(myPolicy);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecksUI();
    endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});
app.MapControllers();

app.Run();


//docker image build -t ecommerce:1.0.0 -f ./EcommerceProject.Service.WebApi/Dockerfile .

// docker container run --name ecommerce -d -p 5000

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//}
//);