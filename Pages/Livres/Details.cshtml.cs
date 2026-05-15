using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Livres;

public class DetailsModel : PageModel
{
    private readonly LivreService _livreService;

    public Livre? Livre { get; set; }

    public DetailsModel(LivreService livreService)
    {
        _livreService = livreService;
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
}