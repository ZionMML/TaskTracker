using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;
using Xunit;

namespace TaskTrackerAPI.Tests.Data
{
    public class TaskDbContextTests
    {
        [Fact]
        public async Task CanAddAndRetrieveTask()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .Options;

            using (var context = new TaskDbContext(options))
            {
                context.Tasks.Add(new TaskItem { Title = "Integration Test" });
                await context.SaveChangesAsync();
            }

            using (var context = new TaskDbContext(options))
            {
                var task = await context.Tasks.FirstAsync();
                Assert.Equal("Integration Test", task.Title);
            }
        }
    }
}
