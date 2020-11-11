using System;

namespace YetAnotherTodoApp.Domain.Extensions
{
    public static class StringExtension
    {
        public static void IsEmpty(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Value cannot be empty. Please provide valid data.");
        }
    }
}
