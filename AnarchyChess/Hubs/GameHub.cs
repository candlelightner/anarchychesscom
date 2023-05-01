using Microsoft.AspNetCore.SignalR;
using AnarchyChess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AnarchyChess.Helpers;

namespace AnarchyChess.Hubs
{
    public class GameHub : Hub
    {
        private readonly AnarchyChessContext _context;
        public GameHub(AnarchyChessContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task MovePiece(string fromPosition, string toPosition, string guid)
        {
            var user = Context?.User?.Identity;

            var game = _context.Games
                .Include(g => g.WhitePlayer)
                .Include(g => g.BlackPlayer)
                .Include(g=> g.GameState)
                .FirstOrDefault(g => g.GUID == guid);

            if (game == null || user == null)
            {
                return;
            }

            var username = user.Name;

            bool isWhite = game?.WhitePlayer?.UserName == username;

            if (game?.WhitePlayer?.UserName != username && game?.BlackPlayer?.UserName != username)
            {
                return;
            }

            var fromPositionFile = ChessCoordinateHelper.FileToFile(fromPosition[0]);
            var fromPositionRank = ChessCoordinateHelper.RankToNumber(fromPosition[1]);

            var toPositionFile = ChessCoordinateHelper.FileToFile(toPosition[0]);
            var toPositionRank = ChessCoordinateHelper.RankToNumber(toPosition[1]);

            var pieceToRemove = game?.GameState.FirstOrDefault(p => p.File == toPositionFile && p.Rank == toPositionRank);
            if(pieceToRemove != null)
            {
                game?.GameState.Remove(pieceToRemove);
            }
            var piece = game?.GameState.FirstOrDefault(p => p.File == fromPositionFile && p.Rank == fromPositionRank);

            if (piece == null)
            {
                return;
            }

            if((int)(piece.Value) <= 6 && !isWhite)
            {
                return;
            }
            if((int)(piece.Value) > 6 && isWhite)
            {
                return;
            }

            piece.File = toPositionFile;
            piece.Rank = toPositionRank;

            await _context.SaveChangesAsync();

            await Clients.OthersInGroup(guid).SendAsync("PieceMoved", fromPosition, toPosition);
        }

        public async Task JoinGroup(string guid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, guid);
        }

        public async Task LeaveGroup(string guid)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, guid);
        }
    }
}
