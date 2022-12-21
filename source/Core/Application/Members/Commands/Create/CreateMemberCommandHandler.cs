using Application.Abstractions.Messaging;
using Domain;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Members.Commands.Create;
internal sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Guid> 
{
    private readonly IMemberRepository mMemberRepository;
    private readonly IRoleRepository mRoleRepository;
    private readonly IEventRepository mEventRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork, IEventRepository eventRepository, IRoleRepository roleRepository)
    {
        mMemberRepository = memberRepository;
        mUnitOfWork = unitOfWork;
        mEventRepository = eventRepository;
        mRoleRepository = roleRepository;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request, CancellationToken cancellationToken = default)
    {
        // Check values
        var userName = UserName.Create(request.Username);
        if (!userName.Successful || userName.Value is null)
            return Result<Guid>.Failure(new Error("UserName", $"The requested username '{request.Username}' is invalid."));
        var email = Email.Create(request.Email);
        if (!email.Successful || email.Value is null)
            return Result<Guid>.Failure(new Error("Email", $"The requested Email '{request.Email}' is invalid."));
        var firstName = FirstName.Create(request.FirstName);
        if (!firstName.Successful || firstName.Value is null)
            return Result<Guid>.Failure(new Error("FirstName", $"The requested FirstName '{request.FirstName}' is invalid."));
        var lastName = LastName.Create(request.LastName);
        if (!lastName.Successful || lastName.Value is null)
            return Result<Guid>.Failure(new Error("LastName", $"The requested FirstName '{request.LastName}' is invalid."));

        // Check uniqueness
        if(!await mMemberRepository.IsEmailUniqueAsync(email.Value, cancellationToken))
            return Result<Guid>.Failure(DomainErrors.Member.DuplicateEmail(email.Value.ToString()));
        if (!await mMemberRepository.IsUsernameUniqueAsync(userName.Value, cancellationToken))
            return Result<Guid>.Failure(DomainErrors.Member.DuplicateUsername(userName.Value.ToString()));

        // Set Default Role
        Role? DefaultRole = await mRoleRepository.GetByName("User", cancellationToken);
        List<Role> Roles = (DefaultRole is not null) ? new() { DefaultRole } : new();
        
        // Create the new item
        var NewItem = Member.Create(
            userName.Value, 
            email.Value, 
            firstName.Value, 
            lastName.Value,
            Roles);

        // Save the item
        await mMemberRepository.CreateAsync(NewItem, cancellationToken);
        await mEventRepository.AddEventAsync(NewItem, cancellationToken);
        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(NewItem.Id);
    }
}
