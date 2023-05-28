using ArangkadaAPI.Context;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Arangkada API",
        Description = "This is the best API for Driver Rental Management!",
        Contact = new OpenApiContact
        {
            Name = "Adrian Jay Barcenilla, Alys Anthea Carillo, Kerr Labajo, John William Miones, Olivein Kurl Potolin",
            Url = new Uri("https://github.com/CITUCCS/csit341-final-project-group-3-team-yamyam")
        },
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

    services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    // Configure Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Transient -> create a new instance of DapperContext every time.
    services.AddTransient<DapperContext>();

    // Services
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IOperatorService, OperatorService>();
    services.AddScoped<IDriverService, DriverService>();
    services.AddScoped<IVehicleService, VehicleService>();
    services.AddScoped<ITransactionService, TransactionService>();

    // Repository
    services.AddScoped<IOperatorRepository, OperatorRepository>();
    services.AddScoped<IDriverRepository, DriverRepository>();
    services.AddScoped<IVehicleRepository, VehicleRepository>();
    services.AddScoped<ITransactionRepository, TransactionRepository>();
}
