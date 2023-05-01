using AnarchyChess.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace AnarchyChess.Areas.Play.Data
{
    public class GameParticipant
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public GameParticipant(string userName)
        {
            UserName = userName;
        }

        public GameParticipant(User user) 
        { 
            UserName = user.UserName;
        }
    }
}
