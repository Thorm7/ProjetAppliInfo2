using Npgsql;
using LiberNet.Models;
using LiberNet.Extensions;

namespace LiberNet.Services;

public class UserService
{
    private readonly NpgsqlConnection _connection;

    public UserService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public User? GetByEmail(string email)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Nom, Email, MotDePasseHash, Role FROM Users WHERE Email = @email";
        command.AddParameter("@email", email);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Email = reader.GetString(2),
                MotDePasseHash = reader.GetString(3),
                Role = reader.GetString(4)
            };
        }

        return null;
    }

    public void Add(User user)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Users (Nom, Email, MotDePasseHash, Role) 
                                VALUES (@nom, @email, @motDePasseHash, @role)";
        command.AddParameter("@nom", user.Nom);
        command.AddParameter("@email", user.Email);
        command.AddParameter("@motDePasseHash", user.MotDePasseHash);
        command.AddParameter("@role", user.Role);
        command.ExecuteNonQuery();
    }
}