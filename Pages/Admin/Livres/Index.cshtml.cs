using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace LiberNet.Pages.Admin.Livres;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly LivreService _livreService;

    public List<Livre> Livres { get; set; } = new();

    [BindProperty]
    public Livre Livre { get; set; } = new();

    public IndexModel(LivreService livreService)
    {
        _livreService = livreService;
    }

    public void OnGet()
    {
        Livres = _livreService.GetAll();
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            Livres = _livreService.GetAll();
            return Page();
        }

        _livreService.Add(Livre);
        TempData["Success"] = "Livre ajouté avec succès !";
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        _livreService.Delete(id);
        TempData["Success"] = "Livre supprimé avec succès !";
        return RedirectToPage();
    }
}
