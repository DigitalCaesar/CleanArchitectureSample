using Domain.Entities.Members;

namespace Infrastructure.Authentication;
public interface IJwtProvider
{
    string Generate(MemberEntity member);
}
