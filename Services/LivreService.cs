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
        command.CommandText = "SELECT Id, Titre, Auteur, Description, Disponible, Stock, CategorieId FROM Livres";

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
                Stock = reader.GetInt32(5),
                CategorieId = reader.GetInt32(6)
            });
        }

        return livres;
    }

    public Livre? GetById(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Titre, Auteur, Description, Disponible, Stock, CategorieId FROM Livres WHERE Id = @id";
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
                Stock = reader.GetInt32(5),
                CategorieId = reader.GetInt32(6)
            };
        }

        return null;
    }

    public void Add(Livre livre)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Livres (Titre, Auteur, Description, Disponible, Stock, CategorieId) 
                                VALUES (@titre, @auteur, @description, @disponible, @stock, @categorieId)";

        command.AddParameter("@titre", livre.Titre);
        command.AddParameter("@auteur", livre.Auteur);
        command.AddParameter("@description", livre.Description);
        command.AddParameter("@disponible", livre.Disponible);
        command.AddParameter("@stock", livre.Stock);
        command.AddParameter("@categorieId", livre.CategorieId);

        command.ExecuteNonQuery();
    }

    public void Update(Livre livre)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"UPDATE Livres 
                                SET Titre = @titre, Auteur = @auteur, Description = @description, 
                                    Disponible = @disponible, Stock = @stock, CategorieId = @categorieId 
                                WHERE Id = @id";

        command.AddParameter("@id", livre.Id);
        command.AddParameter("@titre", livre.Titre);
        command.AddParameter("@auteur", livre.Auteur);
        command.AddParameter("@description", livre.Description);
        command.AddParameter("@disponible", livre.Disponible);
        command.AddParameter("@stock", livre.Stock);
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

    public List<Livre> Search(string? search, int page, int pageSize)
    {
        var sql = "SELECT Id, Titre, Auteur, Description, Disponible, Stock, CategorieId FROM Livres WHERE 1=1";
        var parameters = new List<NpgsqlParameter>();

        if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (Titre ILIKE @search OR Auteur ILIKE @search)";
            parameters.Add(new NpgsqlParameter("@search", $"%{search}%"));
        }

        sql += " ORDER BY Id LIMIT @pageSize OFFSET @offset";
        parameters.Add(new NpgsqlParameter("@pageSize", pageSize));
        parameters.Add(new NpgsqlParameter("@offset", (page - 1) * pageSize));

        var command = _connection.CreateCommand();
        command.CommandText = sql;
        foreach (var p in parameters)
            command.Parameters.Add(p);

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
                Stock = reader.GetInt32(5),
                CategorieId = reader.GetInt32(6)
            });
        }

        return livres;
    }

    public int Count(string? search)
    {
        var sql = "SELECT COUNT(*) FROM Livres WHERE 1=1";
        var parameters = new List<NpgsqlParameter>();

        if (!string.IsNullOrEmpty(search))
        {
            sql += " AND (Titre ILIKE @search OR Auteur ILIKE @search)";
            parameters.Add(new NpgsqlParameter("@search", $"%{search}%"));
        }

        var command = _connection.CreateCommand();
        command.CommandText = sql;
        foreach (var p in parameters)
            command.Parameters.Add(p);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public List<LivreExport> GetExportData()
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT 
                l.Titre, 
                l.Auteur, 
                c.Nom AS Categorie,
                CASE WHEN l.Stock > 0 THEN 'Disponible' ELSE 'Location en cours' END AS Statut,                l.Stock,
                COUNT(e.Id) AS NombreLocations
            FROM Livres l
            LEFT JOIN Categories c ON c.Id = l.CategorieId
            LEFT JOIN Emprunts e ON e.LivreId = l.Id
            GROUP BY l.Id, l.Titre, l.Auteur, c.Nom, l.Disponible, l.Stock
            ORDER BY l.Id";

        using var reader = command.ExecuteReader();
        var exports = new List<LivreExport>();

        while (reader.Read())
        {
            exports.Add(new LivreExport
            {
                Titre = reader.GetString(0),
                Auteur = reader.GetString(1),
                Categorie = reader.GetString(2),
                Statut = reader.GetString(3),
                Stock = reader.GetInt32(4),
                NombreLocations = reader.GetInt32(5)
            });
        }

        return exports;
    }
}