using Application.Abstractions.Messaging;

namespace Application.Members.Queries.GetMemberById;
public sealed record GetMemberByIdQuery(Guid Id) : IQuery<MemberResponse>;
