using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;
using SampleSpaceDal.Entities;

namespace SampleSpaceDal.Repositories.SampleRepository;

public class SampleRepository(IConfiguration configuration) : BaseRepository(configuration), ISampleRepository
{
    public async Task<List<Sample>> GetAll()
    {
        var queryString = "select * from samples";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection);

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var sampleEntities = new List<Sample>();
            while (await reader.ReadAsync())
            {
                var sampleEntity = new SampleEntity
                {
                    SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid")),
                    SamplePath = reader.GetString(reader.GetOrdinal("sample_path")),
                    CoverPath = reader.GetString(reader.GetOrdinal("cover_path")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Artist = reader.GetString(reader.GetOrdinal("artist"))
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SamplePath,
                    sampleEntity.CoverPath, sampleEntity.Name, sampleEntity.Artist).Sample);
            }

            return sampleEntities;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Sample>> Search(string searchString)
    {
        var queryString = "select * from samples where name ilike $1 or artist like $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = "%" + searchString.ToLower() + "%"} }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var sampleEntities = new List<Sample>();
            while (await reader.ReadAsync())
            {
                var sampleEntity = new SampleEntity
                {
                    SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid")),
                    SamplePath = reader.GetString(reader.GetOrdinal("sample_path")),
                    CoverPath = reader.GetString(reader.GetOrdinal("cover_path")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Artist = reader.GetString(reader.GetOrdinal("artist"))
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SamplePath,
                    sampleEntity.CoverPath, sampleEntity.Name, sampleEntity.Artist).Sample);
            }

            return sampleEntities;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}