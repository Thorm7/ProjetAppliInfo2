using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Admin.Livres;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly LivreService _livreService;

    [BindProperty]
    public Livre Livre { get; set; } = new();

    public EditModel(LivreService livreService)
    {
        _livreService = livreService;
    }

    public IActionResult OnGet(int id)
    {
        var livre = _livreService.GetById(id);

        if (livre == null) return NotFound();

        Livre = livre;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _livreService.Update(Livre);
        TempData["Success"] = "Livre modifié avec succès !";
        return RedirectToPage("Index");
    }
}