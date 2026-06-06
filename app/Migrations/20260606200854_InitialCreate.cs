using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LocalPath = table.Column<string>(type: "TEXT", nullable: false),
                    GitCommitHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Context = table.Column<string>(type: "TEXT", nullable: false),
                    RelativePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_artifacts_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bibliography",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    CitationKey = table.Column<string>(type: "TEXT", nullable: true),
                    authors = table.Column<string>(type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    RawBibtex = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bibliography", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bibliography_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "git_commits",
                columns: table => new
                {
                    CommitHash = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_git_commits", x => x.CommitHash);
                    table.ForeignKey(
                        name: "FK_git_commits_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tasks_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "artifact_dependencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceArtifactId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetArtifactId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelationType = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artifact_dependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_artifact_dependencies_artifacts_SourceArtifactId",
                        column: x => x.SourceArtifactId,
                        principalTable: "artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_artifact_dependencies_artifacts_TargetArtifactId",
                        column: x => x.TargetArtifactId,
                        principalTable: "artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "execution_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArtifactId = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMs = table.Column<int>(type: "INTEGER", nullable: false),
                    ExitCode = table.Column<int>(type: "INTEGER", nullable: false),
                    Output = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execution_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_execution_logs_artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artifact_dependencies_SourceArtifactId",
                table: "artifact_dependencies",
                column: "SourceArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_artifact_dependencies_TargetArtifactId",
                table: "artifact_dependencies",
                column: "TargetArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_artifacts_ProjectId",
                table: "artifacts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_bibliography_ProjectId",
                table: "bibliography",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_execution_logs_ArtifactId",
                table: "execution_logs",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_git_commits_ProjectId",
                table: "git_commits",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ProjectId",
                table: "tasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artifact_dependencies");

            migrationBuilder.DropTable(
                name: "bibliography");

            migrationBuilder.DropTable(
                name: "execution_logs");

            migrationBuilder.DropTable(
                name: "git_commits");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "artifacts");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
