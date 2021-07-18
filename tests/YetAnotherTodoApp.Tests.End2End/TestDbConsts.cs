namespace YetAnotherTodoApp.Tests.End2End
{
    public static class TestDbConsts
    {
        #region User
        public static string TestUserEmail => "testuser@yetanothertodoapp.com";
        public static string TestUserUsername => "testuser";
        public static string TestUserPassword => "secretPassword";
        #endregion

        #region TodoList
        public static string TestTodoList => "testTodoList";
        public static string TestTodoListWithAssignedTodo => "testTodoListWithAssignedTodo";
        #endregion

        #region Todo
        public static string TestTodo => "testTodo";
        #endregion
    }
}