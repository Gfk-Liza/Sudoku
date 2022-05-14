
// MouseClass.cs

using System.Windows;


namespace Sudoku
{ 
    /// <summary>
    /// マウスの位置座標を変換するクラス
    /// </summary>
    public static class MouseClass
    {
        private const sbyte minX = 0;
        private const sbyte minY = 0;
        private const sbyte troutSize = 60;


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
            sbyte xC = (sbyte)((point.X - minX) / troutSize);
            sbyte yC = (sbyte)((point.Y - minY) / troutSize);

            if (point.X < minX) xC = -1;
            if (point.Y < minY) yC = -1;

            return new sbyte[2] { xC, yC };
        }
    }
}
