using System.ComponentModel.DataAnnotations;

namespace LiberNet.Models;

public class Utilisateur
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit faire entre 2 et 100 caractères")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public string Email { get; set; } = string.Empty;

    public string MotDePasseHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Membre";
}