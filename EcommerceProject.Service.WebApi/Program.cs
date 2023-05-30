using EcommerceProject.Service.WebApi;

var builder = WebApplication.CreateBuilder(args);
string myPolicy = "policyApiEcommerce";

var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

builder.Services.RegisterServices();
builder.Services.AddSingleton<IConfiguration>(Configuration);
builder.Services.AddCors(options => options.AddPolicy(myPolicy, builder => builder.WithOrigins(Configuration["Config:OriginCors"])
                                                                                   .AllowAnyHeader()
                                                                                   .AllowAnyMethod()));
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
