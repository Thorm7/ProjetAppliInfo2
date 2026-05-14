using Npgsql;
using LiberNet.Models;
using LiberNet.Extensions;

namespace LiberNet.Services;

public class CategorieService
{
    private readonly NpgsqlConnection _connection;

    public CategorieService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Categorie> GetAll()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Nom FROM Categories";

        using var reader = command.ExecuteReader();
        var categories = new List<Categorie>();

        while (reader.Read())
        {
            categories.Add(new Categorie
            {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1)
            });
        }

        return categories;
    }

    public void Add(Categorie categorie)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO Categories (Nom) VALUES (@nom)";
        command.AddParameter("@nom", categorie.Nom);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Categories WHERE Id = @id";
        command.AddParameter("@id", id);
        command.ExecuteNonQuery();
    }
}