using Hypesoft.Infrastructure.Data;
using Hypesoft.Infrastructure.Repositories;
using Hypesoft.Domain.Repositories;
using Serilog;
using MongoDB.Driver;
using MediatR;
using AutoMapper;
using Hypesoft.Application.Products.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Mostrar PII para debug de token
IdentityModelEventSource.ShowPII = true;

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// MongoDb
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IProductRepository, MongoProductRepository>();
builder.Services.AddScoped<ICategoryRepository, MongoCategoryRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Hypesoft.Application.Mapping.ProductProfile).Assembly);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductCommand>());

// Health Checks com MongoDb
var mongoConnectionString = builder.Configuration["MongoSettings:ConnectionString"];
var mongoDatabaseName = builder.Configuration["MongoSettings:DatabaseName"];
var mongoFullConnectionString = $"{mongoConnectionString}/{mongoDatabaseName}";

builder.Services.AddHealthChecks()
    .AddMongoDb(sp => new MongoClient(mongoFullConnectionString), name: "mongodb");

// 🔑 Configuração Keycloak (JWT) - dev-friendly
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],

            ValidateAudience = false, // desativa audience pra dev
            ValidateLifetime = true,

            // Define onde o .NET vai buscar os roles no token Keycloak
            RoleClaimType = "realm_access.roles"
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Controllers + Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
