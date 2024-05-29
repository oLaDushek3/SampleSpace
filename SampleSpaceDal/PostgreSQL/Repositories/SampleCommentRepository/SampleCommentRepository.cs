using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions.PostgreSQL.Repositories;
using SampleSpaceCore.Models;
using SampleSpaceDal.PostgreSQL.Entities;

namespace SampleSpaceDal.PostgreSQL.Repositories.SampleCommentRepository;

public class SampleCommentRepository(IConfiguration configuration) : BaseRepository(configuration),
    ISampleCommentRepository
{
    public async Task<(SampleComment?, string error)> GetByGuid(Guid commentGuid)
    {
        var queryString = "select * " +
                          "from sample_comments " +
                          "          left join users on sample_comments.user_guid = users.user_guid " +
                          "where sample_comment_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = commentGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return (null, "Comment not found");

            await reader.ReadAsync();

            var sampleCommentEntity = new SampleCommentEntity
            {
                SampleCommentGuid = reader.GetGuid(reader.GetOrdinal("sample_comment_guid")),
                SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid")),
                UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                Comment = reader.GetString(reader.GetOrdinal("comment")),

                User = new UserEntity
                {
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    AvatarPath = !reader.IsDBNull(reader.GetOrdinal("avatar_path"))
                        ? reader.GetString(reader.GetOrdinal("avatar_path"))
                        : null
                }
            };

            var (commentUser, sampleUserError) = User.Create(sampleCommentEntity.User.UserGuid,
                sampleCommentEntity.User.Nickname,
                sampleCommentEntity.User.Email, null, sampleCommentEntity.User.AvatarPath);

            if (!string.IsNullOrEmpty(sampleUserError))
                return (null, sampleUserError);

            var (sampleComment, sampleCommentError) = SampleComment.Create(sampleCommentEntity.SampleCommentGuid,
                sampleCommentEntity.SampleGuid, sampleCommentEntity.UserGuid, sampleCommentEntity.Date,
                sampleCommentEntity.Comment, commentUser);

            if (!string.IsNullOrEmpty(sampleCommentError))
                return (null, sampleCommentError);

            await reader.CloseAsync();

            return (sampleComment, string.Empty);
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

    public async Task<(List<SampleComment>?, string error)> Get(Guid sampleGuid)
    {
        var queryString = "select * " +
                          "from sample_comments " +
                          "          left join users on sample_comments.user_guid = users.user_guid " +
                          "where sample_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = sampleGuid } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var sampleComments = new List<SampleComment>();

            while (await reader.ReadAsync())
            {
                var sampleCommentEntity = new SampleCommentEntity
                {
                    SampleCommentGuid = reader.GetGuid(reader.GetOrdinal("sample_comment_guid")),
                    SampleGuid = reader.GetGuid(reader.GetOrdinal("sample_guid")),
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    Comment = reader.GetString(reader.GetOrdinal("comment")),

                    User = new UserEntity
                    {
                        UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                        Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        AvatarPath = !reader.IsDBNull(reader.GetOrdinal("avatar_path"))
                            ? reader.GetString(reader.GetOrdinal("avatar_path"))
                            : null
                    }
                };

                var (commentUser, sampleUserError) = User.Create(sampleCommentEntity.User.UserGuid,
                    sampleCommentEntity.User.Nickname,
                    sampleCommentEntity.User.Email, null, sampleCommentEntity.User.AvatarPath);

                if (!string.IsNullOrEmpty(sampleUserError))
                    return (null, sampleUserError);

                var (sampleComment, sampleCommentError) = SampleComment.Create(sampleCommentEntity.SampleCommentGuid,
                    sampleCommentEntity.SampleGuid, sampleCommentEntity.UserGuid, sampleCommentEntity.Date,
                    sampleCommentEntity.Comment, commentUser);

                if (!string.IsNullOrEmpty(sampleCommentError))
                    return (null, sampleCommentError);

                sampleComments.Add(sampleComment!);
            }

            await reader.CloseAsync();

            return (sampleComments, string.Empty);
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

    public async Task<(Guid? commentGuid, string error)> Create(SampleComment comment)
    {
        var queryString = "insert into sample_comments(sample_comment_guid, sample_guid, user_guid, comment)" +
                          "values ($1, $2, $3, $4) returning sample_comment_guid";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter() { Value = comment.SampleCommentGuid },
                new NpgsqlParameter() { Value = comment.SampleGuid },
                new NpgsqlParameter() { Value = comment.UserGuid },
                new NpgsqlParameter() { Value = comment.Comment },
            }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            var commentGuid = reader.GetGuid(reader.GetOrdinal("sample_comment_guid"));

            await reader.CloseAsync();

            return (commentGuid, string.Empty);
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
    
    public async Task<(bool successfully, string error)> Edit(SampleComment comment)
    {
        var queryString = "update sample_comments set comment = $2, date = $3 where sample_comment_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        { 
            Parameters =
            {
                new NpgsqlParameter { Value = comment.SampleCommentGuid },
                new NpgsqlParameter { Value = comment.Comment },
                new NpgsqlParameter { Value = comment.Date }
            }
        };
        
        try
        {
            await connection.OpenAsync();

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return successfully ? (successfully, string.Empty) : (successfully, "Comment not found");
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

    public async Task<(bool successfully, string error)> Delete(Guid commentGuid)
    {
        var queryString = "delete from sample_comments where sample_comment_guid = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = commentGuid } }
        };
        try
        {
            await connection.OpenAsync();

            var successfully = await command.ExecuteNonQueryAsync() > 0;

            return successfully ? (successfully, string.Empty) : (successfully, "Comment not found");
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