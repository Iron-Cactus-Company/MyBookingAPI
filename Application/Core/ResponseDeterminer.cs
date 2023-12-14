using Application.Core.Error;
using Application.Core.Error.Enums;

namespace Application.Core;

public class ResponseDeterminer
{
    public static (bool isValid, ApplicationRequestError? error) DetermineCreateResponse(int dbResponse)
    {
        var isSuccess = dbResponse > 0;

        if (!isSuccess)
        {
            var error = new ApplicationRequestError
            {
                Type = ErrorType.NothingChanged
            };
            return (false, error);
        }
        
        return (true, null);
    }
    
    public static (bool isValid, ApplicationRequestError? error) DetermineUpdateResponse(int dbResponse)
    {
        var isSuccess = dbResponse > 0;

        if (!isSuccess)
        {
            var error = new ApplicationRequestError
            {
                Type = ErrorType.NothingChanged
            };
            return (false, error);
        }
        
        return (true, null);
    }
    
    public static (bool isValid, ApplicationRequestError? error) DetermineDeleteResponse(int dbResponse)
    {
        var isSuccess = dbResponse > 0;

        if (!isSuccess)
        {
            var error = new ApplicationRequestError
            {
                Type = ErrorType.NothingChanged
            };
            return (false, error);
        }
        
        return (true, null);
    }
}