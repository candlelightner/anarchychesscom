using AnarchyChess.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnarchyChess.Models;

public class AnarchyChessContext : IdentityDbContext<User>
{

    public DbSet<Areas.Play.Data.GameParticipant> GameParticipants => Set<Areas.Play.Data.GameParticipant>();
    public DbSet<Areas.Play.Data.Game> Games => Set<Areas.Play.Data.Game>();
    public DbSet<Areas.Vote.Data.Proposition> Propositions => Set<Areas.Vote.Data.Proposition>();
    public DbSet<Areas.Vote.Data.Vote> Votes => Set<Areas.Vote.Data.Vote>();
    public AnarchyChessContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Areas.Vote.Data.Proposition>()
            .HasMany(p => p.Votes)
            .WithOne(v => v.Proposition);
        base.OnModelCreating(builder);
    }
}
