using Microsoft.EntityFrameworkCore;

namespace MinsaitPersonBack.Models;

public class PersonDbContext : DbContext
{
    public PersonDbContext(DbContextOptions<PersonDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(
                    v => v.ToString(),
                    v => Guid.Parse(v))
                .HasColumnType("TEXT");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(120);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(180);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
        });
    }
}
