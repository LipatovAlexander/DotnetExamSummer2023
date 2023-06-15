using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
}