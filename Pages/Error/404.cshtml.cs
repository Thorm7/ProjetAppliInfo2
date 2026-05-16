using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LiberNet.Pages.Error;

[AllowAnonymous]
public class Error404Model : PageModel
{
    public void OnGet() { }
}