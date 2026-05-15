using LiberNet.Models;
using LiberNet.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace LiberNet.Pages.Admin.Users;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly UserService _userService;

    public List<Utilisateur> Users { get; set; } = new();

    public IndexModel(UserService userService)
    {
        _userService = userService;
    }

    public void OnGet()
    {
        Users = _userService.GetAll();
    }
}
