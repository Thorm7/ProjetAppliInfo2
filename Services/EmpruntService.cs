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
        command.CommandText = @"UPDATE Emprunts 
                                SET DateRetour = @dateRetour, Statut = 'Retourne' 
                                WHERE Id = @id";
        command.AddParameter("@dateRetour", DateOnly.FromDateTime(DateTime.Today));
        command.AddParameter("@id", id);
        command.ExecuteNonQuery();
    }
}