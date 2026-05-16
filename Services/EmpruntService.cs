using Npgsql;
using LiberNet.Models;
using LiberNet.Extensions;

namespace LiberNet.Services;

public class EmpruntService
{
    private readonly NpgsqlConnection _connection;

    public EmpruntService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Emprunt> GetAll()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, UserId, LivreId, DateEmprunt, DateRetour, Statut FROM Emprunts";

        using var reader = command.ExecuteReader();
        var emprunts = new List<Emprunt>();

        while (reader.Read())
        {
            emprunts.Add(new Emprunt
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                LivreId = reader.GetInt32(2),
                DateEmprunt = reader.GetFieldValue<DateOnly>(3),
                DateRetour = reader.IsDBNull(4) ? null : reader.GetFieldValue<DateOnly>(4),
                Statut = reader.GetString(5)
            });
        }

        return emprunts;
    }

    public void Add(Emprunt emprunt)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Emprunts (UserId, LivreId, DateEmprunt, Statut) 
                                VALUES (@userId, @livreId, @dateEmprunt, @statut)";
        command.AddParameter("@userId", emprunt.UserId);
        command.AddParameter("@livreId", emprunt.LivreId);
        command.AddParameter("@dateEmprunt", emprunt.DateEmprunt);
        command.AddParameter("@statut", emprunt.Statut);
        command.ExecuteNonQuery();
    }

    public void Retourner(int id)
    {

        var command = _connection.CreateCommand();
        command.CommandText = "SELECT LivreId FROM Emprunts WHERE Id = @id";
        command.AddParameter("@id", id);
        var livreId = Convert.ToInt32(command.ExecuteScalar());

        var command2 = _connection.CreateCommand();
        command2.CommandText = @"UPDATE Emprunts 
                             SET DateRetour = @dateRetour, Statut = 'Retourne' 
                             WHERE Id = @id";
        command2.AddParameter("@dateRetour", DateOnly.FromDateTime(DateTime.Today));
        command2.AddParameter("@id", id);
        command2.ExecuteNonQuery();

        var command3 = _connection.CreateCommand();
        command3.CommandText = "UPDATE Livres SET Stock = Stock + 1 WHERE Id = @livreId";
        command3.AddParameter("@livreId", livreId);
        command3.ExecuteNonQuery();
    }
    public void Emprunter(int userId, int livreId)
    {

        var command = _connection.CreateCommand();
        command.CommandText = @"INSERT INTO Emprunts (UserId, LivreId, DateEmprunt, DateRetour, Statut) 
                            VALUES (@userId, @livreId, @dateEmprunt, @dateRetour, 'EnCours')";
        command.AddParameter("@userId", userId);
        command.AddParameter("@livreId", livreId);
        command.AddParameter("@dateEmprunt", DateOnly.FromDateTime(DateTime.Today));
        command.AddParameter("@dateRetour", DateOnly.FromDateTime(DateTime.Today.AddMonths(1)));
        command.ExecuteNonQuery();

        var command2 = _connection.CreateCommand();
        command2.CommandText = @"UPDATE Livres SET Stock = Stock - 1 
                             WHERE Id = @livreId AND Stock > 0";
        command2.AddParameter("@livreId", livreId);
        command2.ExecuteNonQuery();
    }

    public List<Emprunt> GetByUserId(int userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"SELECT Id, UserId, LivreId, DateEmprunt, DateRetour, Statut 
                            FROM Emprunts WHERE UserId = @userId";
        command.AddParameter("@userId", userId);

        using var reader = command.ExecuteReader();
        var emprunts = new List<Emprunt>();

        while (reader.Read())
        {
            emprunts.Add(new Emprunt
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                LivreId = reader.GetInt32(2),
                DateEmprunt = reader.GetFieldValue<DateOnly>(3),
                DateRetour = reader.IsDBNull(4) ? null : reader.GetFieldValue<DateOnly>(4),
                Statut = reader.GetString(5)
            });
        }

        return emprunts;
    }
    public List<EmpruntDetails> GetDetailsByUserId(int userId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"SELECT e.Id, l.Titre, e.DateEmprunt, e.DateRetour, e.Statut 
                            FROM Emprunts e
                            JOIN Livres l ON l.Id = e.LivreId
                            WHERE e.UserId = @userId
                            ORDER BY e.DateEmprunt DESC";
        command.AddParameter("@userId", userId);

        using var reader = command.ExecuteReader();
        var emprunts = new List<EmpruntDetails>();

        while (reader.Read())
        {
            emprunts.Add(new EmpruntDetails
            {
                Id = reader.GetInt32(0),
                TitreLivre = reader.GetString(1),
                DateEmprunt = reader.GetFieldValue<DateOnly>(2),
                DateRetour = reader.IsDBNull(3) ? null : reader.GetFieldValue<DateOnly>(3),
                Statut = reader.GetString(4)
            });
        }

        return emprunts;
    }
}