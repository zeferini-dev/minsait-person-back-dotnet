using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MinsaitPersonBack.Models;

namespace MinsaitPersonBack.Tests.Models;

public class PersonDbContextTests
{
    [Fact]
    public void Model_ShouldConfigurePersonEntity()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PersonDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new PersonDbContext(options);
        context.Database.EnsureCreated();

        var entity = context.Model.FindEntityType(typeof(Person));
        entity.Should().NotBeNull();

        entity!.FindPrimaryKey()!.Properties.Select(p => p.Name).Should().Equal(nameof(Person.Id));

        var id = entity.FindProperty(nameof(Person.Id))!;
        id.GetColumnType().Should().Be("TEXT");
        id.GetValueConverter().Should().NotBeNull();

        var name = entity.FindProperty(nameof(Person.Name))!;
        name.IsNullable.Should().BeFalse();
        name.GetMaxLength().Should().Be(120);

        var email = entity.FindProperty(nameof(Person.Email))!;
        email.IsNullable.Should().BeFalse();
        email.GetMaxLength().Should().Be(180);

        entity.FindProperty(nameof(Person.CreatedAt))!.IsNullable.Should().BeFalse();
        entity.FindProperty(nameof(Person.UpdatedAt))!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldPersistAndReadPerson_WithGuidKey()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PersonDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new PersonDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Persons.AddAsync(person);
        await context.SaveChangesAsync();

        var persisted = await context.Persons.FindAsync(person.Id);
        persisted.Should().NotBeNull();
        persisted!.Id.Should().Be(person.Id);
    }
}
