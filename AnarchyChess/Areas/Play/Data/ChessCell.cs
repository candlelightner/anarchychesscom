namespace AnarchyChess.Areas.Play.Data
{
    public class ChessCell
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string File { get; set; } = "";
        public ChessPiece Value { get; set; }
        public Game? Game { get; set; }
    }

}
