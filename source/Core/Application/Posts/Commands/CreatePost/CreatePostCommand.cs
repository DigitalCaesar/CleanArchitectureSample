using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Members;
using Domain.Entities.Tags;
using MediatR;

namespace Application.Posts.Commands.CreatePost;
public sealed record CreatePostCommand(string Name, string Content, Member Author, List<Tag> Tags); //: ICommand<Guid>;
