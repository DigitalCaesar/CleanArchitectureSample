using Domain.Exceptions;
using System.Data;
using Dapper;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Entities.Posts;

namespace Application.Posts.Queries.GetPostById;
internal sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostResponse>
{
    //private readonly IDbConnection mDbConnection;
    private readonly IPostRepository repository;

    public GetPostByIdQueryHandler(IPostRepository repository)
    {
        this.repository = repository;
    }

    //public GetPostByIdQueryHandler(IDbConnection dbConnection)
    //{
    //    mDbConnection = dbConnection;
    //}

    public async Task<Result<PostResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken = default)
    {
        //NOTE:  Skips Repo for more performance
        //const string sql = @"SELECT * FROM ""Posts"" WHERE ""Id"" = @Id";
        //var post = await mDbConnection.QueryFirstOrDefaultAsync<PostResponse>(sql, new { request.Id });

        //if (post is null)
        //    throw new PostNotFoundException(request.Id);
        var post = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null) 
            return Result.Failure<PostResponse>(new Error(
                "Post.NotFound",
                $"The post with id '{request.Id}' was not found"));
        var response = new PostResponse(post.Id, post.Name.Value);

        return response;
    }
}
