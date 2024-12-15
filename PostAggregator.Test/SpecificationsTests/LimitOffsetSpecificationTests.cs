using FluentAssertions;
using PostAggregator.Api.Data.Specification;

namespace PostAggregator.Test.SpecificationsTests;
[TestFixture]
public class LimitOffsetSpecificationTests
{
    [Test]
    public void GetSqlQuery_ShouldReturnCorrectSql()
    {
        // Arrange
        var spec = new LimitOffsetSpecification(2, 10);

        // Act
        var result = spec.GetSqlQuery();

        // Assert
        result.Should().BeEquivalentTo(
            $"{Environment.NewLine}LIMIT @limit OFFSET @offset");
    }

    [Test]
    public void GetParameters_ShouldReturnCorrectParameters()
    {
        // Arrange
        var spec = new LimitOffsetSpecification(2, 10);

        // Act
        var result = spec.GetParameters();

        // Assert
        result.Should().ContainKey("@limit").WhoseValue.Should().Be(10);
        result.Should().ContainKey("@offset").WhoseValue.Should().Be(10);
    }
}