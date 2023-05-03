using Microsoft.EntityFrameworkCore;
using ShoeApp.Models;

namespace ShoeApp.Context;

public class AppContext : DbContext
{
    public DbSet<Shoe> Shoes { get; set; }
    public AppContext(DbContextOptions<AppContext> options): base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Shoes.db");
    }
}