using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Livres;

public class IndexModel : PageModel
{
    private readonly LivreService _livreService;

    public List<Livre> Livres { get; set; } = new();

    public IndexModel(LivreService livreService)
    {
        _livreService = livreService;
    }

    public void OnGet()
    {
        Livres = _livreService.GetAll();
    }
}
