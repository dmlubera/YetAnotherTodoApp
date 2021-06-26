using System;
using System.Linq;

namespace YetAnotherTodoApp.Tests.End2End.Helpers
{
    public static class UriHelper
    {
        public static Guid GetResourceId(this Uri uri)
            => Guid.Parse(uri.ToString().Split('/').Last());
    }
}