using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.Common;   
using RentalService.Application.Enums;
using RentalService.Application.Models;

namespace RentalService.Api.Controllers;

public class BaseController : ControllerBase
{

    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        ErrorResponse apiError;

        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.First(e => e.Code == ErrorCode.NotFound);
            apiError = new ErrorResponse
            {
                Timestamp = DateTime.Now,
                StatusPhrase = "Not Found",
                StatusCode = 404,
            };
            apiError.Errors.Add(error.Message);
            return NotFound(apiError);
        }
        
        
        if (errors.Any(e => e.Code == ErrorCode.ValidationError))
        {
            apiError = new ErrorResponse
            {
                Timestamp = DateTime.Now,
                StatusPhrase = "Validation error",
                StatusCode = 101
            };
            errors.Where(e => e.Code == ErrorCode.ValidationError).ToList().ForEach(e =>
            {
                apiError.Errors.Add(e.Message);
            });
            
            
            return StatusCode(404, apiError);
        }
        

        apiError = new ErrorResponse
        {
            Timestamp = DateTime.Now,
            StatusPhrase = "Internal server error",
            StatusCode = 500
        };
        apiError.Errors.Add("Unknown error");

        return StatusCode(500, apiError);
    }
}