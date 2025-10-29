using Microsoft.Identity.Client;
using Store.Domain.Exceptions;
using Store.Shard.ErrorModels;

namespace Store.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.ContentType = "application/json ";
                    var response = new ErrorDetalis()
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"End Point {context.Request.Path} is Not Found"
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }catch(Exception ex)
            {
                //log Exception
                _logger.LogError(ex, ex.Message);
                //1. set Status code For Response
                //2.Set Content Type Code For Response
                //3. Response Object (Body)
                //4. Return Response


                context.Response.ContentType = "application/json";

                var response = new ErrorDetalis()
                {
                    ErrorMessage = ex.Message
                };
                response.statusCode = ex switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                   _ => StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }

    }
}
