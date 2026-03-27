using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;
using MinsaitPersonBack.Services;

namespace MinsaitPersonBack.Tests.Services;

public class PersonServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldPersistPerson()
    {
        await using var dbContext = CreateInMemoryContext();
        var service = new PersonService(dbContext);
        var dto = new CreatePersonDto { Name = "Ada Lovelace", Email = "ada@example.com" };

        var created = await service.CreateAsync(dto);

        created.Id.Should().NotBe(Guid.Empty);
        created.Name.Should().Be(dto.Name);
        created.Email.Should().Be(dto.Email);
        created.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        created.UpdatedAt.Should().BeCloseTo(created.CreatedAt, TimeSpan.FromMilliseconds(5));
        dbContext.Persons.Should().ContainSingle(p => p.Id == created.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPersonsOrderedByCreatedAt()
    {
        await using var dbContext = CreateInMemoryContext();
        var oldest = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Email = "old@example.com",
            CreatedAt = DateTime.UtcNow.AddMinutes(-10),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
        };
        var newest = new Person
        {
            Id = Guid.NewGuid(),
            Name = "New",
            Email = "new@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await dbContext.Persons.AddRangeAsync(newest, oldest);
        await dbContext.SaveChangesAsync();
        var service = new PersonService(dbContext);

        var result = await service.GetAllAsync();

        result.Select(p => p.Id).Should().ContainInOrder(oldest.Id, newest.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdIsInvalid()
    {
        await using var dbContext = CreateInMemoryContext();
        var service = new PersonService(dbContext);

        var result = await service.GetByIdAsync("invalid-guid");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPerson_WhenIdExists()
    {
        await using var dbContext = CreateInMemoryContext();
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        var service = new PersonService(dbContext);

        var result = await service.GetByIdAsync(person.Id.ToString());

        result.Should().NotBeNull();
        result!.Id.Should().Be(person.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        await using var dbContext = CreateInMemoryContext();
        var service = new PersonService(dbContext);

        var result = await service.UpdateAsync(Guid.NewGuid().ToString(), new UpdatePersonDto { Name = "Updated" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateName_AndKeepEmail_WhenEmailIsNull()
    {
        await using var dbContext = CreateInMemoryContext();
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Email = "old@example.com",
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            UpdatedAt = DateTime.UtcNow.AddHours(-1)
        };
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        var oldUpdatedAt = person.UpdatedAt;
        var service = new PersonService(dbContext);

        var result = await service.UpdateAsync(
            person.Id.ToString(),
            new UpdatePersonDto { Name = "New Name", Email = null });

        result.Should().NotBeNull();
        result!.Name.Should().Be("New Name");
        result.Email.Should().Be("old@example.com");
        result.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldKeepName_AndUpdateEmail_WhenNameIsNull()
    {
        await using var dbContext = CreateInMemoryContext();
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Current Name",
            Email = "old@example.com",
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            UpdatedAt = DateTime.UtcNow.AddHours(-1)
        };
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        var oldUpdatedAt = person.UpdatedAt;
        var service = new PersonService(dbContext);

        var result = await service.UpdateAsync(
            person.Id.ToString(),
            new UpdatePersonDto { Name = null, Email = "new@example.com" });

        result.Should().NotBeNull();
        result!.Name.Should().Be("Current Name");
        result.Email.Should().Be("new@example.com");
        result.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        await using var dbContext = CreateInMemoryContext();
        var service = new PersonService(dbContext);

        var result = await service.DeleteAsync(Guid.NewGuid().ToString());

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePerson_WhenPersonExists()
    {
        await using var dbContext = CreateInMemoryContext();
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "To Remove",
            Email = "remove@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        var service = new PersonService(dbContext);

        var deleted = await service.DeleteAsync(person.Id.ToString());

        deleted.Should().NotBeNull();
        deleted!.Id.Should().Be(person.Id);
        dbContext.Persons.Should().BeEmpty();
    }

    private static PersonDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PersonDbContext>()
            .UseInMemoryDatabase($"persons-tests-{Guid.NewGuid()}")
            .Options;

        return new PersonDbContext(options);
    }
}
