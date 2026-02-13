using CrudWithAuth.Controllers;
using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;
using CrudWithAuth.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CrudWithAuth.Tests.TestParameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CrudWithAuth.Tests
{
    public class ToDosService_Test
    {
        private readonly Mock<IToDoService> mockToDoService;
        private readonly User mockUser;
        private readonly List<ToDo> todosList;

        public ToDosService_Test()
        {
            this.mockToDoService = new Mock<IToDoService>();
            this.mockUser = new User()
            {
                Id = 0,
                PassWordHash = string.Empty,
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = null,
                Roles = UserRoles.None,
                ToDos = null,
                UserName = string.Empty
            };
            this.todosList = [
                    new (){
                        Id = 0,
                        Title = "Test",
                        CreatedDate = DateTime.UtcNow,
                        IsDone = true,
                        UserId = mockUser.Id,
                        User = mockUser
                    },
                    new (){
                        Id = 1,
                        Title = "Test2",
                        CreatedDate = DateTime.UtcNow,
                        IsDone = false,
                        UserId = mockUser.Id,
                        User = mockUser
                    }
                ];
        }

        [Fact]
        public async Task TestGetToDos()
        {
            List<ToDoDto> dtoList = [.. todosList.Select(t => new ToDoDto(t))];

            mockToDoService.Setup(um => um.GetToDosAsync())
                .Returns(Task.FromResult(dtoList));

            ToDoController toDoController = new(mockToDoService.Object);

            var result = await toDoController.GetToDos();

            Assert.NotNull(result);
            Assert.IsType<ActionResult<List<ToDoDto>>>(result);
        }

        [Theory]
        [ClassData(@class: typeof(TestGetData))]
        public async Task TestGetToDo(int id, ToDoDto expected)
        {
            var fromList = todosList.FirstOrDefault(u => u.Id == id);

            mockToDoService.Setup(um => um.GetToDoAsync(id))
                .Returns(
                    Task.FromResult(
                        new ToDoDto(fromList!)
                     )!
                );

            ToDoController toDoController = new(mockToDoService.Object);

            var actionResult = await toDoController.GetToDo(id);
            var result = GetObjectResultContent(actionResult);

            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.IsDone, result.IsDone);

        }

        //get the ToDoDto value of the actionresult
        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result!).Value!;
        }
    }
}
