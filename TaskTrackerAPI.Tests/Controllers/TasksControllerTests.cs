using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Controllers;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;
using Xunit;

namespace TaskTrackerAPI.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly TaskDbContext? _context;
        private readonly TasksController? _controller;

        public TasksControllerTests()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TaskDbContext(options);
            _controller = new TasksController(_context);
        }

        [Fact]
        public async Task GetTasks_ReturnsEmplyList_WhenNoTasksExists()
        {
            var result = await _controller.GetTasks();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
            Assert.Empty(tasks);
        }

        [Fact]
        public async Task CreateTask_AddsNewTask()
        {
            var newTask = new TaskItem { Title = "Write unit tests" };

            var result = await _controller.CreateTask(newTask);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var task = Assert.IsType<TaskItem>(createdResult.Value);
            Assert.Equal("Write unit tests", task.Title);
        }

        [Fact]
        public async Task GetTask_ReturnsCorrectTask()
        {
            var task = new TaskItem { Title = "Read docs" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _controller.GetTask(task.Id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("Read docs", returnedTask.Title);
        }

        [Fact]
        public async Task UpdateTask_ChangesTitle()
        {
            var task = new TaskItem { Title = "Old Title" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            //Detach the first tracked entity in order to update new task with same id
            _context.Entry(task).State = EntityState.Detached;

            var updatedTask = new TaskItem
            {
                Id = task.Id,
                Title = "New Title",
                IsCompleted = false,
            };
            var result = await _controller.UpdateTask(task.Id, updatedTask);
            Assert.IsType<NoContentResult>(result);

            var dbTask = await _context.Tasks.FindAsync(task.Id);
            Assert.Equal("New Title", dbTask.Title);
        }

        [Fact]
        public async Task DeleteTask_RemovesTask()
        {
            var task = new TaskItem { Title = "To be deleted" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteTask(task.Id);
            Assert.IsType<NoContentResult>(result);

            var deleted = await _context.Tasks.FindAsync(task.Id);
            Assert.Null(deleted);
        }
    }
}
