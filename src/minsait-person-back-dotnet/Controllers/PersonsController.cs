using Microsoft.AspNetCore.Mvc;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;
using MinsaitPersonBack.Services;

namespace MinsaitPersonBack.Controllers;

/// <summary>
/// Controller for managing persons
/// </summary>
[ApiController]
[Route("persons")]
[Produces("application/json")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Create a new person
    /// </summary>
    /// <param name="dto">Person data</param>
    /// <returns>The created person</returns>
    /// <response code="201">Returns the newly created person</response>
    /// <response code="400">If the data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Create([FromBody] CreatePersonDto dto)
    {
        var person = await _personService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    /// <summary>
    /// Get all persons
    /// </summary>
    /// <returns>List of all persons</returns>
    /// <response code="200">Returns the list of persons</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Person>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Person>>> GetAll()
    {
        var persons = await _personService.GetAllAsync();
        return Ok(persons);
    }

    /// <summary>
    /// Get a person by ID
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>The person with the specified ID</returns>
    /// <response code="200">Returns the person</response>
    /// <response code="404">If the person is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> GetById(string id)
    {
        var person = await _personService.GetByIdAsync(id);
        
        if (person == null)
            return NotFound(new { message = $"Person with ID {id} not found" });

        return Ok(person);
    }

    /// <summary>
    /// Update a person
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <param name="dto">Updated person data</param>
    /// <returns>The updated person</returns>
    /// <response code="200">Returns the updated person</response>
    /// <response code="404">If the person is not found</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> Update(string id, [FromBody] UpdatePersonDto dto)
    {
        var person = await _personService.UpdateAsync(id, dto);
        
        if (person == null)
            return NotFound(new { message = $"Person with ID {id} not found" });

        return Ok(person);
    }

    /// <summary>
    /// Delete a person
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>The deleted person</returns>
    /// <response code="200">Returns the deleted person</response>
    /// <response code="404">If the person is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> Delete(string id)
    {
        var person = await _personService.DeleteAsync(id);
        
        if (person == null)
            return NotFound(new { message = $"Person with ID {id} not found" });

        return Ok(person);
    }
}
