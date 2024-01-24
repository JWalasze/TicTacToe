using Lib.Enums;
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
        modelBuilder.Entity<Player>().Property(p => p.WonGames).HasDefaultValue(0);
        modelBuilder.Entity<Player>().Property(p => p.DrawGames).HasDefaultValue(0);
        modelBuilder.Entity<Player>().Property(p => p.LostGames).HasDefaultValue(0);
        modelBuilder.Entity<Game>().HasKey(g => g.Id);
        modelBuilder.Entity<Game>().Property(g => g.GameStatus).HasDefaultValue(GameStatus.BeingPrepared);
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
        modelBuilder.Entity<Game>().Property(g => g.GameStatus).HasConversion(gs => gs.ToString(),
            gs => (GameStatus)Enum.Parse(typeof(GameStatus), gs));
        modelBuilder.Entity<Game>().Property(g => g.Player1Piece)
            .HasConversion(p1 => p1.ToString(), p1 => (Piece)Enum.Parse(typeof(Piece), p1));
        modelBuilder.Entity<Game>().Property(g => g.Player2Piece)
            .HasConversion(p2 => p2.ToString(), p2 => (Piece)Enum.Parse(typeof(Piece), p2));
    }

    public DbSet<Player> Players { get; set; }

    public DbSet<Game> Games { get; set; }
}