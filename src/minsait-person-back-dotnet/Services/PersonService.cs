using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;
using Microsoft.EntityFrameworkCore;

namespace MinsaitPersonBack.Services;

public interface IPersonService
{
    Task<Person> CreateAsync(CreatePersonDto dto);
    Task<List<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(string id);
    Task<Person?> UpdateAsync(string id, UpdatePersonDto dto);
    Task<Person?> DeleteAsync(string id);
}

public class PersonService : IPersonService
{
    private readonly PersonDbContext _dbContext;

    public PersonService(PersonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Person> CreateAsync(CreatePersonDto dto)
    {
        var personId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var person = new Person
        {
            Id = personId,
            Name = dto.Name,
            Email = dto.Email,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _dbContext.Persons.AddAsync(person);
        await _dbContext.SaveChangesAsync();

        return person;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _dbContext.Persons
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return null;
        return await _dbContext.Persons.FindAsync(guid);
    }

    public async Task<Person?> UpdateAsync(string id, UpdatePersonDto dto)
    {
        var existingPerson = await GetByIdAsync(id);
        if (existingPerson == null)
            return null;

        existingPerson.Name = dto.Name ?? existingPerson.Name;
        existingPerson.Email = dto.Email ?? existingPerson.Email;
        existingPerson.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return existingPerson;
    }

    public async Task<Person?> DeleteAsync(string id)
    {
        var person = await GetByIdAsync(id);
        if (person == null)
            return null;

        _dbContext.Persons.Remove(person);
        await _dbContext.SaveChangesAsync();

        return person;
    }
}
