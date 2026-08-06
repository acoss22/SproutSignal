using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SproutSignal.Web.Pages;

[AllowAnonymous]
public class GuestModel(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Plants/Index");
        }

        var guestId = Guid.NewGuid().ToString("N");
        var guest = new IdentityUser
        {
            UserName = $"guest-{guestId}@sproutsignal.local",
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(guest);
        if (!createResult.Succeeded)
        {
            AddErrors(createResult);
            return Page();
        }

        var claimResult = await userManager.AddClaimAsync(
            guest,
            new Claim("sproutsignal:guest", "true"));

        if (!claimResult.Succeeded)
        {
            await userManager.DeleteAsync(guest);
            AddErrors(claimResult);
            return Page();
        }

        await signInManager.SignInAsync(guest, isPersistent: false);

        return RedirectToPage("/Plants/Index");
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
