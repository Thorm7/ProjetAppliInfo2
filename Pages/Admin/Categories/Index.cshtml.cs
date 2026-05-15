using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace LiberNet.Pages.Admin.Categories;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly CategorieService _categorieService;

    public List<Categorie> Categories { get; set; } = new();

    [BindProperty]
    public Categorie Categorie { get; set; } = new();

    public IndexModel(CategorieService categorieService)
    {
        _categorieService = categorieService;
    }

    public void OnGet()
    {
        Categories = _categorieService.GetAll();
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            Categories = _categorieService.GetAll();
            return Page();
        }

        _categorieService.Add(Categorie);
        TempData["Success"] = "Catégorie ajoutée avec succès !";
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        _categorieService.Delete(id);
        TempData["Success"] = "Catégorie supprimée avec succès !";
        return RedirectToPage();
    }
}
