using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;

public class ArtifactDependencyRepository
    : RepositoryBase<ArtifactDependency>
{
    public ArtifactDependencyRepository(
        ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<ArtifactDependency>>
        GetDependenciesAsync(int artifactId)
    {
        return await Context.ArtifactDependencies
            .Where(x => x.SourceArtifactId == artifactId)
            .ToListAsync();
    }
}