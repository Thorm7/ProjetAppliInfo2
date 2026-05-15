using System.ComponentModel.DataAnnotations;
using LiberNet.Helpers;
using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Compte;

public class RegisterModel : PageModel
{
    private readonly UserService _userService;

    public RegisterModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public string Nom { get; set; } = "";

    [BindProperty]
    [EmailAddress]
    public string Email { get; set; } = "";

    [BindProperty]
    [MinLength(6, ErrorMessage = "Le mot de passe doit faire au moins 6 caractères")]
    public string Password { get; set; } = "";

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (_userService.GetByEmail(Email) is not null)
        {
            ModelState.AddModelError("Email", "Cet email est déjà utilisé");
            return Page();
        }

        var hash = PasswordHelper.HashPassword(Password);

        _userService.Add(new Utilisateur
        {
            Nom = Nom,
            Email = Email,
            MotDePasseHash = hash,
            Role = "Membre"
        });

        return RedirectToPage("Login");
    }
}