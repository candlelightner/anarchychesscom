using AnarchyChess.Areas.Play.Data;
using AnarchyChess.Models;
using Microsoft.EntityFrameworkCore;

namespace AnarchyChess.Services
{
    public class GameService
    {
        private readonly AnarchyChessContext _context;
        public GameService(AnarchyChessContext context)
        {
            _context = context;
        }

        public Game? GetGameForUser(string username)
        {
            var game = _context.Games.FirstOrDefault(g => (g.WhitePlayer.UserName == username || g.BlackPlayer.UserName == username) && !g.gameOver);
            return game;
        }

        public Game? GetGameByID(string id)
        {
            var game = _context.Games
                .Include(g => g.WhitePlayer)
                .Include(g => g.BlackPlayer)
                .Include(g => g.GameState)
                .FirstOrDefault(g => g.GUID == id);
            return game;
        }

        public Game? CreateGame(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            GameParticipant participant = new(user);

            _context.GameParticipants.Add(participant);

            var game = new Game(participant, null, true);
            _context.Games.Add(game);
            _context.SaveChanges();
            return game;
        }

        public Game? JoinGame(string username)
        {
            var game = _context.Games.FirstOrDefault(g => (g.WhitePlayer.UserName != null || g.BlackPlayer == null) && !g.gameOver);
            if (game != null)
            {
                game.BlackPlayer = new GameParticipant(_context.Users.FirstOrDefault(u => u.UserName == username));
                _context.SaveChanges();
            }
            return game;
        }

        public Game? JoinGameOrCreate(string username)
        {
            var game = JoinGame(username);
            if (game == null)
            {
                game = CreateGame(username);
            }
            return game;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }
    }
}
