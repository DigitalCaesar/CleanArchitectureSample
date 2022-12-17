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

    public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        //NOTE:  Skips Repo for more performance
        const string sql = @"SELECT * FROM ""Webinars"" WHERE ""Id"" = @PostId";
        var post = await mDbConnection.QueryFirstOrDefaultAsync<PostResponse>(sql, new { request.PostId });

        if(post is null)
            throw new PostNotFoundException(request.PostId);

        return post;
    }
}
