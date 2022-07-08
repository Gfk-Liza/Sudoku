
// MouseProgram.MouseClass.cs

using System.Windows;


namespace Sudoku.MouseProgram
{ 
    /// <summary>
    /// マウスの位置座標を変換するクラス
    /// </summary>
    public static class MouseClass
    {
        private const sbyte MIN_X = 0;
        private const sbyte MIN_Y = 0;
        private const sbyte TROUT_SIZE = 60;


        /// <summary>
        /// マウスの位置座標を変換する
        /// </summary>
        /// 
        /// <param name="point">
        /// マウスの状態
        /// </param>
        /// 
        /// <returns>
        /// XY座標
        /// </returns>
        public static sbyte[] MouseXYConvert(Point point)
        {
            sbyte xC = (sbyte)((point.X - MIN_X) / TROUT_SIZE);
            sbyte yC = (sbyte)((point.Y - MIN_Y) / TROUT_SIZE);

            if (point.X < MIN_X) xC = -1;
            if (point.Y < MIN_Y) yC = -1;

            return new sbyte[2] { xC, yC };
        }
    }
}
