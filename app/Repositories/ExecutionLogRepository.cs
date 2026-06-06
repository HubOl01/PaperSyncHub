using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;

public class ExecutionLogRepository
    : RepositoryBase<ExecutionLog>
{
    public ExecutionLogRepository(
        ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<ExecutionLog>>
        GetByArtifactIdAsync(int artifactId)
    {
        return await Context.ExecutionLogs
            .Where(x => x.ArtifactId == artifactId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}