using Elastic.Clients.Elasticsearch;
using FluentValidation;
using MicroservicePermissions.Api.Middlewares;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Application.Mappings;
using MicroservicePermissions.Domain.Repositories;
using MicroservicePermissions.Infrastructure.Elasticsearch;
using MicroservicePermissions.Infrastructure.Kafka;
using MicroservicePermissions.Infrastructure.Persistence;
using MicroservicePermissions.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Reemplaza el logger por Serilog
builder.Host.UseSerilog();

var assemblies = new[] { Assembly.GetExecutingAssembly() };
var elasticUri = builder.Configuration["Elasticsearch:Uri"];

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//ElasticsearchClient
builder.Services.AddSingleton(provider =>
{
    var settings = new ElasticsearchClientSettings(new Uri(elasticUri));
    var client = new ElasticsearchClient(settings);
    return client;
});
builder.Services.AddScoped<IElasticPermissionIndexer, ElasticPermissionIndexer>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorios
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();

//UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Validators
builder.Services.AddScoped<IValidator<CreatePermissionCommand>, CreatePermissionCommandValidator>();
builder.Services.AddScoped<IValidator<CreatePermissionTypeCommand>, CreatePermissionTypeCommandValidator>();

//Kafka
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();


// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
});

// AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<PermissionProfile>();
});



var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

