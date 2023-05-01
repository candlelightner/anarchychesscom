using AnarchyChess.Areas.Identity.Data;

namespace AnarchyChess.Areas.Vote.Data
{
    public class Proposition
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Vote> Votes { get; set; }
        public User Created { get; set; }
        public Guid GUID { get; set; }
        public int Score => Votes.Count(v => v.isFor) - Votes.Count(v => !v.isFor);

        public Proposition(string text, User created)
        {
            Text = text;
            Votes = new List<Vote>();
            GUID = Guid.NewGuid();
            Created = created;
        }

        private Proposition()
        {
            Votes = new List<Vote>();
        }
    }
}
