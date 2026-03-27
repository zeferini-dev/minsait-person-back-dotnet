using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace MinsaitPersonBack.Tests.Integration;

public class ProgramConfigurationTests
{
    [Fact]
    public void ResolveSchemaId_ShouldReplaceNestedTypeSeparator()
    {
        var schemaId = Program.ResolveSchemaId(typeof(Outer.Inner));

        schemaId.Should().NotBeNull();
        schemaId.Should().Contain("Outer.Inner");
        schemaId.Should().NotContain("+");
    }

    [Fact]
    public void ResolveSchemaId_ShouldReturnNull_WhenTypeHasNoFullName()
    {
        var genericArgumentType = typeof(GenericContainer<>).GetGenericArguments()[0];

        var schemaId = Program.ResolveSchemaId(genericArgumentType);

        schemaId.Should().BeNull();
    }

    [Fact]
    public void ResolveConnectionString_ShouldUseConfiguredValue_WhenAvailable()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Data Source=configured.db"
            })
            .Build();

        var connectionString = Program.ResolveConnectionString(configuration);

        connectionString.Should().Be("Data Source=configured.db");
    }

    [Fact]
    public void ResolveConnectionString_ShouldUseDefault_WhenMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var connectionString = Program.ResolveConnectionString(configuration);

        connectionString.Should().Be("Data Source=persons.db");
    }

    private sealed class Outer
    {
        public sealed class Inner;
    }

    private sealed class GenericContainer<T>;
}
