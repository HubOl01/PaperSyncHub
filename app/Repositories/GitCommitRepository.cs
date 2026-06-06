using System.Collections.Generic;
using System.Threading.Tasks;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace app.Repositories;

public class GitCommitRepository
{
    private readonly ApplicationDbContext _context;

    public GitCommitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GitCommit>> GetAllAsync()
    {
        return await _context.GitCommits.ToListAsync();
    }

    public async Task<GitCommit?> GetByHashAsync(string hash)
    {
        return await _context.GitCommits
            .FirstOrDefaultAsync(x => x.CommitHash == hash);
    }

    public async Task AddAsync(GitCommit commit)
    {
        await _context.GitCommits.AddAsync(commit);
        await _context.SaveChangesAsync();
    }
}