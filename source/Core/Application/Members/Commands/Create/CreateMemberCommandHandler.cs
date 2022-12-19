using Domain;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Commands.Create;
internal sealed class CreateMemberCommandHandler //: IRequestHandler<CreatePostCommand, Guid> // : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IMemberRepository mMemberRepository;
    private readonly IUnitOfWork mUnitOfWork;

    public CreateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        mMemberRepository = memberRepository;
        mUnitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken = default)
    {
        var userName = UserName.Create(request.Username);
        if (!userName.Successful || userName.Value is null)
            throw new ValidationException($"The requested name '{request.Username}' is invalid."); //TODO: Add Error info

        var email = Email.Create(request.Email);
        if (!email.Successful || email.Value is null)
            throw new ValidationException($"The requested post content '{request.Email}' is invalid."); //TODO: Add Error info

        var firstName = FirstName.Create(request.FirstName);
        if (!firstName.Successful || firstName.Value is null)
            throw new ValidationException($"The requested name '{request.FirstName}' is invalid."); //TODO: Add Error info

        var lastName = LastName.Create(request.LastName);
        if (!lastName.Successful || lastName.Value is null)
            throw new ValidationException($"The requested name '{request.LastName}' is invalid."); //TODO: Add Error info


        var NewItem = Member.Create(
            userName.Value, 
            email.Value, 
            firstName.Value, 
            lastName.Value,
            request.Roles);

        await mMemberRepository.CreateAsync(NewItem, cancellationToken);

        await mUnitOfWork.SaveChangesAsync(cancellationToken);

        return NewItem.Id;
    }
}
