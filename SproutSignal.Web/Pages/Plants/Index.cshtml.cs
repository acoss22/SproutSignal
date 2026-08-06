using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SproutSignal.Web.Data;
using SproutSignal.Web.Models;

namespace SproutSignal.Web.Pages.Plants;

[Authorize]
public class IndexModel(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager) : PageModel
{
    public IReadOnlyList<Plant> Plants { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var userId = userManager.GetUserId(User);

        if (userId is null)
        {
            return;
        }

        Plants = await context.Plants
            .Where(plant => plant.OwnerId == userId)
            .OrderBy(plant => plant.Name)
            .AsNoTracking()
            .ToListAsync();
    }
}