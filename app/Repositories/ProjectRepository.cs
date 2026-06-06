using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;

public class ProjectRepository : RepositoryBase<Project>
{
    public ProjectRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Project?> GetWithArtifactsAsync(int id)
    {
        return await Context.Projects
            .Include(x => x.Artifacts)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}