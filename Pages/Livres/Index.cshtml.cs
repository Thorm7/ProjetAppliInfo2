using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Livres;

public class IndexModel : PageModel
{
    private readonly LivreService _livreService;

    public List<Livre> Livres { get; set; } = new();
    public string? Search { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    private const int PageSize = 5;

    public IndexModel(LivreService livreService)
    {
        _livreService = livreService;
    }

    public void OnGet(string? search, int pageNumber = 1)
    {
        Search = search;
        CurrentPage = pageNumber;

        var total = _livreService.Count(search);
        TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        Livres = _livreService.Search(search, pageNumber, PageSize);
    }
}