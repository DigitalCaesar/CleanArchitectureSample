namespace Api.Startup;

public static class LoggingStartup
{
    public static void AddApplicationLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
    }
}
