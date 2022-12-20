using MediatR;

namespace Api.Controllers;

public sealed class PostController : ApiController
{
    public PostController(ISender sender) : base(sender)
    {
    }
}
