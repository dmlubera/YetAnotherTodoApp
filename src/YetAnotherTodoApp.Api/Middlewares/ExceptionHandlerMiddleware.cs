using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Domain.Exceptions;
using ApplicationException = YetAnotherTodoApp.Application.Exceptions.ApplicationException;

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
                case DomainException e when ex.GetType().BaseType == typeof(DomainException):
                    errorCode = e.Code;
                    statusCode = HttpStatusCode.BadRequest;
                    message = e.Message;
                    break;
                case ApplicationException e when ex.GetType().BaseType == typeof(ApplicationException):
                    errorCode = e.Code;
                    statusCode = HttpStatusCode.BadRequest;
                    message = e.Message;
                    break;
                default: break;
            }

            var errorResponse = new ErrorResponse { Code = errorCode, Message = message };
            var payload = JsonConvert.SerializeObject(errorResponse);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        } 
    }
}