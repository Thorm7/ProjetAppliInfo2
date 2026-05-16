using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LiberNet.Pages.Compte;

[Authorize]
public class EmpruntsModel : PageModel
{
    private readonly EmpruntService _empruntService;

    public List<EmpruntDetails> Emprunts { get; set; } = new();

    public EmpruntsModel(EmpruntService empruntService)
    {
        _empruntService = empruntService;
    }

    public void OnGet()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return;

        var userId = int.Parse(userIdClaim.Value);
        Emprunts = _empruntService.GetDetailsByUserId(userId);
    }
}