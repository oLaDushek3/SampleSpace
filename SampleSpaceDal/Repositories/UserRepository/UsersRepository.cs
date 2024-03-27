using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SampleSpaceCore.Abstractions;
using SampleSpaceCore.Models;
using SampleSpaceDal.Entities;

namespace SampleSpaceDal.Repositories.UserRepository;

public class UsersRepository(IConfiguration configuration) : BaseRepository(configuration), IUsersRepository
{
    public async Task<User?> GetByNickname(string email)
    {
        var queryString = "select * from users where nickname = $1";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters = { new NpgsqlParameter { Value = email } }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            UserEntity? userEntity = null;
            while (await reader.ReadAsync())
            {
                userEntity = new UserEntity
                {
                    UserGuid = reader.GetGuid(reader.GetOrdinal("user_guid")),
                    Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    AvatarPath = !reader.IsDBNull(reader.GetOrdinal("avatar_path"))
                        ? reader.GetString(reader.GetOrdinal("avatar_path"))
                        : null,
                };
            }

            return userEntity != null
                ? User.Create(userEntity.UserGuid, userEntity.Nickname, userEntity.Email, userEntity.PasswordHash,
                    userEntity.AvatarPath).User
                : null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Guid> Create(User user)
    {
        var queryString = "insert into users(user_guid, nickname, email, password_hash, avatar_path)" +
                          "values ($1, $2, $3, $4, $5) returning user_guid";

        var connection = GetConnection();

        var command = new NpgsqlCommand(queryString, connection)
        {
            Parameters =
            {
                new NpgsqlParameter() { Value = user.UserGuid },
                new NpgsqlParameter() { Value = user.Nickname },
                new NpgsqlParameter() { Value = user.Email },
                new NpgsqlParameter() { Value = user.Password },
                new NpgsqlParameter() { Value = user.AvatarPath != null ? user.AvatarPath : DBNull.Value },
            }
        };

        try
        {
            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            var userGuid = Guid.Empty;
            while (await reader.ReadAsync())
                userGuid = reader.GetGuid(reader.GetOrdinal("user_guid"));

            return userGuid;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}