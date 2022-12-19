using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Application.Posts.Queries;
internal sealed class GetPostQueryHandler //: IQueryHandler<GetPostByIdQuery, PostResponse>
{
    private readonly IDbConnection mDbConnection;

    public GetPostQueryHandler(IDbConnection dbConnection)
    {
        mDbConnection = dbConnection;
    }

    public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken = default)
    {
        //NOTE:  Skips Repo for more performance
        const string sql = @"SELECT * FROM ""Posts"" WHERE ""Id"" = @Id";
        var post = await mDbConnection.QueryFirstOrDefaultAsync<PostResponse>(sql, new { request.Id });

        if(post is null)
            throw new PostNotFoundException(request.Id);

        return post;
    }
}
