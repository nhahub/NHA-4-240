using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace CodeBook.Business.App.Middleware
{ 
public class GlobalExceptionHandler
 {
        /*private readonly RequestDelegate request;
        public GlobalExceptionHandler(RequestDelegate next) {
            request = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex) {
            var statusCode = ex switch {
                KeyNotFoundException => 404,
                ArgumentException => 400,
                UnauthorizedAccessException => 403,
                  _                      => 500
            };
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse {

                StatusCode = statusCode,
                Message = statusCode switch{
                    404 =>"Resource not found",
                    400 =>"Bad request",
                    403 =>"Forbidden",
                    _   =>"Unexpected error occurred"
                
                },
                Details=ex.Message
            };
            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }*/



 }
}