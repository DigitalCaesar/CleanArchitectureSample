namespace Api.Middleware;

public class ExceptionHandlingMiddlewareSimple
{
    private readonly RequestDelegate mNext;
    private readonly ILogger<ExceptionHandlingMiddlewareSimple> mLogger;
    public ExceptionHandlingMiddlewareSimple(RequestDelegate next, ILogger<ExceptionHandlingMiddlewareSimple> logger)
    {
        mNext = next;
        mLogger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await mNext(context);
        }
        catch (Exception ex)
        {
            mLogger.LogError(ex, ex.Message);
        }
    }
}
