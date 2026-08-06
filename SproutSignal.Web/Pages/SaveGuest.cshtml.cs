using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SproutSignal.Web.Data;

namespace SproutSignal.Web.Pages;

[Authorize]
public class SaveGuestModel(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : PageModel
{
    private const string GuestClaimType = "sproutsignal:guest";

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Saved { get; private set; }
    public string? SavedEmail { get; private set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        return IsGuest() ? Page() : RedirectToPage("/Plants/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsGuest())
        {
            return RedirectToPage("/Plants/Index");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var guest = await userManager.GetUserAsync(User);
        if (guest is null)
        {
            return Challenge();
        }

        var email = Input.Email.Trim();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var result = await userManager.SetEmailAsync(guest, email);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await transaction.RollbackAsync();
            return Page();
        }

        result = await userManager.SetUserNameAsync(guest, email);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await transaction.RollbackAsync();
            return Page();
        }

        result = await userManager.AddPasswordAsync(guest, Input.Password);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await transaction.RollbackAsync();
            return Page();
        }

        guest.EmailConfirmed = true;
        result = await userManager.UpdateAsync(guest);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await transaction.RollbackAsync();
            return Page();
        }

        result = await userManager.RemoveClaimAsync(
            guest,
            new Claim(GuestClaimType, "true"));
        if (!result.Succeeded)
        {
            AddErrors(result);
            await transaction.RollbackAsync();
            return Page();
        }

        await transaction.CommitAsync();
        await signInManager.RefreshSignInAsync(guest);

        Saved = true;
        SavedEmail = email;
        Input = new InputModel();

        return Page();
    }

    private bool IsGuest()
    {
        return User.HasClaim(GuestClaimType, "true");
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
