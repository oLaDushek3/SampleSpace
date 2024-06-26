using SampleSpaceBll.Abstractions.Auth;
using SampleSpaceBll.Abstractions.AuthScheme;
using SampleSpaceBll.Abstractions.Email;
using SampleSpaceBll.Abstractions.Sample;
using SampleSpaceBll.Services;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Abstractions.Redis.Repositories;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceDal.CloudStorage;
using SampleSpaceDal.PostgreSQL.Repositories.PlaylistRepository;
using SampleSpaceDal.PostgreSQL.Repositories.SampleCommentRepository;
using SampleSpaceDal.Redis.Repositories.AuthTokensRepository;
using SampleSpaceInfrastructure;
using SampleSpaceInfrastructure.Auth;
using SampleSpaceInfrastructure.AuthScheme;
using SampleSpaceInfrastructure.AuthScheme.Cookie;
using SampleSpaceInfrastructure.AuthScheme.Token;
using SampleSpaceInfrastructure.Email;
using SampleSpaceInfrastructure.Sample;
using IPostgreSQLSampleRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.ISampleRepository;
using PostgreSQLSampleRepository = SampleSpaceDal.PostgreSQL.Repositories.SampleRepository.SampleRepository;
using ICloudStorageSampleRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.ISampleRepository;
using CloudStorageSampleRepository = SampleSpaceDal.CloudStorage.Repositories.SampleRepository.SampleRepository;
using IPostgreSQLUsersRepository = SampleSpaceCore.Abstractions.PostgreSQL.Repositories.IUsersRepository;
using PostgreSQLUsersRepository = SampleSpaceDal.PostgreSQL.Repositories.UserRepository.UsersRepository;
using ICloudStorageUserRepository = SampleSpaceCore.Abstractions.CloudStorage.Repositories.IUserRepository;
using CloudStorageUserRepository = SampleSpaceDal.CloudStorage.Repositories.UserRepository.UserRepository;

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
    services.AddScoped<IPostgreSQLUsersRepository, PostgreSQLUsersRepository>();
    services.AddScoped<IPostgreSQLSampleRepository, PostgreSQLSampleRepository>();
    services.AddScoped<ISampleCommentRepository, SampleCommentRepository>();
    services.AddScoped<IPlaylistRepository, PlaylistRepository>();
    
    //CloudStorage
    services.AddScoped<ICloudStorageSampleRepository, CloudStorageSampleRepository>();
    services.AddScoped<ICloudStorageUserRepository, CloudStorageUserRepository>();
    
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
    services.AddScoped<IPasswordValidation, PasswordValidation>();
    services.AddScoped<ITokenManager, TokenManager>();
    services.AddScoped<ICookieManager, CookieManager>();
    services.AddScoped<IEmailService, EmailService>();
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

void ConfigureRedis(IServiceCollection services)
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "158.160.171.213:6379";
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

services.Configure<AuthTokensOptions>(configuration.GetSection(nameof(AuthTokensOptions)));
services.Configure<FfMpegOptions>(configuration.GetSection(nameof(FfMpegOptions)));
services.Configure<CloudStorageOptions>(configuration.GetSection(nameof(CloudStorageOptions)));
services.Configure<EmailServiceOptions>(configuration.GetSection(nameof(EmailServiceOptions)));

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