using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentalService.Api.Contracts.Common;

namespace RentalService.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly List<string> _keys;
    public ValidateGuidAttribute(params string[] keys)
    {
        _keys = keys.ToList();
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var hasError = false;
        var apiError = new ErrorResponse();
        _keys.ForEach(k =>
        {
            if (!context.ActionArguments.TryGetValue(k, out var value) ||
                !Guid.TryParse(value?.ToString(), out _)) hasError = true;

            if (hasError)
            {
                apiError.Errors.Add($"The inputted identifier {k} is in wrong format");
            }
        });

        if (!hasError) return;

        apiError.Timestamp = DateTime.Now;
        apiError.StatusPhrase = "Bad request";
        apiError.StatusCode = 400;

        context.Result = new JsonResult(apiError) { StatusCode = 400 };
    }
    
}