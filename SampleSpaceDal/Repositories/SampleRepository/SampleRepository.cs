using System.Data;
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
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration"))
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SamplePath,
                    sampleEntity.CoverPath, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.NumberOfListens, sampleEntity.Duration).Sample);
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
            Parameters = { new NpgsqlParameter { Value = "%" + searchString.ToLower() + "%" } }
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
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration"))
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SamplePath,
                    sampleEntity.CoverPath, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.NumberOfListens, sampleEntity.Duration).Sample);
            }

            return sampleEntities;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Sample>> GetUserSamples(Guid userGuid)
    {
        var queryString = "select * from samples where user_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = userGuid } }
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
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration"))
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SamplePath,
                    sampleEntity.CoverPath, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.NumberOfListens, sampleEntity.Duration).Sample);
            }

            return sampleEntities;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<bool> AddAnListens(Guid sampleGuid)
    {
        var queryString = "update samples set number_of_listens = number_of_listens + 1 where sample_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = sampleGuid } }
        };

        try
        {
            await connection.OpenAsync();
            
            return await command.ExecuteNonQueryAsync() > 0;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}