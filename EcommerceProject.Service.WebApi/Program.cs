using EcommerceProject.Service.WebApi;
using EcommerceProject.Service.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string myPolicy = "policyApiEcommerce";

var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

builder.Services.RegisterServices();
builder.Services.AddSingleton<IConfiguration>(Configuration);


var appSettingsSection = Configuration.GetSection("Config");

builder.Services.Configure<AppSettings>(appSettingsSection);

var appsettings = appSettingsSection.Get<AppSettings>();


builder.Services.AddCors(options => options.AddPolicy(myPolicy, builder => builder.WithOrigins(appsettings.OriginCors)
                                                                                   .AllowAnyHeader()
                                                                                   .AllowAnyMethod()));
builder.Services.RegisterAuthentication(appsettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors(myPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
