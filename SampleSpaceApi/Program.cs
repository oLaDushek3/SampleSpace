using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Services;
using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceDal.Repositories.PlaylistRepository;
using SampleSpaceDal.Repositories.SampleCommentRepository;
using SampleSpaceDal.Repositories.SampleRepository;
using SampleSpaceDal.Repositories.UserRepository;
using SampleSpaceInfrastructure;

void AddApiAuthentication(IServiceCollection services, IConfiguration configuration)
{
    var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];

                return Task.CompletedTask;
            }
        };
    });

    services.AddAuthentication();
}

void ConfigureRepositories(IServiceCollection services)
{
    services.AddScoped<IUsersRepository, UsersRepository>();
    services.AddScoped<ISampleRepository, SampleRepository>();
    services.AddScoped<ISampleCommentRepository, SampleCommentRepository>();
    services.AddScoped<IPlaylistRepository, PlaylistRepository>();
}

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ISampleService, SampleService>();
    services.AddScoped<ISampleCommentServices, SampleCommentService>();
    services.AddScoped<IPlaylistService, PlaylistService>();
}

void ConfigureInfrastructure(IServiceCollection services)
{
    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddScoped<IJwtProvider, JwtProvider>();
}

void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            corsPolicyBuilder =>
            {
                corsPolicyBuilder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
    });
}

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

AddApiAuthentication(services, configuration);

ConfigureRepositories(services);

ConfigureServices(services);

ConfigureInfrastructure(services);

ConfigureCors(services);

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();