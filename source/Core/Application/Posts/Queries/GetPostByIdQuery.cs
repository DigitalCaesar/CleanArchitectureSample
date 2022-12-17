using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Queries;
public sealed record GetPostByIdQuery(Guid PostId);// : IQuery<PostResponse>;
