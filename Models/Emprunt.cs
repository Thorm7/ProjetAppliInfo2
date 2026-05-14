namespace LiberNet.Models;

public class Emprunt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LivreId { get; set; }
    public DateOnly DateEmprunt { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? DateRetour { get; set; }
    public string Statut { get; set; } = "EnCours";
}