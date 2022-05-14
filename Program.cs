
// Program.cs

using System;
using System.Collections.Generic;


namespace Sudoku
{
    /// <summary>
    /// GUIと関係ない数独を構成するProgramのクラス
    /// 但し、生成と、解くのは、別クラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 終了したか判定する(精密)
        /// </summary>
        /// 
        /// <param name="map">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 終了したか
        /// </returns>
        public static bool IsFin(Board[,] map)
        {
            // 縦横方向 + ブロックの判定
            {
                for (sbyte i = 0; i < 9; i++)
                {
                    sbyte[] longitudinal = new sbyte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    sbyte[] lateral = new sbyte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    sbyte[] block = new sbyte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    
                    for (sbyte j = 0; j < 9; j++)
                    {
                        // 横方向
                        if (Array.IndexOf(longitudinal, map[i, j].Number) == -1) return false;
                        else longitudinal[Array.IndexOf(longitudinal, map[i, j].Number)] = 10;

                        // 縦方向
                        if (Array.IndexOf(lateral, map[j, i].Number) == -1) return false;
                        else lateral[Array.IndexOf(lateral, map[j, i].Number)] = 10;
                        
                        // ブロック
                        {
                            sbyte x = (sbyte)(3 * (i % 3) + j % 3);
                            sbyte y = (sbyte)(3 * (i / 3) + j / 3);

                            if (Array.IndexOf(block, map[y, x].Number) == -1) return false;
                            else block[Array.IndexOf(block, map[y, x].Number)] = 10;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// おける数値を検索する
        /// </summary>
        /// 
        /// <param name="map">
        /// map情報
        /// </param>
        /// <param name="x">
        /// x座標
        /// </param>
        /// <param name="y">
        /// y座標
        /// </param>
        /// 
        /// <returns>
        /// おける数値のリスト
        /// </returns>
        public static List<sbyte> PlaceableNumbers(Board[,] map, sbyte x, sbyte y)
        {
            List<sbyte> result = new List<sbyte>() { };

            bool[] possibleNumbers = new bool[10];
            for (sbyte j = 0; j < 10; j++)
            {
                possibleNumbers[j] = true;
            }
            
            for (sbyte j = 0; j < 9; j++)
            {
                possibleNumbers[map[j, x].Number] = false;
                possibleNumbers[map[y, j].Number] = false;
                possibleNumbers[map[j / 3 + 3 * (y / 3), j % 3 + 3 * (x / 3)].Number] = false;
            }

            for (sbyte i = 1; i < 10; i++)
            {
                if (possibleNumbers[i]) result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// おける数値を検索する
        /// </summary>
        /// 
        /// <param name="map">
        /// map情報
        /// </param>
        /// <param name="x">
        /// x座標
        /// </param>
        /// <param name="y">
        /// y座標
        /// </param>
        /// 
        /// <returns>
        /// おける数値のリスト
        /// </returns>
        public static List<sbyte> PlaceableNumbers_2(Board[,] map, sbyte x, sbyte y)
        {
            List<sbyte> result = new List<sbyte>() { };

            bool[] possibleNumbers = new bool[10];
            for (sbyte j = 0; j < 10; j++)
            {
                possibleNumbers[j] = true;
            }

            for (sbyte j = 0; j < 9; j++)
            {
                possibleNumbers[map[j, x].Number] = false;
                possibleNumbers[map[y, j].Number] = false;
                possibleNumbers[map[j / 3 + 3 * (y / 3), j % 3 + 3 * (x / 3)].Number] = false;
            }

            for (sbyte i = 9; i > 0; i--)
            {
                if (possibleNumbers[i]) result.Add(i);
            }

            return result;
        }


        /// <summary>
        /// 終了したか判定する(簡易的)
        /// </summary>
        /// 
        /// <param name="map">
        /// map情報
        /// </param>
        /// 
        /// <returns>
        /// 終了したか
        /// </returns>
        public static bool IsFinConveni(Board[,] map)
        {
            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    if (map[i, j].Number == 0) return false;
                }
            }

            return true;
        }
    }
}
