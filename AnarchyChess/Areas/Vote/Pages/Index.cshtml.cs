using AnarchyChess.Areas.Vote.Data;
using AnarchyChess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnarchyChess.Areas.Vote.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PropositionService _propositionService;
        private readonly UserManager<Identity.Data.User> _userManager;

        public List<Proposition> Propositions { get; set; }
        [BindProperty]
        public string? PropositionText { get; set; }

        public IndexModel(PropositionService propositionService, UserManager<Identity.Data.User> userManager)
        {
            _propositionService = propositionService;
            _userManager = userManager;
            Propositions = new List<Proposition>();
        }

        public void OnGet()
        {
            Propositions = _propositionService.GetTopPropositions();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (PropositionText == null)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Page();
            }
            var prop = new Proposition(PropositionText, user);

            var result = _propositionService.AddProposition(prop);

            if(result == null)
            {
                return Page();
            }

            await result;

            return Redirect($"/Vote/Proposition/{prop.GUID}");
        }
    }
}
