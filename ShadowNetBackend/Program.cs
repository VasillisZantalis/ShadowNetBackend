using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ShadowNetBackend.Behaviors;
using ShadowNetBackend.Features.Messages;
using ShadowNetBackend.Middleware;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<RedisCacheSettings>(builder.Configuration.GetSection("RedisCacheSettings"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<Agent, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("RedisCacheSettings:ConnectionString").Value;
    options.InstanceName = "Shadownet_";
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<ICryptographyService, CryptographyService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<JwtHelper>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapAuthEndpoints();
app.MapAgentEndpoints();
app.MapMissionEndpoints();
app.MapWitnessEndpoints();
app.MapSafeHouseEndpoints();
app.MapMessageEndpoints();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapHub<ChatHub>("/chatHub");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference(options =>
{
    options.Title = "ShadowNet API";
});

app.UseHttpsRedirection();

app.Run();