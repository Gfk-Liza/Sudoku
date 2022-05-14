
// IsOn.cs


namespace Sudoku
{
    /// <summary>
    /// 指定されたxy座標が盤面上にあるか判定するクラス
    /// </summary>
    public static class IsOn
    {
        /// <summary>
        /// 指定されたxy座標が盤面上にあるか判定する
        /// </summary>
        /// 
        /// <param name="x">
        /// x 座標
        /// </param>
        /// <param name="y">
        /// y 座標
        /// </param>
        /// 
        /// <returns>
        /// 指定されたxy座標が盤面上にあるか
        /// </returns>
        public static bool IsOnTheBoard(sbyte x, sbyte y)
        {
            if (x > 8 || x < 0 || y > 8 || y < 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// IsOnTheBoard(sbyte x, sbyte y) のオーバーロード
        /// </summary>
        /// 
        /// <param name="xy">
        /// xy 座標
        /// </param>
        /// 
        /// <returns>
        /// 指定されたxy座標が盤面上にあるか
        /// </returns>
        public static bool IsOnTheBoard(sbyte[] xy)
        {
            return IsOnTheBoard(xy[0], xy[1]);
        }
    }
}
