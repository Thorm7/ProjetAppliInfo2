using System.ComponentModel.DataAnnotations;

namespace LiberNet.Models;

public class Categorie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit faire entre 2 et 100 caractères")]
    public string Nom { get; set; } = string.Empty;
}