
// SudokuProgram.CanBeSloved.cs


namespace Sudoku.SudokuProgram
{
    /// <summary>
    /// 解けるか判定するクラス
    /// </summary>
    public static class CanBeSloved
    {
        /// <summary>
        /// 解くことが可能であるか判定する(簡易的)
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// マップ情報
        /// Board[9, 9]
        /// </param>
        /// 
        /// <returns>
        /// 解けるか
        /// bool
        /// </returns>
        public static bool CanBeSlovedThis(Board[,] mapArg)
        {
            Board[,] map = mapArg.Clone() as Board[,];

            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x  = 0; x < 9; x++)
                {
                    if (map[y, x].Number == 0) continue;

                    sbyte serchingLocation = map[y, x].Number;
                    map[y, x].Number = 0;

                    if (!Program.PlaceableNumbers(map, x, y).Contains(serchingLocation))
                    {
                        map[y, x].Number = serchingLocation;
                        return false;
                    }

                    map[y, x].Number = serchingLocation;
                }
            }

            return true;
        }
    }
}
