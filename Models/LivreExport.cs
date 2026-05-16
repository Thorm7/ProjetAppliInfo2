namespace LiberNet.Models;

public class LivreExport
{
    public string Titre { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int NombreLocations { get; set; }
}