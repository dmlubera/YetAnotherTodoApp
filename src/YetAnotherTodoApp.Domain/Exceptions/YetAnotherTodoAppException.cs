using System;

namespace YetAnotherTodoApp.Domain.Exceptions
{
    public abstract class YetAnotherTodoAppException : Exception
    {
        public string Code { get; }

        protected YetAnotherTodoAppException()
        {
        }

        protected YetAnotherTodoAppException(string code)
        {
            Code = code;
        }

        protected YetAnotherTodoAppException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        protected YetAnotherTodoAppException(string code, string message, params object[] args)
            : this(null, code, message, args)
        { 
        }

        protected YetAnotherTodoAppException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {

        }

        protected YetAnotherTodoAppException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }

    }
}