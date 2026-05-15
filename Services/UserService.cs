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

    public Utilisateur? GetByEmail(string email)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Nom, Email, MotDePasseHash, Role FROM Users WHERE Email = @email";
        command.AddParameter("@email", email);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Utilisateur
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

    public void Add(Utilisateur utilisateur)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Users (Nom, Email, MotDePasseHash, Role) 
                                VALUES (@nom, @email, @motDePasseHash, @role)";
        command.AddParameter("@nom", utilisateur.Nom);
        command.AddParameter("@email", utilisateur.Email);
        command.AddParameter("@motDePasseHash", utilisateur.MotDePasseHash);
        command.AddParameter("@role", utilisateur.Role);
        command.ExecuteNonQuery();
    }
    public List<Utilisateur> GetAll()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Nom, Email, Role FROM Users";

        using var reader = command.ExecuteReader();
        var users = new List<Utilisateur>();

        while (reader.Read())
        {
            users.Add(new Utilisateur
            {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Email = reader.GetString(2),
                Role = reader.GetString(3)
            });
        }

        return users;
    }
}