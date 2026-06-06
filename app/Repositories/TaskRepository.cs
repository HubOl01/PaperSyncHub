using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;
using TaskStatus = app.Models.TaskStatus;

namespace app.Repositories;

public class TaskRepository : RepositoryBase<TaskItem>
{
    public TaskRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<TaskItem>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Tasks
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetByStatusAsync(TaskStatus status)
    {
        return await Context.Tasks
            .Where(x => x.Status == status)
            .ToListAsync();
    }
}