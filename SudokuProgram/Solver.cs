
// SudokuProgram.Solver.cs

using System.Collections.Generic;


namespace Sudoku.SudokuProgram
{ 
    /// <summary>
    /// 数独を解くクラス
    /// </summary>
    public static class Solver
    {
        /// <summary>
        /// 数独を解く
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        public static Board[,] SolveSudokuMain(Board[,] mapArg)
        {
            Board[,] map = mapArg.Clone() as Board[,];
            sbyte y, x;
            for (y = 0; y < 9; ++y)
            {
                for (x = 0; x < 9; ++x)
                {
                    if (map[y, x].Number == 0 || (map[y, x].IsAnswer && !map[y, x].IsPeculiar)) map[y, x] = new Board(0, false, true);
                    else map[y, x] = new Board(map[y, x].Number, true, false);
                }
            }

            if (!CanBeSloved.CanBeSlovedThis(map)) return map;

            Board[,] result = SolveSudoku(map).Clone() as Board[,];

            for (y = 0; y < 9; ++y) for (x = 0; x < 9; ++x) if (result[y, x].Number != 0) result[y, x].IsAnswer = true;

            return result;
        }

        /// <summary>
        /// 数独を解く
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        private static Board[,] SolveSudoku(Board[,] mapArg)
        {
            Board[,] map = mapArg.Clone() as Board[,];

            sbyte i, j, y, x;

            for (i = 0; i < 9; ++i) for (j = 0; j < 9; ++j) if (map[i, j].Number == 0) goto Exit;

            return map;

            Exit:

            List<sbyte> placeableNumbersList = Program.PlaceableNumbers(map, j, i);

            if (placeableNumbersList.Count == 0) return map;

            foreach (sbyte p in placeableNumbersList)
            {
                Board[,] nb = new Board[9, 9];
                for (y = 0; y < 9; ++y)
                {
                    for (x = 0; x < 9; ++x)
                    {
                        nb[y, x] = new Board(map[y, x].Number, map[y, x].IsPeculiar, true);

                        if (y == i && x == j) nb[i, j].Number = p;
                    }
                }

                // 回帰
                Board[,] retrunedBoard = SolveSudoku(nb);

                if (Program.IsFinConveni(retrunedBoard)) return retrunedBoard;
            }

            return map;
        }
    }
}
