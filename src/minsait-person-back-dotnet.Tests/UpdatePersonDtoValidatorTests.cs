using Xunit;
using MinsaitPersonBack.DTOs;

public class UpdatePersonDtoValidatorTests
{
    private readonly UpdatePersonDtoValidator _validator = new();

    [Fact]
    public void ValidDto_PassesValidation()
    {
        var dto = new UpdatePersonDto { Name = "Ada", Email = "ada@example.com" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var dto = new UpdatePersonDto { Name = new string('a', 121), Email = "ada@example.com" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Email_InvalidFormat_Fails()
    {
        var dto = new UpdatePersonDto { Name = "Ada", Email = "not-an-email" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_TooLong_Fails()
    {
        var dto = new UpdatePersonDto { Name = "Ada", Email = new string('a', 181) + "@e.com" };
        var result = _validator.Validate(dto);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void NullFields_PassValidation()
    {
        var dto = new UpdatePersonDto { Name = null, Email = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
