using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using PRUEBAGS.ApplicationDbContext;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

//CONFIGURACION DE LOS CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});


// Add NLog as the logger provider
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Prueba G.S. Api Net Core", Version = "v1" });

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
