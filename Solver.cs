
// Solver.cs

using System.Collections.Generic;


namespace Sudoku
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
            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x = 0; x < 9; x++)
                {
                    if (map[y, x].Number == 0 || (map[y, x].IsAnswer && !map[y, x].IsPeculiar)) map[y, x] = new Board(0, false, true);
                    else map[y, x] = new Board(map[y, x].Number, true, false);
                }
            }

            if (!CanBeSloved.CanBeSlovedThis(map)) return map;

            Board[,] result = SolveSudoku(map).Clone() as Board[,];

            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x = 0; x < 9; x++)
                {
                    if (result[y, x].Number != 0) result[y, x].IsAnswer = true;
                }
            }

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

            sbyte nX = -1;
            sbyte nY = -1;

            bool isf = false;
            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    if (map[i, j].Number == 0)
                    {
                        nX = j;
                        nY = i;
                        isf = true;
                        break;
                    }
                }
                if (isf) break;
            }

            if (!isf) return map;


            List<sbyte> placeableNumbersList = Program.PlaceableNumbers(map, nX, nY);

            if (placeableNumbersList.Count == 0) return map;

            foreach (sbyte i in placeableNumbersList)
            {
                Board[,] nb = new Board[9, 9];
                for (sbyte y = 0; y < 9; y++)
                {
                    for (sbyte x = 0; x < 9; x++)
                    {
                        nb[y, x] = new Board(map[y, x].Number, map[y, x].IsPeculiar, true);

                        if (y == nY && x == nX) nb[nY, nX].Number = i;
                    }
                }

                // 回帰
                Board[,] retrunedBoard = SolveSudoku(nb).Clone() as Board[,];

                if (Program.IsFinConveni(retrunedBoard)) return retrunedBoard;
            }

            return map;
        }
    }
}
