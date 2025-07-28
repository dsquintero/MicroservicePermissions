using FluentValidation;
using MicroservicePermissions.Api.Middlewares;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Application.Mappings;
using MicroservicePermissions.Domain.Repositories;
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

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorios
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

//UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Validators
builder.Services.AddScoped<IValidator<CreatePermissionCommand>, CreatePermissionCommandValidator>();


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

