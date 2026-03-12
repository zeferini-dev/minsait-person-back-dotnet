using Xunit;
using MinsaitPersonBack.DTOs;

public class CreatePersonDtoValidatorTests
{
    private readonly CreatePersonDtoValidator _validator = new();

    [Fact]
    public void ValidDto_PassesValidation()
    {
        var dto = new CreatePersonDto { Name = "Ada", Email = "ada@example.com" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Name_Required(string? name)
    {
        var dto = new CreatePersonDto { Name = name ?? string.Empty, Email = "ada@example.com" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var dto = new CreatePersonDto { Name = new string('a', 121), Email = "ada@example.com" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Email_Required()
    {
        var dto = new CreatePersonDto { Name = "Ada", Email = "" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_InvalidFormat()
    {
        var dto = new CreatePersonDto { Name = "Ada", Email = "not-an-email" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_TooLong_Fails()
    {
        var dto = new CreatePersonDto { Name = "Ada", Email = new string('a', 181) + "@e.com" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
}
