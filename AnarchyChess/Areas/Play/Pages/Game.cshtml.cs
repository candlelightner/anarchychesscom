using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnarchyChess.Areas.Identity.Data;
using AnarchyChess.Models;
using AnarchyChess.Areas.Play.Data;
using Microsoft.EntityFrameworkCore;
using AnarchyChess.Helpers;
using AnarchyChess.Services;

namespace AnarchyChess.Areas.Play
{
    public class GameModel : PageModel
    {
        [FromRoute]
        public string GameId { get; set; }
        public List<string> GameState { get; set; }
        public bool IsWhite { get; set; }
        private readonly UserManager<Identity.Data.User> _userManager;
        private readonly GameService _matchingService;
        public GameModel(UserManager<Identity.Data.User> userManager, GameService matchingService)
        {
            _userManager = userManager;
            _matchingService = matchingService;
            GameState = new List<string>();
        }
        public IActionResult OnGet()
        {
            GameState = new List<string>();

            var username = _userManager.GetUserName(User);
            if (GameId == null)
            {
                
                var gametmp = _matchingService.GetGameForUser(username);

                if(!_matchingService.UserExists(username))
                {
                    return RedirectToPage("/Index");
                }

                if(gametmp != null)
                {
                    GameId = gametmp.GUID!;
                }
                else
                {
                    gametmp = _matchingService.JoinGameOrCreate(username);
                    GameId = gametmp.GUID;
                }

                return Redirect($"/Play/Game/{GameId}");
            }

            var game = _matchingService.GetGameByID(GameId);

            if(game == null)
            {
                return RedirectToPage("/Index");
            }

            foreach(var state in game.GameState)
            {
                GameState.Add($"{state.File}{state.Rank}|{state.Value}");
            }

            IsWhite = game.WhitePlayer?.UserName == username;

            return Page();
        }
    }
}
