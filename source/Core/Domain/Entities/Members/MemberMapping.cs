using Domain.ValueObjects;

namespace Domain.Entities.Members;
public static class MemberMapping
{
    public static Member Map(this MemberEntity entity)
    {
        //List<Role> Roles = entity.Roles.Select(x => x.Map()).ToList();

        Member NewItem = new Member()
        {
            Id = entity.Id,
            Username = entity.Username.Value,
            Email= entity.Email.Value,
            FirstName= entity.FirstName.Value,
            LastName = entity.LastName.Value,
            Roles = entity.Roles.ToList()
        };
        return NewItem;
    }
    public static MemberEntity Map(this Member data)
    {
        UserName? UserName = UserName.Create(data.Username).Value;
        Email? Email = Email.Create(data.Email).Value;
        FirstName? FirstName = FirstName.Create(data.FirstName).Value;
        LastName? LastName = LastName.Create(data.LastName).Value;
        //List<Role> Roles = data.Roles.Select(x => x.Map()).ToList();

        if (
            UserName is null ||
            Email is null ||
            FirstName is null ||
            LastName is null)
            throw new Domain.Exceptions.EntityMappingException("MemberData");

        MemberEntity NewItem = MemberEntity.Create(
            data.Id, 
            UserName,
            Email,
            FirstName, 
            LastName, 
            data.Roles.ToList());

        return NewItem;
    }
}
