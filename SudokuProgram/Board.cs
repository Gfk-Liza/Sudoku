
// SudokuProgram.Board.cs


namespace Sudoku.SudokuProgram
{
    /// <summary>
    /// 盤面の状態クラス
    /// </summary>
    public class Board
    {
        /// <summary>
        /// 盤面の状態
        /// </summary>
        /// 
        /// <param name="n">
        /// 番号
        /// </param>
        /// <param name="i">
        /// 固有のものか
        /// </param>
        /// <param name="j">
        /// 答か
        /// </param>
        public Board(sbyte n, bool i, bool j)
        {
            Number = n;
            IsPeculiar = i;
            IsAnswer = j;
        }

        public sbyte Number { get; set; }
        public bool IsPeculiar { get; set; }
        public bool IsAnswer { get; set; }
    }


    /// <summary>
    /// まっさらな盤面を生成するクラス
    /// </summary>
    public static class NewBoard
    {
     
        /// <summary>
        /// まっさらな盤面を生成する
        /// </summary>
        /// 
        /// <param name="IsPecu">
        /// 固有かどうかの設定
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        public static Board[,] MakeNewBoard(bool isPecu)
        {
            Board[,] result = new Board[9, 9];

            for (sbyte i = 0; i < 9; i++) for (sbyte j = 0; j < 9; j++) result[i, j] = new Board(0, isPecu, false);

            return result.Clone() as Board[,];
        }
    }
}
