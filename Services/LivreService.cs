using Npgsql;
using LiberNet.Models;
using LiberNet.Extensions;

namespace LiberNet.Services;

public class LivreService
{
    private readonly NpgsqlConnection _connection;

    public LivreService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Livre> GetAll()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Titre, Auteur, Description, Disponible, CategorieId FROM Livres";

        using var reader = command.ExecuteReader();
        var livres = new List<Livre>();

        while (reader.Read())
        {
            livres.Add(new Livre
            {
                Id = reader.GetInt32(0),
                Titre = reader.GetString(1),
                Auteur = reader.GetString(2),
                Description = reader.GetString(3),
                Disponible = reader.GetBoolean(4),
                CategorieId = reader.GetInt32(5)
            });
        }

        return livres;
    }

    public Livre? GetById(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Titre, Auteur, Description, Disponible, CategorieId FROM Livres WHERE Id = @id";
        command.AddParameter("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Livre
            {
                Id = reader.GetInt32(0),
                Titre = reader.GetString(1),
                Auteur = reader.GetString(2),
                Description = reader.GetString(3),
                Disponible = reader.GetBoolean(4),
                CategorieId = reader.GetInt32(5)
            };
        }

        return null;
    }
    public void Add(Livre livre)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Livres (Titre, Auteur, Description, Disponible, CategorieId) 
                            VALUES (@titre, @auteur, @description, @disponible, @categorieId)";

        command.AddParameter("@titre", livre.Titre);
        command.AddParameter("@auteur", livre.Auteur);
        command.AddParameter("@description", livre.Description);
        command.AddParameter("@disponible", livre.Disponible);
        command.AddParameter("@categorieId", livre.CategorieId);

        command.ExecuteNonQuery();
    }
    public void Update(Livre livre)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"UPDATE Livres 
                            SET Titre = @titre, Auteur = @auteur, Description = @description, 
                                Disponible = @disponible, CategorieId = @categorieId 
                            WHERE Id = @id";

        command.AddParameter("@id", livre.Id);
        command.AddParameter("@titre", livre.Titre);
        command.AddParameter("@auteur", livre.Auteur);
        command.AddParameter("@description", livre.Description);
        command.AddParameter("@disponible", livre.Disponible);
        command.AddParameter("@categorieId", livre.CategorieId);

        command.ExecuteNonQuery();
    }
    public void Delete(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Livres WHERE Id = @id";

        command.AddParameter("@id", id);

        command.ExecuteNonQuery();
    }
}