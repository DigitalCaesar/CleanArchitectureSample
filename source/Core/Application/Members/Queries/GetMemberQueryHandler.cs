using Domain.Exceptions;
using System.Data;
using Dapper;

namespace Application.Members.Queries;
internal sealed class GetMemberQueryHandler //: IQueryHandler<GetPostByIdQuery, PostResponse>
{
    private readonly IDbConnection mDbConnection;

    public GetMemberQueryHandler(IDbConnection dbConnection)
    {
        mDbConnection = dbConnection;
    }

    public async Task<MemberResponse> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken = default)
    {
        //NOTE:  Skips Repo for more performance
        const string sql = @"SELECT * FROM ""Members"" WHERE ""Id"" = @Id";
        var post = await mDbConnection.QueryFirstOrDefaultAsync<MemberResponse>(sql, new { request.Id });

        if(post is null)
            throw new PostNotFoundException(request.Id);

        return post;
    }
}
