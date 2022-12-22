using Application.Abstractions.Messaging;

namespace Application.Login;
public record LoginCommand(string Email) : ICommand<string>;
//TODO:  This needs to add a password and hash to validate
