using AnarchyChess.Areas.Play.Data;
using AnarchyChess.Models;

namespace AnarchyChess.Services
{
    public class UserSyncService
    {
        public AnarchyChessContext _idContext;

        public UserSyncService(AnarchyChessContext idContext)
        {
            _idContext = idContext;
        }
    }
}
