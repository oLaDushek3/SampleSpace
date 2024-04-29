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
                    SampleLink = reader.GetString(reader.GetOrdinal("sample_link")),
                    CoverLink = reader.GetString(reader.GetOrdinal("cover_link")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration")),
                    VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link")),
                    SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link")),
                    SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens).Sample);
            }

            await reader.CloseAsync();
            
            return sampleEntities;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
    
    public async Task<Sample?> GetByGuid(Guid sampleGuid)
    {
        var queryString = "select * from samples where sample_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = sampleGuid} }
        };
        
        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var sampleEntity = new SampleEntity();
            
            await reader.ReadAsync();
            if (!reader.HasRows)
                return null;
            
            sampleEntity.SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid"));
            sampleEntity.SampleLink = reader.GetString(reader.GetOrdinal("sample_link"));
            sampleEntity.CoverLink = reader.GetString(reader.GetOrdinal("cover_link"));
            sampleEntity.Name = reader.GetString(reader.GetOrdinal("name"));
            sampleEntity.Artist = reader.GetString(reader.GetOrdinal("artist"));
            sampleEntity.UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid"));
            sampleEntity.Duration = reader.GetDouble(reader.GetOrdinal("duration"));
            sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
            sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
            sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));
            sampleEntity.NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens"));

            await reader.CloseAsync();
            
            return Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens).Sample;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Sample>> Search(string searchString)
    {
        var queryString = "select * from samples where name ilike $1 or artist ilike $1";

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
                    SampleLink = reader.GetString(reader.GetOrdinal("sample_link")),
                    CoverLink = reader.GetString(reader.GetOrdinal("cover_link")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration")),
                    VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link")),
                    SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link")),
                    SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens).Sample);
            }

            await reader.CloseAsync();
            
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
                    SampleLink = reader.GetString(reader.GetOrdinal("sample_link")),
                    CoverLink = reader.GetString(reader.GetOrdinal("cover_link")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Artist = reader.GetString(reader.GetOrdinal("artist")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Duration = reader.GetDouble(reader.GetOrdinal("duration")),
                    VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link")),
                    SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link")),
                    SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link")),
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                };

                sampleEntities.Add(Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens).Sample);
            }

            await reader.CloseAsync();
            
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