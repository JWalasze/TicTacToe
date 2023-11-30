using Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasIndex(p => p.Username).IsUnique();
        modelBuilder.Entity<Player>().Property(p => p.WonGames).HasDefaultValue(0);
        modelBuilder.Entity<Player>().Property(p => p.DrawGames).HasDefaultValue(0);
        modelBuilder.Entity<Player>().Property(p => p.LostGames).HasDefaultValue(0);
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Player1)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Player2)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Winner)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Player> Players { get; set; }

    public DbSet<Game> Games { get; set; }
}