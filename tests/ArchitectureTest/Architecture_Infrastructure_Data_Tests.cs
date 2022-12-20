using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTest;
public class Architecture_Infrastructure_Data_Tests : Architecture_Tests
{
    [Fact]
    public void Data_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(Data.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
