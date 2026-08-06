using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SproutSignal.Web.Pages;

[AllowAnonymous]
public class AccessModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; } = "/Plants";

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(GetSafeReturnUrl());
        }

        ReturnUrl = GetSafeReturnUrl();
        return Page();
    }

    private string GetSafeReturnUrl()
    {
        return Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/Plants";
    }
}
