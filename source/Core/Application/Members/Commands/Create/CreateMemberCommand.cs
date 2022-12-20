using Application.Abstractions.Messaging;
using Domain.Entities.Members;
using Domain.Entities.Roles;
using MediatR;

namespace Application.Members.Commands.Create;
public sealed record CreateMemberCommand(
    string Username, 
    string Email,
    string FirstName,
    string LastName,
    List<Role> Roles) : ICommand<Guid>; 
