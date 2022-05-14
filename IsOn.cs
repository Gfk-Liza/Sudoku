
// IsOn.cs


namespace Sudoku
{
    internal class IsOn
    {
        ///
        /// <summary>
        /// 指定されたxy座標が盤面上にあるか判定するクラス
        /// </summary>
        /// 

        public static bool IsOnTheBoard(sbyte x, sbyte y)
        {
            ///
            /// <summary>
            /// 指定されたxy座標が盤面上にあるか判定する
            /// </summary>
            /// 
            /// <param name="x">
            /// x 座標
            /// sbyte
            /// </param>
            /// <param name="y">
            /// y 座標
            /// sbyte
            /// </param>
            /// 
            /// <return>
            /// 指定されたxy座標が盤面上にあるか
            /// bool
            /// </return>
            /// 

            if (x > 8 || x < 0 || y > 8 || y < 0)
            {
                return false;
            }
            return true;
        }


        public static bool IsOnTheBoard(sbyte[] xy)
        {
            ///
            /// <summary>
            /// IsOnTheBoard(sbyte x, sbyte y) のオーバーロード
            /// </summary>
            /// 
            /// <param name="xy">
            /// xy 座標
            /// sbyte[2]
            /// </param>
            /// 
            /// <return>
            /// 指定されたxy座標が盤面上にあるか
            /// bool
            /// </return>
            /// 

            return IsOnTheBoard(xy[0], xy[1]);
        }
    }
}
