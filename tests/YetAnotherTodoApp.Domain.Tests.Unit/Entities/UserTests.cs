using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class UserTests
    {
        [Fact]
        public void Create_WhenValidData_ThenShouldCreateUser()
        {
            var username = "username";
            var email = "user@yetanothertodoapp.com";
            var passwordHash = "hash1234$%^&";
            var passwordSalt = "salt1234$%^&";

            var user = new User(username, email, passwordHash, passwordSalt);

            user.Should().NotBeNull();
            user.Username.Value.Should().Be(username);
            user.Email.Value.Should().Be(email);
            user.Password.Hash.Should().Be(passwordHash);
            user.Password.Salt.Should().Be(passwordSalt);
            user.Name.Should().BeNull();
            user.TodoLists.Should().HaveCount(1);
        }

        [Fact]
        public void AddTodoList_WhenTodoListWithGivenTitleExists_ThenShouldThrowAnException()
        {
            var user = CreateUser();
            var inbox = new TodoList("Inbox");

            Assert.Throws<TodoListWithGivenTitleAlreadyExistsException>(() => user.AddTodoList(inbox));
        }

        [Fact]
        public void AddTodoList_WhenTodoListWithGivenNameDoesNotExists_ThenShouldAddTodoList()
        {
            var user = CreateUser();
            var todoList = new TodoList("Title");

            user.AddTodoList(todoList);

            user.TodoLists.Should().HaveCount(2);
        }

        [Fact]
        public void DeleteTodoList_WhenTodoListToDeleteIsInbox_ThenShouldThrowAnException()
        {
            var user = CreateUser();
            var inboxId = user.TodoLists.FirstOrDefault(x => x.Title == "Inbox").Id;

            Assert.Throws<InboxDeletionIsNotAllowedException>(() => user.DeleteTodoList(inboxId));
        }

        [Fact]
        public void DeleteTodoList_WhenTodoListWithGivenIdDoesNotBelongToUser_ThenShouldThrowAnExcepion()
        {
            var user = CreateUser();

            Assert.Throws<TodoListWithGivenIdDoesNotExistException>(() => user.DeleteTodoList(Guid.NewGuid()));
        }

        [Fact]
        public void DeleteTodoList_WhenValidData_ThenShouldDeleteTodoList()
        {
            var user = CreateUser();
            var todoList = new TodoList("Title");
            user.AddTodoList(todoList);

            user.DeleteTodoList(todoList.Id);

            user.TodoLists.Should().HaveCount(1);
            user.TodoLists.FirstOrDefault().Title.Value.Should().Be("Inbox");
        }

        [Fact]
        public void UpdateUserInfo_WhenValidData_ThenShouldUpdateUser()
        {
            var user = CreateUser();
            var firstName = "John";
            var lastName = "Doe";

            user.UpdateUserInfo(firstName, lastName);

            user.Name.FirstName.Should().Be(firstName);
            user.Name.LastName.Should().Be(lastName);
        }

        [Fact]
        public void UpdateEmail_WhenValidData_ThenShouldUpdateEmail()
        {
            var user = CreateUser();
            var updatedEmail = "updateduser@yetanothertodoapp.com";

            user.UpdateEmail(updatedEmail);

            user.Email.Value.Should().Be(updatedEmail);
        }

        [Fact]
        public void UpdatePassword_WhenValidData_ThenShouldUpdatePassword()
        {
            var user = CreateUser();
            var updatedPasswordHash = "updatedHash1234$%^&";
            var updatedPasswordSalt = "updatedSalt1234$%^&";

            user.UpdatePassword(updatedPasswordHash, updatedPasswordSalt);

            user.Password.Hash.Should().Be(updatedPasswordHash);
            user.Password.Salt.Should().Be(updatedPasswordSalt);
        }

        private static User CreateUser()
            => new User("username", "user@yetanothertodoapp.com", "hash1234$%^&", "salt1234$%^&");
    }
}