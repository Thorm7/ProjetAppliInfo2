using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LiberNet.Pages.Livres;

public class DetailsModel : PageModel
{
    private readonly LivreService _livreService;
    private readonly EmpruntService _empruntService;

    public Livre? Livre { get; set; }

    public DetailsModel(LivreService livreService, EmpruntService empruntService)
    {
        _livreService = livreService;
        _empruntService = empruntService;
    }

    public IActionResult OnGet(int id)
    {
        Livre = _livreService.GetById(id);

        if (Livre == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPostEmprunter(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return RedirectToPage("/Compte/Login");
        }

        var userId = int.Parse(userIdClaim.Value);
        _empruntService.Emprunter(userId, id);

        TempData["Success"] = "Livre emprunté avec succès ! Date de retour : " +
                              DateTime.Today.AddMonths(1).ToString("dd/MM/yyyy");

        return RedirectToPage("/Compte/Emprunts");
    }
}