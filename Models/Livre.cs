using System.ComponentModel.DataAnnotations;

namespace LiberNet.Models;

public class Livre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Le titre doit faire entre 2 et 200 caractères")]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'auteur est obligatoire")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "L'auteur doit faire entre 2 et 150 caractères")]
    public string Auteur { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Disponible { get; set; } = true;

    [Required(ErrorMessage = "Le stock est obligatoire")]
    [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif")]
    public int Stock { get; set; } = 1;

    [Required(ErrorMessage = "La catégorie est obligatoire")]
    [Range(1, int.MaxValue, ErrorMessage = "Veuillez choisir une catégorie")]
    public int CategorieId { get; set; }
}