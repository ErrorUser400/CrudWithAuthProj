using CrudWithAuth.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrudWithAuth.Tests.TestParameter
{
    public class TestGetData : TheoryData<int, ToDoDto>
    {
        public TestGetData()
        {
            Add(1, new ToDoDto() { Id = 1, Title = "Test2", IsDone = false });
            Add(0, new ToDoDto() { Id = 0, Title = "Test", IsDone = true });
        }
    }
}
