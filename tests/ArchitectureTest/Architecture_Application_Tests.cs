﻿using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTest;
public class Architecture_Application_Tests : Architecture_Tests
{
    [Fact]
    public void Application_Should_Not_DependOnOtherProject()
    {

        // Arrange
        var otherProject = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace,
            ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(Application.AssemblyReference.Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Application_Should_Have_DependOnDomain()
    {

        // Arrange
        var otherProject = new[]
        {
            DomainNamespace
        };

        // Act
        var result = Types
            .InAssembly(Application.AssemblyReference.Assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Application_Should_Have_DependOnMediatR()
    {
        // Arrange
        var otherProject = new[]
        {
            "MediatR"
        };

        // Act
        var result = Types
            .InAssembly(Application.AssemblyReference.Assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOnAll(otherProject)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
