using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApi.UnitTests.Controllers;

[TestFixture]
public class HealthControllerTests
{
    private HealthController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _controller = new HealthController();
    }

    [Test]
    public void Get_WhenCalled_ShouldReturnOkWithStatus()
    {
        // Act
        var result = _controller.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().NotBeNull();
        
        var value = okResult.Value;
        value.Should().NotBeNull();
        
        var properties = value!.GetType().GetProperties();
        properties.Should().Contain(p => p.Name == "status");
        properties.Should().Contain(p => p.Name == "timestamp");
    }
}
