using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions.Repositories;
using SampleSpaceCore.Models;
using SampleSpaceDal.Entities;

namespace SampleSpaceDal.Repositories.PlaylistRepository;

public class PlaylistRepository(IConfiguration configuration) : BaseRepository(configuration), IPlaylistRepository
{
    public async Task<(Playlist?, string error)> GetByGuid(Guid playlistGuid)
    {
        var queryString = "select * " +
                          "from playlists " +
                          "where playlist_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = playlistGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return (null, "Playlist not found");

            await reader.ReadAsync();

            var playlistEntity = new PlaylistEntity()
            {
                PlaylistGuid = reader.GetGuid(reader.GetOrdinal("playlist_guid")),
                UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                CanBeModified = reader.GetBoolean(reader.GetOrdinal("can_be_modified")),
            };

            var (playlist, playlistError) = Playlist.Create(playlistEntity.PlaylistGuid, playlistEntity.UserGuid,
                playlistEntity.Name, playlistEntity.CanBeModified);

            if (!string.IsNullOrEmpty(playlistError))
                return (null, playlistError);

            await reader.CloseAsync();

            return (playlist, string.Empty);
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

    public async Task<(List<Playlist>?, string error)> Get(Guid userGuid)
    {
        var queryString = "select * " +
                          "from playlists " +
                          "where user_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = userGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var playlists = new List<Playlist>();

            while (await reader.ReadAsync())
            {
                var playlistEntity = new PlaylistEntity()
                {
                    PlaylistGuid = reader.GetGuid(reader.GetOrdinal("playlist_guid")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    CanBeModified = reader.GetBoolean(reader.GetOrdinal("can_be_modified")),
                };

                var (playlist, playlistError) = Playlist.Create(playlistEntity.PlaylistGuid, playlistEntity.UserGuid,
                    playlistEntity.Name, playlistEntity.CanBeModified);

                if (!string.IsNullOrEmpty(playlistError))
                    return (null, playlistError);

                playlists.Add(playlist!);
            }

            await reader.CloseAsync();

            return (playlists, string.Empty);
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

    public async Task<(Guid? playlistGuid, string error)> Create(Playlist playlist)
    {
        var queryString = "insert into playlists " +
                          "values ($1, $2, $3) returning playlist_guid";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter() { Value = playlist.PlaylistGuid },
                new NpgsqlParameter() { Value = playlist.UserGuid },
                new NpgsqlParameter() { Value = playlist.Name },
            }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            var playlistGuid = reader.GetGuid(reader.GetOrdinal("playlist_guid"));

            await reader.CloseAsync();

            return (playlistGuid, string.Empty);
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

    public async Task<(bool successfully, string error)> Edit(Playlist playlist)
    {
        var queryString = "update playlists set name = $2 where playlist_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = playlist.PlaylistGuid },
                new NpgsqlParameter { Value = playlist.Name }
            }
        };

        try
        {
            await connection.OpenAsync();

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return successfully ? (successfully, string.Empty) : (successfully, "Playlist not found");
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

    public async Task<(bool successfully, string error)> AddSample(PlaylistSample playlistSample)
    {
        var queryString = "insert into playlist_samples " +
                          "values ($1, $2, $3)";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = playlistSample.PlaylistSampleGuid },
                new NpgsqlParameter { Value = playlistSample.PlaylistGuid },
                new NpgsqlParameter { Value = playlistSample.SampleGuid }
            }
        };
        try
        {
            await connection.OpenAsync();

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return (successfully, string.Empty);
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

    public async Task<(bool successfully, string error)> CheckSampleContain(Guid playlistGuid, Guid sampleGuid)
    {
        var queryString = "select * " +
                          "from playlist_samples " +
                          "where playlist_guid = $1 and sample_guid = $2";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = playlistGuid },
                new NpgsqlParameter { Value = sampleGuid }
            }
        };
        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            return reader.HasRows ? (true, string.Empty) : (false, string.Empty);
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

    public async Task<(bool successfully, string error)> DeleteSample(Guid playlistGuid, Guid sampleGuid)
    {
        var queryString = "delete from playlist_samples where playlist_guid = $1 and sample_guid = $2";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter { Value = playlistGuid },
                new NpgsqlParameter { Value = sampleGuid },
            }
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

    public async Task<(bool successfully, string error)> Delete(Guid playlistGuid)
    {
        var queryString = "delete from playlists where playlist_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = playlistGuid } }
        };
        try
        {
            await connection.OpenAsync();

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return successfully ? (successfully, string.Empty) : (successfully, "Playlist not found");
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