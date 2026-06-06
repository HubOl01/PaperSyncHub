using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Services;
using Microsoft.EntityFrameworkCore;
using Task = app.Models.Task;
using TaskStatus = app.Models.TaskStatus;

namespace app.Repositories;

public class TaskRepository : RepositoryBase<Task>
{
    public TaskRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Task>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Tasks
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<List<Task>> GetByStatusAsync(TaskStatus status)
    {
        return await Context.Tasks
            .Where(x => x.Statuc == status)
            .ToListAsync();
    }
}