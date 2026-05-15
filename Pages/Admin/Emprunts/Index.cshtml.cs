using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace LiberNet.Pages.Admin.Emprunts;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly EmpruntService _empruntService;

    public List<Emprunt> Emprunts { get; set; } = new();

    public IndexModel(EmpruntService empruntService)
    {
        _empruntService = empruntService;
    }

    public void OnGet()
    {
        Emprunts = _empruntService.GetAll();
    }

    public IActionResult OnPostRetourner(int id)
    {
        _empruntService.Retourner(id);
        TempData["Success"] = "Livre retourné avec succès !";
        return RedirectToPage();
    }
}
