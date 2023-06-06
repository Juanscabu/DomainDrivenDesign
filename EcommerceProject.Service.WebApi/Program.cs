using EcommerceProject.Service.WebApi;
using EcommerceProject.Service.WebApi.Helpers;

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
builder.Services.RegisterSwagger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountOwner API V1");
    });
}

app.UseCors(myPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


//docker image build -t ecommerce:1.0.0 -f ./EcommerceProject.Service.WebApi/Dockerfile .

// docker container run --name ecommerce -d -p 5000

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//}
//);