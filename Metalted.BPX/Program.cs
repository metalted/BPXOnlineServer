using System.Text;
using Metalted.BPX.Authentication;
using Metalted.BPX.Blueprints;
using Metalted.BPX.Data;
using Metalted.BPX.DataStore;
using Metalted.BPX.Jwt;
using Metalted.BPX.Steam;
using Metalted.BPX.Storage;
using Metalted.BPX.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SteamWebAPI2.Utilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, provider, configuration) =>
    {
        configuration
            .WriteTo.Console()
            .MinimumLevel.Debug();
    });

// Add services to the container.

builder.Services.AddAuthentication(
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = "https://bpx.metalted.com",
                ValidAudience = "https://bpx.metalted.com",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetSection(JwtOptions.Key)[nameof(JwtOptions.Token)]!)),
#if DEBUG
                // Disable validation for easier debugging
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false
#else
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
#endif
            };
        });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
        });
    });

builder.Services.AddNpgsql<BpxContext>(
    builder.Configuration["Database:ConnectionString"],
    options => { options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });

builder.Services.AddScoped<IDatabase, Database>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddScoped<IBlueprintService, BlueprintService>();
builder.Services.AddScoped<IBlueprintRepository, BlueprintRepository>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ISteamService, SteamService>();
builder.Services.AddScoped<ISteamWebInterfaceFactory, SteamWebInterfaceFactory>(
    provider =>
    {
        SteamOptions options = provider.GetRequiredService<IOptions<SteamOptions>>().Value;
        return new SteamWebInterfaceFactory(options.ApiKey);
    });

builder.Services.AddScoped<IStorageService, StorageService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.Key));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Key));
builder.Services.Configure<SteamOptions>(builder.Configuration.GetSection(SteamOptions.Key));
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(StorageOptions.Key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(
    new ForwardedHeadersOptions()
    {
        ForwardedHeaders = ForwardedHeaders.All
    });

app.UseCors(
    cors => cors
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
