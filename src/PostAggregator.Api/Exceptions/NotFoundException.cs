using System.Net;

namespace PostAggregator.Api.Exceptions;

public class NotFoundException : ServiceException
{
    public NotFoundException(string displayMessage)
        : base((int)HttpStatusCode.NotFound, displayMessage, "Not Found")
    {
    }
}
