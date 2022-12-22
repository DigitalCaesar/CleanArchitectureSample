using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTest;
public class Architecture_Api_Tests : Architecture_Tests
{
    [Fact(Skip = "Api is at the top and has dependency on all below.")]
    public void Api_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            ""
        };

        // Act
        var result = Types
            .InAssembly(Api.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Api_Should_Have_DependOnOtherProject()
    {
        // Arrange
        var otherProject = new[]
        {
            ApplicationNamespace
        };

        // Act
        var result = Types
            .InAssembly(Api.AssemblyReference.Assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Api_Should_Have_DependOnMediatR()
    {
        // Arrange
        var otherProject = new[]
        {
            "MediatR"
        };

        // Act
        var result = Types
            .InAssembly(Api.AssemblyReference.Assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
