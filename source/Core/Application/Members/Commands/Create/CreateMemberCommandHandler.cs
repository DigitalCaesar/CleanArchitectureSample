using Application.Abstractions.Messaging;
using Data.Models;
using Domain;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Exceptions;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.Members.Commands.Create;
internal sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand> 
{
    private readonly IMemberRepository mMemberRepository;
    private readonly IEventRepository mEventRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository)
    {
        mMemberRepository = memberRepository;
        mUnitOfWork = unitOfWork;
        mEventRepository = eventRepository;
    }

    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken = default)
    {
        var userName = UserName.Create(request.Username);
        if (!userName.Successful || userName.Value is null)
            return Result.Failure(new Error("UserName", $"The requested username '{request.Username}' is invalid."));
        var email = Email.Create(request.Email);
        if (!email.Successful || email.Value is null)
            return Result.Failure(new Error("Email", $"The requested Email '{request.Email}' is invalid."));
        var firstName = FirstName.Create(request.FirstName);
        if (!firstName.Successful || firstName.Value is null)
            return Result.Failure(new Error("FirstName", $"The requested FirstName '{request.FirstName}' is invalid."));
        var lastName = LastName.Create(request.LastName);
        if (!lastName.Successful || lastName.Value is null)
            return Result.Failure(new Error("LastName", $"The requested FirstName '{request.LastName}' is invalid."));

        //TODO:  Find a better strategy for uniqueness check
        if(!await mMemberRepository.IsEmailUniqueAsync(email.Value, cancellationToken))
            return Result.Failure(new Error("Email", "Email must be unique"));
        if (!await mMemberRepository.IsUsernameUniqueAsync(userName.Value, cancellationToken))
            return Result.Failure(new Error("Username", "Username must be unique"));

        var NewItem = Member.Create(
            userName.Value, 
            email.Value, 
            firstName.Value, 
            lastName.Value,
            request.Roles);

        await mMemberRepository.CreateAsync(NewItem, cancellationToken);
        await mEventRepository.AddEventAsync(NewItem, cancellationToken);

        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
