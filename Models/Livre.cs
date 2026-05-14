namespace LiberNet.Models;

public class Livre
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Auteur { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Disponible { get; set; } = true;
    public int CategorieId { get; set; }
}