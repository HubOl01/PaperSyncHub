using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;

public class ArtifactRepository : RepositoryBase<Artifact>
{
    public ArtifactRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Artifact>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Artifacts
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<List<Artifact>> GetByTypeAsync(ArtifactType type)
    {
        return await Context.Artifacts
            .Where(x => x.Type == type)
            .ToListAsync();
    }
}