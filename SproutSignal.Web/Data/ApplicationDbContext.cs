using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SproutSignal.Web.Models;

namespace SproutSignal.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<Plant> Plants => Set<Plant>();
}