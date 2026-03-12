using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MinsaitPersonBack.Controllers;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;
using MinsaitPersonBack.Services;

namespace MinsaitPersonBack.Tests.Controllers;

public class PersonsControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly PersonsController _controller;

    public PersonsControllerTests()
    {
        _personServiceMock = new Mock<IPersonService>();
        _controller = new PersonsController(_personServiceMock.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenPersonCreated()
    {
        var dto = new CreatePersonDto { Name = "Test", Email = "test@example.com" };
        var person = new Person { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email, CreatedAt = DateTime.UtcNow };
        _personServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(person);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(person, createdResult.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithPersonsList()
    {
        var persons = new List<Person> { new Person { Id = Guid.NewGuid(), Name = "Test", Email = "test@example.com", CreatedAt = DateTime.UtcNow } };
        _personServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(persons);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(persons, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenPersonExists()
    {
        var id = Guid.NewGuid().ToString();
        var person = new Person { Id = Guid.Parse(id), Name = "Test", Email = "test@example.com", CreatedAt = DateTime.UtcNow };
        _personServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(person);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(person, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        var id = Guid.NewGuid().ToString();
        _personServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Person?)null);

        var result = await _controller.GetById(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains(id, notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenPersonUpdated()
    {
        var id = Guid.NewGuid().ToString();
        var dto = new UpdatePersonDto { Name = "Updated", Email = "updated@example.com" };
        var person = new Person { Id = Guid.Parse(id), Name = dto.Name!, Email = dto.Email!, CreatedAt = DateTime.UtcNow };
        _personServiceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(person);

        var result = await _controller.Update(id, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(person, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        var id = Guid.NewGuid().ToString();
        var dto = new UpdatePersonDto { Name = "Updated", Email = "updated@example.com" };
        _personServiceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync((Person?)null);

        var result = await _controller.Update(id, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains(id, notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenPersonDeleted()
    {
        var id = Guid.NewGuid().ToString();
        var person = new Person { Id = Guid.Parse(id), Name = "Test", Email = "test@example.com", CreatedAt = DateTime.UtcNow };
        _personServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(person);

        var result = await _controller.Delete(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(person, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        var id = Guid.NewGuid().ToString();
        _personServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync((Person?)null);

        var result = await _controller.Delete(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains(id, notFoundResult.Value.ToString());
    }
}