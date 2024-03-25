using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SampleSpaceDal.Repositories;

public class BaseRepository
{
    private readonly IConfiguration _configuration;
    
    protected BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_configuration.GetConnectionString("sample_space"));
    }
}