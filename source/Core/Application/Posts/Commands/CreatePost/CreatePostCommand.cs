using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Posts.Commands.CreatePost;
public sealed record CreatePostCommand(string Name); //: ICommand<Guid>;
