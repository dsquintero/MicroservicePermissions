using FluentValidation;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Application.Mappings;
using MicroservicePermissions.Domain.Repositories;
using MicroservicePermissions.Infrastructure.Persistence;
using MicroservicePermissions.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
app.UseAuthorization();
app.MapControllers();

app.Run();

