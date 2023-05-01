using AnarchyChess.Areas.Identity.Data;
using AnarchyChess.Areas.Vote.Data;
using AnarchyChess.Models;
using Microsoft.EntityFrameworkCore;

namespace AnarchyChess.Services
{
    public class PropositionService
    {
        private readonly AnarchyChessContext _context;
        public PropositionService(AnarchyChessContext context)
        {
            _context = context;
        }

        public bool VoteForPropositionWithGuid(string username, string propositionId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username); 
            var proposition = GetPropositionByGUID(propositionId);


            if (user == null || proposition == null)
            {
                return false;
            }

            if(HasVotedInProposition(proposition, user))
            {
                return false;
            }

            VoteForProposition(proposition, user);

            return true;
        }

        public bool VoteAgainstPropositionWithGuid(string username, string propositionId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var proposition = GetPropositionByGUID(propositionId);

            if (user == null || proposition == null)
            {
                return false;
            }

            if (HasVotedInProposition(proposition,user))
            {
                return false;
            }

            VoteAgainstProposition(proposition, user);

            return true;
        }

        public List<Proposition> GetTopPropositions()
        {
            var propositions = _context.Propositions
                .Include(p => p.Created)
                .Include(p => p.Votes)
                .ToList()
                .OrderByDescending(p => p.Score)
                .Take(10)
                .ToList();
            return propositions;
        }

        public Task AddProposition(Proposition proposition)
        {
            if(proposition == null)
            {
                return null;
            }

            _context.Propositions.Add(proposition);
            return _context.SaveChangesAsync();
        }

        public Task<Proposition?> GetPropositionByGUIDAsync(string guid)
        {
            var proposition = _context.Propositions
                                .Include(p => p.Created)
                                .Include(p => p.Votes)
                                .ThenInclude(v => v.User)
                                .FirstOrDefaultAsync(p => p.GUID.ToString().ToLower() == guid.ToLower());
            return proposition;
        }

        public Proposition? GetPropositionByGUID(string guid)
        {
            var proposition = _context.Propositions
                                .Include(p => p.Created)
                                .Include(p => p.Votes)
                                .FirstOrDefault(p => p.GUID.ToString().ToLower() == guid.ToLower());
            return proposition;
        }

        public void VoteForProposition(Proposition prop, User user)
        {
            var vot = new Vote(user, prop);
            vot.isFor = true;
            _context.Votes.Add(vot);
            prop.Votes.Add(vot);
            _context.SaveChanges();
        }

        public void VoteAgainstProposition(Proposition prop, User user)
        {
            var vot = new Vote(user, prop);
            vot.isFor = false;
            _context.Votes.Add(vot);
            prop.Votes.Add(vot);
            _context.SaveChanges();
        }

        public int GetVotesForProposition(Proposition prop)
        {
            return prop.Score;
        }

        public bool HasVotedInProposition(Proposition prop, User user)
        {
            return HasVotedForProposition(prop, user) || HasVotedAgainstProposition(prop, user);
        }

        public bool HasVotedForProposition(Proposition prop, User user)
        {
            return prop.Votes.Any(v => v.User == user && v.isFor);
        }

        public bool HasVotedAgainstProposition(Proposition prop, User user)
        {
            return prop.Votes.Any(v => v.User == user && !v.isFor);
        }
    }
}
