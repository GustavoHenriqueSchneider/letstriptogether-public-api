using Application.UseCases.v1.Destination.Query.GetDestinationById;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Destination.Query.GetDestinationById;

[TestFixture]
public class DestinationAttractionModelTests
{
    [Test]
    public void Create_WhenAllPropertiesAreSet_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var model = new DestinationAttractionModel
        {
            Name = "Test Attraction",
            Description = "Test Description",
            Category = "Test Category"
        };

        // Assert
        model.Name.Should().Be("Test Attraction");
        model.Description.Should().Be("Test Description");
        model.Category.Should().Be("Test Category");
    }

    [Test]
    public void Create_WhenUsedInGetDestinationByIdResponse_ShouldWorkCorrectly()
    {
        // Arrange
        var attraction = new DestinationAttractionModel
        {
            Name = "Museum",
            Description = "A beautiful museum",
            Category = "Culture"
        };

        var response = new GetDestinationByIdResponse
        {
            Place = "Paris",
            Description = "City of Lights",
            Image = "https://example.com/paris.jpg",
            Attractions = new List<DestinationAttractionModel> { attraction },
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        response.Attractions.Should().NotBeEmpty();
        response.Attractions.Should().HaveCount(1);
        response.Attractions.First().Name.Should().Be("Museum");
        response.Attractions.First().Description.Should().Be("A beautiful museum");
        response.Attractions.First().Category.Should().Be("Culture");
    }

    [Test]
    public void Create_WhenMultipleAttractions_ShouldWorkCorrectly()
    {
        // Arrange
        var attractions = new List<DestinationAttractionModel>
        {
            new DestinationAttractionModel
            {
                Name = "Attraction 1",
                Description = "Description 1",
                Category = "Category 1"
            },
            new DestinationAttractionModel
            {
                Name = "Attraction 2",
                Description = "Description 2",
                Category = "Category 2"
            }
        };

        var response = new GetDestinationByIdResponse
        {
            Place = "Test Place",
            Description = "Test Description",
            Image = "https://example.com/test.jpg",
            Attractions = attractions,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        response.Attractions.Should().HaveCount(2);
        response.Attractions.Should().Contain(a => a.Name == "Attraction 1");
        response.Attractions.Should().Contain(a => a.Name == "Attraction 2");
    }

    [Test]
    public void Create_WhenEmptyAttractions_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var response = new GetDestinationByIdResponse
        {
            Place = "Test Place",
            Description = "Test Description",
            Image = "https://example.com/test.jpg",
            Attractions = new List<DestinationAttractionModel>(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        response.Attractions.Should().BeEmpty();
    }
}
