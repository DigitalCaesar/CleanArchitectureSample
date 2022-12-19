using Data.Models;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using Domain.ValueObjects;
using Data.Exceptions;

namespace Data.Mapping;
internal static class MemberMapping
{
    public static MemberData Map(this Member entity)
    {
        List<RoleData> Roles = entity.Roles.Select(x => (RoleData)x.Map()).ToList();

        MemberData NewItem = new MemberData()
        {
            Id = entity.Id,
            Username = entity.Username.Value,
            Email= entity.Email.Value,
            FirstName= entity.FirstName.Value,
            LastName = entity.LastName.Value,
            Roles = Roles
        };
        return NewItem;
    }
    public static Member Map(this MemberData data)
    {
        UserName? UserName = UserName.Create(data.Username).Value;
        Email? Email = Email.Create(data.Email).Value;
        FirstName? FirstName = FirstName.Create(data.FirstName).Value;
        LastName? LastName = LastName.Create(data.LastName).Value;
        List<Role> Roles = data.Roles.Select(x => (Role)x.Map()).ToList();

        if (
            UserName is null ||
            Email is null ||
            FirstName is null ||
            LastName is null)
            throw new Data.Exceptions.InvalidDataException("MemberData");

        Member NewItem = Member.Create(
            data.Id, 
            UserName,
            Email,
            FirstName, 
            LastName, 
            Roles);

        return NewItem;
    }
}
