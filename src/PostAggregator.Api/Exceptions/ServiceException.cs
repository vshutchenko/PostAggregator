namespace PostAggregator.Api.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(int statusCode, string displayMessage, string displayTitle)
    {
        StatusCode = statusCode;
        DisplayMessage = displayMessage;
        DisplayTitle = displayTitle;
    }

    public int StatusCode { get; }
    public string DisplayMessage { get; }
    public string DisplayTitle { get; }
}
