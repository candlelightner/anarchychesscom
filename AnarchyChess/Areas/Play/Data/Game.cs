namespace AnarchyChess.Areas.Play.Data
{
    public class Game
    {
        public int Id { get; set; }
        public GameParticipant? WhitePlayer { get; set; }
        public GameParticipant? BlackPlayer { get; set; }
        public List<ChessCell> GameState { get; set; }
        public string? GUID { get; set; }
        public bool WhiteToMove { get; set; }
        public bool gameOver { get; set; }

        public Game(GameParticipant WhitePlayer, GameParticipant BlackPlayer, bool WhiteToMove)
        {
            this.WhitePlayer = WhitePlayer;
            this.BlackPlayer = BlackPlayer;
            this.GUID = Guid.NewGuid().ToString();
            this.WhiteToMove = WhiteToMove;
            this.GameState = new ();
            populateGame();
        }

        public Game() { this.GUID = Guid.NewGuid().ToString(); }

        private void populateGame()
        {
            var gameState = new List<ChessCell>();

            // Place the pawns
            foreach (var cha in new List<string> { "a", "b", "c", "d", "e", "f", "g", "h" })
            {
                gameState.Add(new ChessCell { Rank = 2, File = cha, Value = ChessPiece.White_Pawn, Game = this });
                gameState.Add(new ChessCell { Rank = 7, File = cha, Value = ChessPiece.Black_Pawn, Game = this });
            }

            // Place the rooks
            gameState.Add(new ChessCell { Rank = 1, File = "a", Value = ChessPiece.White_Rook, Game = this });
            gameState.Add(new ChessCell { Rank = 1, File = "h", Value = ChessPiece.White_Rook, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "a", Value = ChessPiece.Black_Rook, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "h", Value = ChessPiece.Black_Rook, Game = this });

            // Place the knights
            gameState.Add(new ChessCell { Rank = 1, File = "b", Value = ChessPiece.White_Knight, Game = this });
            gameState.Add(new ChessCell { Rank = 1, File = "g", Value = ChessPiece.White_Knight, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "b", Value = ChessPiece.Black_Knight, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "g", Value = ChessPiece.Black_Knight, Game = this });

            // Place the bishops
            gameState.Add(new ChessCell { Rank = 1, File = "c", Value = ChessPiece.White_Bishop, Game = this });
            gameState.Add(new ChessCell { Rank = 1, File = "f", Value = ChessPiece.White_Bishop, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "c", Value = ChessPiece.Black_Bishop, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "f", Value = ChessPiece.Black_Bishop, Game = this });

            // Place the queens
            gameState.Add(new ChessCell { Rank = 1, File = "d", Value = ChessPiece.White_Queen, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "d", Value = ChessPiece.Black_Queen, Game = this });

            // Place the kings
            gameState.Add(new ChessCell { Rank = 1, File = "e", Value = ChessPiece.White_King, Game = this });
            gameState.Add(new ChessCell { Rank = 8, File = "e", Value = ChessPiece.Black_King, Game = this });

            GameState = gameState;
        }
    }
}
