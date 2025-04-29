using Microsoft.EntityFrameworkCore;
using CinemaWebApi.Models;
using CinemaWebApi.Dto;

namespace CinemaWebApi.Data;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Showing> Showings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hall>().HasKey(h => h.HallNumber);

        modelBuilder.Entity<Showing>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Showings)
            .HasForeignKey(s => s.MovieId);

        modelBuilder.Entity<Showing>()
            .HasOne(s => s.Hall)
            .WithMany(h => h.Showings)
            .HasForeignKey(s => s.HallNumber);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Showing)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.ShowingId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Sale)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.SaleId)
            .IsRequired(false);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sales)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => new { t.ShowingId, t.SeatNumber })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}