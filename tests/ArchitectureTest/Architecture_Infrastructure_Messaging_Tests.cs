using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTest;
public class Architecture_Infrastructure_Messaging_Tests : Architecture_Tests
{
    [Fact]
    public void Messaging_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(Infrastructure.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
