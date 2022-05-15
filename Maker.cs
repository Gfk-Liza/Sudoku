
// Maker.cs

using System;
using System.Collections.Generic;
using System.Linq;


namespace Sudoku
{
    /// <summary>
    /// 数独を生成するクラス
    /// </summary>
    public class Maker : Program
    {
        private static bool[,] canPlace = new bool[9, 9];


        /// <summary>
        /// 数独を生成する
        /// </summary>
        /// 
        /// <param name="i">
        /// あける穴の数
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        public static Board[,] MakeSudokuMain(sbyte i)
        {
            Board[,] result = NewBoard.MakeNewBoard(true);
            ResetCanPlaceList();

            Random random = new Random();
            result[0, 0].Number = (sbyte)random.Next(1, 9);

            result = MakeSudoku(result);
            result = MakeHoles(result, i);

            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x = 0; x < 9; x++)
                {
                    if (result[y, x].Number == 0) result[y, x].IsPeculiar = false;
                }
            }

            return result.Clone() as Board[,];
        }


        /// <summary>
        /// 数独のもう一つの解を見つける
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// <param name="ans">
        /// 確定している解
        /// </param>
        /// 
        /// <returns>
        /// 二つ目の解があったか
        /// [解であるか, ansと同じであるか]
        /// </returns>
        private static bool[] SolveSudokuOther(Board[,] mapArg, Board[,] ans)
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

            bool isSame = true;
            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    if (ans[i, j].Number != map[i, j].Number)
                    {
                        isSame = false;
                        break;
                    }
                }
                if (!isSame) break;
            }

            if (!isf) return new bool[2] { true, isSame };

            List<sbyte> placeableNumbersList = PlaceableNumbers_2(map, nX, nY);

            if (placeableNumbersList.Count == 0) return new bool[2] { false, false };

            foreach (sbyte i in placeableNumbersList)
            {
                Board[,] nb = new Board[9, 9];
                for (sbyte y = 0; y < 9; y++)
                {
                    for (sbyte x = 0; x < 9; x++)
                    {
                        nb[y, x] = new Board(map[y, x].Number, map[y, x].IsPeculiar, false);

                        if (y == nY && x == nX) nb[nY, nX].Number = i;
                    }
                }

                // 回帰
                bool[] isOK = SolveSudokuOther(nb, ans).Clone() as bool[];

                if (isOK[0]) return new bool[2] { true, isOK[1] };
            }

            return new bool[2] { false, false };
        }


        /// <summary>
        /// 数独を生成する
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        private static Board[,] MakeSudoku(Board[,] mapArg)
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

            List<sbyte> placeableNumbersList = PlaceableNumbers(map, nX, nY);

            if (placeableNumbersList.Count == 0) return map;

            // Random
            Random randoms = new Random();
            placeableNumbersList = placeableNumbersList.OrderBy(a => randoms.Next(placeableNumbersList.Count)).ToList();

            foreach (sbyte i in placeableNumbersList)
            {
                Board[,] nb = new Board[9, 9];
                for (sbyte y = 0; y < 9; y++)
                {
                    for (sbyte x = 0; x < 9; x++)
                    {
                        nb[y, x] = new Board(map[y, x].Number, true, false);

                        if (y == nY && x == nX) nb[nY, nX].Number = i;
                    }
                }

                // 回帰
                Board[,] returnedBoard = MakeSudoku(nb);
                if (IsFinConveni(returnedBoard)) return returnedBoard;
            }


            return map;
        }


        /// <summary>
        /// 穴をあける
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// <param name="holesNumber">
        /// あける穴の数
        /// </param>
        /// 
        /// <returns>
        /// 盤面の状態
        /// </returns>
        private static Board[,] MakeHoles(Board[,] mapArg, sbyte holesNumber)
        {
            Board[,] map = mapArg.Clone() as Board[,];

            for (sbyte index = 0; index <= holesNumber; index++)
            {
                sbyte[] possibleLocations = PossibleLocationsForMakingAHole(map).Clone() as sbyte[];

                if (possibleLocations[0] == -1) break;
                map[possibleLocations[1], possibleLocations[0]].Number = 0;
            }

            return map;
        }


        /// <summary>
        /// 穴をあけることができる場所を検索する
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 穴をあけることができる場所
        /// [x, y]
        /// もし穴をあけることができる場所がないならば[-1, -1]
        /// </returns>
        private static sbyte[] PossibleLocationsForMakingAHole(Board[,] mapArg)
        {
            sbyte[] Y = (new sbyte[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }).OrderBy(i => Guid.NewGuid()).ToArray();
            sbyte[] X = (new sbyte[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }).OrderBy(i => Guid.NewGuid()).ToArray();
            foreach (sbyte y in Y)
            {
                foreach (sbyte x in X)
                {
                    if (!canPlace[y, x]) continue;

                    Board[,] map = new Board[9, 9];

                    for (sbyte ty = 0; ty < 9; ty++)
                    {
                        for (sbyte tx = 0; tx < 9; tx++)
                        {
                            map[ty, tx] = new Board(mapArg[ty, tx].Number, mapArg[ty, tx].IsPeculiar, false);
                        }
                    }

                    map[y, x] = new Board(0, false, false);

                    if (!DuplicateAnswers(map))
                    {
                        canPlace[y, x] = false;
                        return new sbyte[2] { x, y };
                    }
                    else canPlace[y, x] = false;
                }
            }

            return new sbyte[2] { -1, -1 };
        }


        /// <summary>
        /// 重複解が存在するか
        /// </summary>
        /// 
        /// <param name="mapArg">
        /// 盤面の状態
        /// </param>
        /// 
        /// <returns>
        /// 重複解が存在するか
        /// </returns>
        public static bool DuplicateAnswers(Board[,] mapArg)
        {
            Board[,] map = mapArg.Clone() as Board[,];
            Board[,] ans = Solver.SolveSudokuMain(map);
            bool[] result = SolveSudokuOther(map, ans);

            return !result[1] && result[0];
        }


        /// <summary>
        /// canPlaceをリセットする
        /// </summary>
        private static void ResetCanPlaceList()
        {
            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    canPlace[i, j] = true;
                }
            }
        }
    }
}
