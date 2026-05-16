using LiberNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace LiberNet.Pages.Admin.Livres;

[Authorize(Roles = "Admin")]
public class ExportModel : PageModel
{
    private readonly LivreService _livreService;

    public ExportModel(LivreService livreService)
    {
        _livreService = livreService;
    }

    public IActionResult OnGet()
    {
        var livres = _livreService.GetExportData();

        var sb = new StringBuilder();
        sb.AppendLine("Titre;Auteur;Categorie;Statut;Stock;NombreLocations");

        foreach (var livre in livres)
        {
            sb.AppendLine($"{livre.Titre};{livre.Auteur};{livre.Categorie};{livre.Statut};{livre.Stock};{livre.NombreLocations}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "livres.csv");
    }
}