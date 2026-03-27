using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace MinsaitPersonBack.Tests.Integration;

public class ProgramIntegrationTests : IClassFixture<ProgramIntegrationTests.PersonApiFactory>
{
    private readonly PersonApiFactory _factory;

    public ProgramIntegrationTests(PersonApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Root_ShouldServeSwaggerUi()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.Should().BeTrue();
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
        body.Should().Contain("Swagger UI");
    }

    [Fact]
    public async Task SwaggerJson_ShouldContainApiTitle()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/swagger/v1/swagger.json");
        var body = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.Should().BeTrue();
        body.Should().Contain("Zeferini Person API");
    }

    [Fact]
    public async Task PersonsEndpoint_ShouldReturnJsonArray()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/persons");
        var body = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.Should().BeTrue();
        using var document = JsonDocument.Parse(body);
        document.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    public sealed class PersonApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"persons-tests-{Guid.NewGuid():N}.db");

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = $"Data Source={_dbPath}"
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            try
            {
                if (File.Exists(_dbPath))
                    File.Delete(_dbPath);
            }
            catch (IOException)
            {
            }
        }
    }
}
