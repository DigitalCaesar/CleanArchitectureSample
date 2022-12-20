using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTest;
public class Architecture_Domain_Tests
{
    protected const string DomainNamespace = "Domain";
    protected const string ApplicationNamespace = "Application";
    protected const string InfrastructureNamespace = "Infrastructure";
    protected const string DataNamespace = "Data";
    protected const string MessagingNamespace = "Messaging";
    protected const string PresentationNamespace = "Presentation";
    protected const string ApiNamespace = "Api";

    [Fact]
    public void Domain_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(Domain.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
