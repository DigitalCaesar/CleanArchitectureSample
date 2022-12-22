using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Members;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using Infrastructure.Authentication;

namespace Application.Login;
internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IMemberRepository mMemberRepository;
    private readonly IJwtProvider mJwtProvider;

    public LoginCommandHandler(IMemberRepository memberRepository, IJwtProvider jwtProvider)
    {
        mMemberRepository = memberRepository;
        mJwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);
        Member? member = await mMemberRepository.GetByEmailAsync(email.Value, cancellationToken);
        if (member is null)
            return Result.Failure<string>(DomainErrors.Member.InvalidCredentials);

        string token = mJwtProvider.Generate(member);

        return token;
    }
}
