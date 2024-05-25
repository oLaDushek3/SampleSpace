using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Abstractions.Sample;
using SampleSpaceBll.Services;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Abstractions.Redis.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceDal.CloudStorage;
using SampleSpaceDal.PostgreSQL.Repositories.PlaylistRepository;
using SampleSpaceDal.PostgreSQL.Repositories.SampleCommentRepository;
using SampleSpaceDal.PostgreSQL.Repositories.UserRepository;
using SampleSpaceDal.Redis.Repositories.AuthTokensRepository;
using SampleSpaceInfrastructure;
using SampleSpaceInfrastructure.AuthScheme;
using SampleSpaceInfrastructure.AuthScheme.Cookie;
using SampleSpaceInfrastructure.AuthScheme.Token;
using SampleSpaceInfrastructure.JWT;
using IPostgreSQLSampleRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.ISampleRepository;
using PostgreSQLSampleRepository = SampleSpaceDal.PostgreSQL.Repositories.SampleRepository.SampleRepository;
using ICloudStorageSampleRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.ISampleRepository;
using CloudStorageSampleRepository = SampleSpaceDal.CloudStorage.Repositories.SampleRepository.SampleRepository;

// void AddApiAuthentication(IServiceCollection services, IConfiguration configuration)
// {
//     var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
//
//     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
//         };
//
//         options.Events = new JwtBearerEvents
//         {
//             OnMessageReceived = context =>
//             {
//                 context.Token = context.Request.Cookies["jwt"];
//
//                 return Task.CompletedTask;
//             }
//         };
//     });
//
//     services.AddAuthentication();
//     services.AddSingleton<AuthTokensOptions>(_ => GetOptions(configuration));
// }

AuthTokensOptions GetOptions(IConfiguration configuration)
{
    return configuration.GetSection("AuthTokensOptions").Get<AuthTokensOptions>()!;
}

void AddApiAuthentication(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(AuthTokensDefault.AuthenticationScheme).AddAuthTokens(
        AuthTokensDefault.AuthenticationScheme,
        _ => GetOptions(configuration));

    services.AddAuthentication();
    
    services.AddSingleton<AuthTokensOptions>(_ => GetOptions(configuration));
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
    
    //Redis
    services.AddScoped<IAuthTokensRepository, AuthTokensRepository>();
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
    services.AddScoped<ITokenManager, TokenManager>();
    services.AddScoped<ICookieManager, CookieManager>();
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

void ConfigureRedis(IServiceCollection services)
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "158.160.169.2:6379";
        options.InstanceName = "sample";
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

ConfigureRedis(services);

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthTokensOptions>(configuration.GetSection(nameof(AuthTokensOptions)));
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