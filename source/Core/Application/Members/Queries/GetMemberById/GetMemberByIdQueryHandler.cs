using Domain.Exceptions;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Entities.Members;

namespace Application.Members.Queries.GetMemberById;
internal sealed class GetMemberByIdQueryHandler : IQueryHandler<GetMemberByIdQuery, MemberResponse>
{
    private readonly IMemberRepository repository;

    public GetMemberByIdQueryHandler(IMemberRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result<MemberResponse>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken = default)
    {
        var member = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
            return Result.Failure<MemberResponse>(new Error(
                "Member.NotFound",
                $"The Member with Id '{request.Id}' was not found."));

        var response = new MemberResponse(member.Id, member.Email.Value);

        return response;
    }
}
