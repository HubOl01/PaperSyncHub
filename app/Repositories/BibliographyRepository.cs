using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;

namespace app.Repositories;

public class BibliographyRepository : RepositoryBase<Bibliography>
{
    public BibliographyRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Bibliography>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Bibliography
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }
}