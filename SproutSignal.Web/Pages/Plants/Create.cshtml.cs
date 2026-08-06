using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SproutSignal.Web.Data;
using SproutSignal.Web.Models;

namespace SproutSignal.Web.Pages.Plants;

[Authorize]
public class CreateModel(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Plant name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Species { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [Range(1, 365)]
        [Display(Name = "Water every")]
        public int WateringIntervalDays { get; set; } = 7;

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = userManager.GetUserId(User);

        if (userId is null)
        {
            return Challenge();
        }

        var plant = new Plant
        {
            Name = Input.Name.Trim(),
            Species = Normalize(Input.Species),
            Location = Normalize(Input.Location),
            WateringIntervalDays = Input.WateringIntervalDays,
            Notes = Normalize(Input.Notes),
            OwnerId = userId,
            CreatedAtUtc = DateTime.UtcNow
        };

        context.Plants.Add(plant);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}