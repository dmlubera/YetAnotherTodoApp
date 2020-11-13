using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorCode = "internal_server_error";
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Oops, something went wrong.";

            switch(ex)
            {
                case DomainException e when ex.GetType() == typeof(DomainException):
                    errorCode = e.Code;
                    statusCode = HttpStatusCode.BadRequest;
                    message = e.Message;
                    break;
                default: break;
            }

            var response = new { code = errorCode, message = message };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        } 
    }
}