using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTest;
public class Architecture_Api_Tests : Architecture_Tests
{
    [Fact]
    public void Api_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            InfrastructureNamespace,
            DataNamespace,
            MessagingNamespace
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
    //[Fact]
    //public void Api_Should_Have_DependOnMediatR()
    //{
    //    // Arrange
    //    var otherProject = new[]
    //    {
    //        "MediatR"
    //    };

    //    // Act
    //    var result = Types
    //        .InAssembly(Api.AssemblyReference.Assembly)
    //        .That()
    //        .HaveNameEndingWith("Controller")
    //        .Should()
    //        .HaveDependencyOnAll(otherProject)
    //        .GetResult();

    //    // Assert
    //    result.IsSuccessful.Should().BeTrue();
    //}
}
