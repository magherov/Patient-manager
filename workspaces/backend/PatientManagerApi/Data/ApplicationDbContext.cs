using Microsoft.EntityFrameworkCore;
using PatientManagerApi.Models;

namespace PatientManagerApi.Data;

public class ApplicationDbContext : DbContext
{
    // Costruttore che accetta le opzioni di configurazione
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Definizione delle tabelle
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Parameter> Parameters { get; set; } = null!;

    // Configurazione del modello
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazione della tabella Users
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();  // Username deve essere univoco

        // Configurazione della relazione Patient-Parameter
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Parameters)
            .WithOne()
            .HasForeignKey(p => p.PatientID)
            .OnDelete(DeleteBehavior.Cascade);  // Eliminazione a cascata
    }
} 