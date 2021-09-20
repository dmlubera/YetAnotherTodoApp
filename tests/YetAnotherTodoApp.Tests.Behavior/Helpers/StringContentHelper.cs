using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace YetAnotherTodoApp.Tests.Behavior.Helpers
{
    public static class StringContentHelper
    {
        public static StringContent GetStringContent(this object value)
            => new StringContent(JsonConvert.SerializeObject(value),
                    Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}