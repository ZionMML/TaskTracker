using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Data;

public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
}
