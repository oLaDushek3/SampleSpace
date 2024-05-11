using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Models;
using SampleSpaceDal.Entities;

namespace SampleSpaceDal.Repositories.SampleRepository;

public class SampleRepository(IConfiguration configuration) : BaseRepository(configuration), ISampleRepository
{
    public async Task<(List<Sample>? samples, string error)> GetAll()
    {
        var queryString = "select * from samples";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection);

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var samples = new List<Sample>();
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

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null);

                if (!string.IsNullOrEmpty(error))
                    return (null, error);
                
                samples.Add(sample!);
            }

            await reader.CloseAsync();

            return (samples, string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<(List<Sample>? samples, string error)> Search(string searchString)
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

            var samples = new List<Sample>();
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

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null);

                if (!string.IsNullOrEmpty(error))
                    return (null, error);
                
                samples.Add(sample!);
            }

            await reader.CloseAsync();

            return (samples, string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
    
    public async Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid)
    {
        var queryString = "select samples.* " +
                          "from playlist_samples " +
                          "join samples on playlist_samples.sample_guid = samples.sample_guid " +
                          "where playlist_samples.playlist_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = playlistGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var samples = new List<Sample>();
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

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null);

                if (!string.IsNullOrEmpty(error))
                    return (null, error);
                
                samples.Add(sample!);
            }

            await reader.CloseAsync();

            return (samples, string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<(Sample? sample, string error)> GetByGuid(Guid sampleGuid)
    {
        var queryString =
            "select * from samples left join users on samples.user_guid = users.user_guid where samples.sample_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = sampleGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            
            if (!reader.HasRows)
                return (null, "Sample not found");
            
            var sampleEntity = new SampleEntity();

            await reader.ReadAsync();
            
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

            sampleEntity.User = new UserEntity
            {
                UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                AvatarPath = reader.GetString(reader.GetOrdinal("avatar_path")),
            };

            await reader.CloseAsync();

            var (sampleUser, sampleUserError) = User.Create(sampleEntity.User.UserGuid, sampleEntity.User.Nickname,
                sampleEntity.User.Email, null, sampleEntity.User.AvatarPath);

            if (!string.IsNullOrEmpty(sampleUserError))
                return (null, sampleUserError);

            var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, sampleUser);

            return !string.IsNullOrEmpty(error) ? (null, error) : (sample, string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid)
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

            var samples = new List<Sample>();
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

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null);

                if (!string.IsNullOrEmpty(error))
                    return (null, error);
                
                samples.Add(sample!);
            }

            await reader.CloseAsync();

            return (samples, string.Empty);
        }
        catch (Exception exception)
        {
            return (null, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<(bool successfully, string error)> AddAnListens(Guid sampleGuid)
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

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return successfully ? (successfully, string.Empty) : (successfully, "Sample not found");
        }
        catch (Exception exception)
        {
            return (false, exception.Message);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}