using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.JSInterop;
using RentalService.Api.Contracts.Common;

namespace RentalService.Api.Filters;

public class RentalServiceExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse
        {
            Timestamp = DateTime.Now,
            StatusPhrase = "Internal server error",
            StatusCode = 500,
        };
        
        apiError.Errors.Add(context.Exception.Message);

        context.Result = new JsonResult(apiError) {StatusCode = 500};
    }
}