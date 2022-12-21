using Application.Members.Commands.Create;
using Domain;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> memberRepositoryMock;
    private readonly Mock<IRoleRepository> roleRepositoryMock;
    private readonly Mock<IEventRepository> eventRepositoryMock;
    private readonly Mock<IUnitOfWork> unitOfWorkMock;

    public CreateMemberCommandHandlerTests()
    {
        memberRepositoryMock = new();
        roleRepositoryMock = new();
        eventRepositoryMock = new();
        unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        string testEmailAddress = "UnitTestUser@email.com";
        var TestCommand = new CreateMemberCommand("UnitTestUser", testEmailAddress, "UnitTestFirstName", "UnitTestLastName");
        var Handler = new CreateMemberCommandHandler(
            memberRepositoryMock.Object, 
            unitOfWorkMock.Object, 
            eventRepositoryMock.Object, 
            roleRepositoryMock.Object);
        memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        memberRepositoryMock.Setup(
            x => x.IsUsernameUniqueAsync(
                It.IsAny<UserName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<Guid> Result = await Handler.Handle(TestCommand, default);

        // Assert
        Result.Successful.Should().BeFalse();
        Result.Error.Should().Be(DomainErrors.Member.DuplicateEmail(testEmailAddress));
    }
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        // Arrange
        string testEmailAddress = "UnitTestUser@email.com";
        var TestCommand = new CreateMemberCommand("UnitTestUser", testEmailAddress, "UnitTestFirstName", "UnitTestLastName");
        var Handler = new CreateMemberCommandHandler(
            memberRepositoryMock.Object,
            unitOfWorkMock.Object,
            eventRepositoryMock.Object,
            roleRepositoryMock.Object);
        memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        memberRepositoryMock.Setup(
            x => x.IsUsernameUniqueAsync(
                It.IsAny<UserName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<Guid> Result = await Handler.Handle(TestCommand, default);

        // Assert
        Result.Successful.Should().BeTrue();
        Result.Value.Should().NotBeEmpty();
    }
    [Fact]
    public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUnique()
    {
        // Arrange
        string testEmailAddress = "UnitTestUser@email.com";
        var TestCommand = new CreateMemberCommand("UnitTestUser", testEmailAddress, "UnitTestFirstName", "UnitTestLastName");
        var Handler = new CreateMemberCommandHandler(
            memberRepositoryMock.Object,
            unitOfWorkMock.Object,
            eventRepositoryMock.Object,
            roleRepositoryMock.Object);
        memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        memberRepositoryMock.Setup(
            x => x.IsUsernameUniqueAsync(
                It.IsAny<UserName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<Guid> Result = await Handler.Handle(TestCommand, default);

        // Assert
        memberRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<Member>(m => m.Id == Result.Value), 
                It.IsAny<CancellationToken>()), 
            Times.Once);

    }
    [Fact]
    public async Task Handle_Should_NotCallUnitOfWork_WhenEMailIsNotUnique()
    {
        // Arrange
        string testEmailAddress = "UnitTestUser@email.com";
        var TestCommand = new CreateMemberCommand("UnitTestUser", testEmailAddress, "UnitTestFirstName", "UnitTestLastName");
        var Handler = new CreateMemberCommandHandler(
            memberRepositoryMock.Object,
            unitOfWorkMock.Object,
            eventRepositoryMock.Object,
            roleRepositoryMock.Object);
        memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        memberRepositoryMock.Setup(
            x => x.IsUsernameUniqueAsync(
                It.IsAny<UserName>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<Guid> Result = await Handler.Handle(TestCommand, default);

        // Assert
        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

}