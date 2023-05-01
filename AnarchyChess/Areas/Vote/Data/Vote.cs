using AnarchyChess.Areas.Identity.Data;

namespace AnarchyChess.Areas.Vote.Data
{
    public class Vote
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Proposition Proposition { get; set; }

        public bool isFor { get; set; }

        public Vote(User user, Proposition proposition)
        {
            User = user;
            Proposition = proposition;
        }

        private Vote() { }
    }
}
