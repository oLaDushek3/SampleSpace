using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Abstractions.Sample;
using SampleSpaceBll.Services;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceDal.CloudStorage;
using SampleSpaceDal.PostgreSQL.Repositories.PlaylistRepository;
using SampleSpaceDal.PostgreSQL.Repositories.SampleCommentRepository;
using SampleSpaceDal.PostgreSQL.Repositories.UserRepository;
using SampleSpaceInfrastructure;
using SampleSpaceInfrastructure.JWT;
using IPostgreSQLSampleRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.ISampleRepository;
using PostgreSQLSampleRepository = SampleSpaceDal.PostgreSQL.Repositories.SampleRepository.SampleRepository;
using ICloudStorageSampleRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.ISampleRepository;
using CloudStorageSampleRepository = SampleSpaceDal.CloudStorage.Repositories.SampleRepository.SampleRepository;

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
    //PostgreSQL
    services.AddScoped<IUsersRepository, UsersRepository>();
    services.AddScoped<IPostgreSQLSampleRepository, PostgreSQLSampleRepository>();
    services.AddScoped<ISampleCommentRepository, SampleCommentRepository>();
    services.AddScoped<IPlaylistRepository, PlaylistRepository>();
    
    //CloudStorage
    services.AddScoped<ICloudStorageSampleRepository, CloudStorageSampleRepository>();
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
    services.AddScoped<ISampleTrimmer, SampleTrimmer>();
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
services.Configure<CloudStorageOptions>(configuration.GetSection(nameof(CloudStorageOptions)));

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