using System.Threading.Tasks;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateTodoTests
    {
        public async Task WithValidData_ReturnsHttpStatusCodeOk()
        {

        }

        public async Task WithValidData_UpdateTaskInDatabase()
        {

        }

        public async Task WithInvalidData_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }

        public async Task WithEmptyTitle_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }

        public async Task WithFinishDateEarlierThanToday_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }
    }
}
