namespace AnarchyChess.Helpers
{
    public static class ChessCoordinateHelper
    {
        public static string FileToFile(char file)
        {
            return file.ToString();
        }

        public static int RankToNumber(char rank)
        {
            return int.Parse(rank.ToString());
        }
    }
}
