using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using app.Models;
using app.Repositories;

namespace app.Services;

public class GitService
{
    private readonly GitCommitRepository _commitRepo;
    private readonly ArtifactRepository _artifactRepo;

    public GitService(GitCommitRepository commitRepo, ArtifactRepository artifactRepo)
    {
        _commitRepo = commitRepo;
        _artifactRepo = artifactRepo;
    }

    public async Task<GitCommit> CreateCommitAsync(int projectId, string message, string authorName)
    {
        // Снимаем снапшот артефактов проекта
        var artifacts = await _artifactRepo.GetByProjectIdAsync(projectId);
        var snapshot = JsonSerializer.Serialize(artifacts.Select(a => new
        {
            a.Id, a.Title, a.Context, a.RelativePath, a.Type
        }));

        var hash = ComputeHash(snapshot + DateTime.UtcNow.Ticks);

        var commit = new GitCommit
        {
            CommitHash = hash,
            ProjectId = projectId,
            Message = message,
            AuthorName = authorName,
            CreatedAt = DateTime.UtcNow
        };

        await _commitRepo.AddAsync(commit);
        return commit;
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower()[..8];
    }
}