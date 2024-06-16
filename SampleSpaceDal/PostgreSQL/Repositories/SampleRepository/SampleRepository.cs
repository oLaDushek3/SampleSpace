using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Models;
using SampleSpaceCore.Models.Sample;
using SampleSpaceDal.PostgreSQL.Entities;

namespace SampleSpaceDal.PostgreSQL.Repositories.SampleRepository;

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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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

    public async Task<(List<Sample>? samples, string error)> GetSortByDate(int limit, int numberOfPage)
    {
        var queryString = "select * from samples order by date desc " +
                          "limit $1 offset $2";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = limit > 0 ? limit : "all" },
                new NpgsqlParameter { Value = numberOfPage > 0 ?  (numberOfPage - 1) * limit : 0}
            }
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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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

    public async Task<(List<Sample>? samples, string error)> GetSortByListens(int limit, int numberOfPage)
    {
        var queryString = "select * from samples order by date number_of_listens " +
                          "limit $1 offset $2";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = limit > 0 ? limit : "all" },
                new NpgsqlParameter { Value = numberOfPage > 0 ?  (numberOfPage - 1) * limit : 0}
            }
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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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

    public async Task<(List<Sample>? samples, string error)> Search(string searchString, int limit, int numberOfPage)
    {
        var queryString =
            "select * from samples where name ilike $1 or artist ilike $1 " +
            "order by date desc limit $2 offset $3";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = "%" + searchString.ToLower() + "%" },
                new NpgsqlParameter { Value = limit > 0 ? limit : "all" },
                new NpgsqlParameter { Value = numberOfPage > 0 ?  (numberOfPage - 1) * limit : 0}
            }
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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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

    public async Task<(List<Sample>? samples, string error)> GetByPlaylist(Guid playlistGuid, int limit, int numberOfPage)
    {
        var queryString = "select samples.* " +
                          "from playlist_samples " +
                          "join samples on playlist_samples.sample_guid = samples.sample_guid " +
                          "where playlist_samples.playlist_guid = $1" +
                          "order by date desc limit $2 offset $3";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = playlistGuid },
                new NpgsqlParameter { Value = limit > 0 ? limit : "all" },
                new NpgsqlParameter { Value = numberOfPage > 0 ?  (numberOfPage - 1) * limit : 0}
            }
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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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


            await reader.ReadAsync();

            var sampleEntity = new SampleEntity();

            sampleEntity.SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid"));
            sampleEntity.SampleLink = reader.GetString(reader.GetOrdinal("sample_link"));
            sampleEntity.CoverLink = reader.GetString(reader.GetOrdinal("cover_link"));
            sampleEntity.Name = reader.GetString(reader.GetOrdinal("name"));
            sampleEntity.Artist = reader.GetString(reader.GetOrdinal("artist"));
            sampleEntity.UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid"));
            sampleEntity.Duration = reader.GetDouble(reader.GetOrdinal("duration"));
            sampleEntity.Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")));
            if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
            if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
            if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
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
                sampleEntity.User.Email, null, sampleEntity.User.IsAdmin, sampleEntity.User.AvatarPath);

            if (!string.IsNullOrEmpty(sampleUserError))
                return (null, sampleUserError);

            var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, sampleUser, sampleEntity.Date);

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

    public async Task<(List<Sample>? samples, string error)> GetUserSamples(Guid userGuid, int limit, int numberOfPage)
    {
        var queryString = "select * from samples where user_guid = $1 " +
                          "order by date desc limit $2 offset $3";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = userGuid },
                new NpgsqlParameter { Value = limit > 0 ? limit : "all" },
                new NpgsqlParameter { Value = numberOfPage > 0 ?  (numberOfPage - 1) * limit : 0}
            }
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
                    NumberOfListens = reader.GetInt32(reader.GetOrdinal("number_of_listens")),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date")))
                };

                if (!reader.IsDBNull(reader.GetOrdinal("vkontakte_link")))
                    sampleEntity.VkontakteLink = reader.GetString(reader.GetOrdinal("vkontakte_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("spotify_link")))
                    sampleEntity.SpotifyLink = reader.GetString(reader.GetOrdinal("spotify_link"));
                if (!reader.IsDBNull(reader.GetOrdinal("soundcloud_link")))
                    sampleEntity.SoundcloudLink = reader.GetString(reader.GetOrdinal("soundcloud_link"));

                var (sample, error) = Sample.Create(sampleEntity.SampleGuid, sampleEntity.SampleLink,
                    sampleEntity.CoverLink, sampleEntity.Name, sampleEntity.Artist, sampleEntity.UserGuid,
                    sampleEntity.Duration, sampleEntity.VkontakteLink, sampleEntity.SpotifyLink,
                    sampleEntity.SoundcloudLink, sampleEntity.NumberOfListens, null, sampleEntity.Date);

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

    public async Task<(Guid? sampleGuid, string error)> Create(Sample sample)
    {
        var queryString = "insert into samples " +
                          "values ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11) returning sample_guid";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter() { Value = sample.SampleGuid },
                new NpgsqlParameter() { Value = sample.SampleLink },
                new NpgsqlParameter() { Value = sample.CoverLink },
                new NpgsqlParameter() { Value = sample.Name },
                new NpgsqlParameter() { Value = sample.Artist },
                new NpgsqlParameter() { Value = sample.NumberOfListens },
                new NpgsqlParameter() { Value = sample.UserGuid },
                new NpgsqlParameter() { Value = sample.Duration },
                new NpgsqlParameter() { Value = sample.VkontakteLink != null ? sample.VkontakteLink : DBNull.Value },
                new NpgsqlParameter() { Value = sample.SpotifyLink != null ? sample.SpotifyLink : DBNull.Value },
                new NpgsqlParameter() { Value = sample.SoundcloudLink != null ? sample.SoundcloudLink : DBNull.Value }
            }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            var sampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid"));

            await reader.CloseAsync();

            return (sampleGuid, string.Empty);
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

    public async Task<(bool successfully, string error)> Delete(Guid sampleGuid)
    {
        var queryString = "delete from samples where sample_guid = $1";

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