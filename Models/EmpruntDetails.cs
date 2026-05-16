namespace LiberNet.Models;

public class EmpruntDetails
{
    public int Id { get; set; }
    public string TitreLivre { get; set; } = string.Empty;
    public DateOnly DateEmprunt { get; set; }
    public DateOnly? DateRetour { get; set; }
    public string Statut { get; set; } = string.Empty;
}