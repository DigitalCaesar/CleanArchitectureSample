using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts;
public interface IPostRepository
{
    void Insert(Post post);
}
