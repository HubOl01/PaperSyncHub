using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Artifact> Artifacts => Set<Artifact>();

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<BibliographyItem> Bibliography => Set<BibliographyItem>();

    public DbSet<GitCommit> GitCommits => Set<GitCommit>();

    public DbSet<ArtifactDependency> ArtifactDependencies => Set<ArtifactDependency>();

    public DbSet<ExecutionLog> ExecutionLogs => Set<ExecutionLog>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=orgarticles.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GitCommit>()
            .HasKey(x => x.CommitHash);

        modelBuilder.Entity<ArtifactDependency>()
            .HasOne(x => x.SourceArtifact)
            .WithMany()
            .HasForeignKey(x => x.SourceArtifactId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ArtifactDependency>()
            .HasOne(x => x.TargetArtifact)
            .WithMany()
            .HasForeignKey(x => x.TargetArtifactId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}