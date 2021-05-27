using System.Threading.Tasks;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class AddTodoTests
    {
        public async Task WithValidData_ReturnsHttpStatusCodeCreatedWithLocationHeader()
        {

        }

        public async Task WithValidData_AddTodoToDatabase()
        {

        }

        public async Task WithoutProjectName_ReturnsHttpStatusCodeCreatedWithLocationHeader()
        {

        }

        public async Task WithoutProjectName_CreateTodoIntoInboxAndAddToDatabase()
        {

        }

        public async Task WithTasks_ReturnsHttpStatusCodeCreatedWithLocationHeader()
        {

        }
        
        public async Task WithTasks_AddTodoAndAssignedTasksToDatabase()
        {

        }

        public async Task WithFinishDateEarlierThanToday_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }

        public async Task WithoutTitle_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }

        public async Task WithoutFinishDate_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }

        public async Task WithExactlyTheSameDataAsTheExisting_ReturnsHttpStatusBadRequestWithCustomException()
        {

        }
    }
}
