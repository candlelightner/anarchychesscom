using AnarchyChess.Areas.Vote.Data;
using AnarchyChess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnarchyChess.Areas.Vote.Pages
{
    public class PropositionModel : PageModel
    {
        [FromRoute]
        public string? PropositionId { get; set; }
        public Proposition? Proposition { get; set; }
        private readonly PropositionService _propositionService;
        private readonly UserManager<Identity.Data.User> _userManager;
        public bool HasVoted { get; set; } = false;
        public bool HasVotedFor { get; set; } = false;

        public PropositionModel(PropositionService propositionService, UserManager<Identity.Data.User> userManager)
        {
            _propositionService = propositionService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (PropositionId == null)
            {
                return Redirect("/Vote/Index");
            }

            Proposition = await _propositionService.GetPropositionByGUIDAsync(PropositionId);

            if (Proposition == null)
            {
                return Redirect("/Vote/Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Vote/Index");
            }

            HasVoted = _propositionService.HasVotedInProposition(Proposition, user);
            HasVotedFor = _propositionService.HasVotedForProposition(Proposition, user);

            return Page();
        }

        public async Task<IActionResult> OnPostForAsync()
        {
            if (PropositionId == null)
            {
                return Redirect("/Vote/Index");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Vote/Index");
            }
            var result = _propositionService.VoteForPropositionWithGuid(user.UserName, PropositionId);
            if (!result)
            {
                return Redirect("/Vote/Index");
            }

            if(Proposition == null)
            {
                return Redirect("/Vote/Index");
            }

            return Redirect($"/Vote/Proposition/{Proposition.GUID}");
        }

        public async Task<IActionResult> OnPostAgainstAsync()
        {
            if (PropositionId == null)
            {
                return Redirect("/Vote/Index");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Vote/Index");
            }
            var result = _propositionService.VoteAgainstPropositionWithGuid(user.UserName, PropositionId);
            if (!result)
            {
                return Redirect("/Vote/Index");
            }

            if (Proposition == null)
            {
                return Redirect("/Vote/Index");
            }

            return Redirect($"/Vote/Proposition/{Proposition.GUID}");
        }
    }
}
