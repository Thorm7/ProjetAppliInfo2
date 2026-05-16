using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LiberNet.Pages.Admin.Livres;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly LivreService _livreService;
    private readonly CategorieService _categorieService;

    public List<Livre> Livres { get; set; } = new();
    public List<SelectListItem> CategorieOptions { get; set; } = new();

    [BindProperty]
    public Livre Livre { get; set; } = new();

    public IndexModel(LivreService livreService, CategorieService categorieService)
    {
        _livreService = livreService;
        _categorieService = categorieService;
    }

    private void LoadOptions()
    {
        Livres = _livreService.GetAll();
        CategorieOptions = _categorieService.GetAll()
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nom
            }).ToList();
    }

    public void OnGet()
    {
        LoadOptions();
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            LoadOptions();
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