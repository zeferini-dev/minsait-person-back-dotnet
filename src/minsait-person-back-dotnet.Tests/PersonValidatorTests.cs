using Xunit;
using MinsaitPersonBack.Models;

public class PersonValidatorTests
{
    private readonly PersonValidator _validator = new();

    [Fact]
    public void ValidPerson_PassesValidation()
    {
        var person = new Person { Name = "Ada", Email = "ada@example.com" };
        var result = _validator.Validate(person);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Name_Required()
    {
        var person = new Person { Name = "", Email = "ada@example.com" };
        var result = _validator.Validate(person);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var person = new Person { Name = new string('a', 121), Email = "ada@example.com" };
        var result = _validator.Validate(person);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Email_Required()
    {
        var person = new Person { Name = "Ada", Email = "" };
        var result = _validator.Validate(person);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_InvalidFormat()
    {
        var person = new Person { Name = "Ada", Email = "not-an-email" };
        var result = _validator.Validate(person);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_TooLong_Fails()
    {
        var person = new Person { Name = "Ada", Email = new string('a', 181) + "@e.com" };
        var result = _validator.Validate(person);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
}
