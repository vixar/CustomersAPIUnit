using System.Net;
using Application.Exceptions;
using Application.Extentions;
using System.Text.Json;

namespace Customers.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = await ApiResponse.Response<string>.FailAsync(error.Message);

                switch (error)
                {
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case HttpException e:
                        // 
                        HandleExceptionAsync(context, e);
                        break;

                    case Exception ex:

                        HandleExceptionAsync(context, ex);
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.BadGateway;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }

        private void HandleExceptionAsync(HttpContext context, HttpException exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is HttpException)
            {
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

        }

        private void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
}