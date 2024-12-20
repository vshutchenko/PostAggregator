﻿using FluentAssertions;
using PostAggregator.Api.Data.Specification;

namespace PostAggregator.Test.SpecificationsTests;
[TestFixture]
public class OrderBySpecificationTests
{
    [Test]
    public void GetSqlQuery_ShouldReturnCorrectSql_WhenAscIsTrue()
    {
        // Arrange
        var spec = new OrderBySpecification("column_name", true);

        // Act
        var result = spec.GetSqlQuery();

        // Assert
        result.Should().BeEquivalentTo(
            $"{Environment.NewLine}ORDER BY column_name");
    }

    [Test]
    public void GetSqlQuery_ShouldReturnCorrectSql_WhenAscIsFalse()
    {
        // Arrange
        var spec = new OrderBySpecification("column_name", false);

        // Act
        var result = spec.GetSqlQuery();

        // Assert
        result.Should().BeEquivalentTo(
            $"{Environment.NewLine}ORDER BY column_name DESC");
    }

    [Test]
    public void GetSqlQuery_ShouldReturnCorrectSql_WhenColumnIsCreatedAtUtc_AndAscIsTrue()
    {
        // Arrange
        var spec = new OrderBySpecification("createdatutc", true);

        // Act
        var result = spec.GetSqlQuery();

        // Assert
        result.Should().BeEquivalentTo($"{Environment.NewLine}ORDER BY datetime(createdatutc)");
    }

    [Test]
    public void GetSqlQuery_ShouldReturnCorrectSql_WhenColumnIsCreatedAtUtc_AndAscIsFalse()
    {
        // Arrange
        var spec = new OrderBySpecification("createdatutc", false);

        // Act
        var result = spec.GetSqlQuery();

        // Assert
        result.Should().BeEquivalentTo($"{Environment.NewLine}ORDER BY datetime(createdatutc) DESC");
    }

    [Test]
    public void GetParameters_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var spec = new OrderBySpecification("column_name", true);

        // Act
        var result = spec.GetParameters();

        // Assert
        result.Should().BeEmpty();
    }
}
