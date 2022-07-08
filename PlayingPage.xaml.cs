
// PlayingPage.xaml.cs

using Sudoku.SudokuProgram;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace Sudoku
{
    /// <summary>
    /// PlayingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayingPage : Page
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();

        private static bool[,] isMemo = new bool[9, 9];
        private static string[,] Memo = new string[9, 9];
        private static bool[,] inPN = new bool[9, 9];


        /// <summary>
        /// コントラクタ
        /// </summary>
        public PlayingPage()
        {
            this.InitializeComponent();
        }

        public void INIT()
        {
            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x = 0; x < 9; x++)
                {
                    isMemo[y, x] = false;
                    Memo[y, x] = "";
                    inPN[y, x] = true;
                }
            }

            this.Grid_1.Focus();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(TimerMethod);
            timer.Start();

            ShowBoard(MainWindow.MainBoard);
        }


        /// <summary>
        /// 盤面を描画する
        /// </summary>
        /// 
        /// <param name="board">
        /// 盤面の状態
        /// </param>
        private void ShowBoard(Board[,] board)
        {
            TextBlock[,] labels = new TextBlock[9, 9]
            {
                { this.L00, this.L01, this.L02, this.L03, this.L04, this.L05, this.L06, this.L07, this.L08 },
                { this.L10, this.L11, this.L12, this.L13, this.L14, this.L15, this.L16, this.L17, this.L18 },
                { this.L20, this.L21, this.L22, this.L23, this.L24, this.L25, this.L26, this.L27, this.L28 },
                { this.L30, this.L31, this.L32, this.L33, this.L34, this.L35, this.L36, this.L37, this.L38 },
                { this.L40, this.L41, this.L42, this.L43, this.L44, this.L45, this.L46, this.L47, this.L48 },
                { this.L50, this.L51, this.L52, this.L53, this.L54, this.L55, this.L56, this.L57, this.L58 },
                { this.L60, this.L61, this.L62, this.L63, this.L64, this.L65, this.L66, this.L67, this.L68 },
                { this.L70, this.L71, this.L72, this.L73, this.L74, this.L75, this.L76, this.L77, this.L78 },
                { this.L80, this.L81, this.L82, this.L83, this.L84, this.L85, this.L86, this.L87, this.L88 }
            };
            
            for (sbyte y = 0; y < 9; y++)
            {
                for (sbyte x = 0; x < 9; x++)
                {
                    if (isMemo[y, x])
                    {
                        labels[y, x].FontSize = 15;
                        if (Memo[y, x].Length > 5) labels[y, x].Text = $"{Memo[y, x].Substring(0, 5)}\n{Memo[y, x].Substring(5)}";
                        else labels[y, x].Text = Memo[y, x];
                        labels[y, x].Foreground = Values.colorList[Values.ColorSetting5];
                        continue;
                    }

                    labels[y, x].FontSize = 40;
                    labels[y, x].Text = Values.converter[board[y, x].Number];

                    if (board[y, x].IsPeculiar) labels[y, x].Foreground = Values.colorList[Values.ColorSetting1];
                    else if (board[y, x].IsAnswer) labels[y, x].Foreground = Values.colorList[Values.ColorSetting3];
                    else labels[y, x].Foreground = Values.colorList[Values.ColorSetting2];

                    if (Values.CheckSetting1 && !inPN[y, x]) labels[y, x].TextDecorations = TextDecorations.Underline;
                    else labels[y, x].TextDecorations = null;
                }
            }
        }



        /// <summary>
        /// マウスが押されたときの処理
        /// </summary>
        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Grid_1.Focus();

            sbyte[] mouseXY = MouseClass.MouseXYConvert(Mouse.GetPosition(this));

            if (!IsOn.IsOnTheBoard(mouseXY) || (MainWindow.MainBoard[mouseXY[1], mouseXY[0]].IsPeculiar && MainWindow.MainBoard[mouseXY[1], mouseXY[0]].Number != 0))
            {
                this.SelectedXY.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x0, 0x0));

                Values.SelectedX = -1;
                Values.SelectedY = -1;

                return;
            }

            this.SelectedXY.DataContext = new Point { X = mouseXY[0], Y = mouseXY[1] };
            this.SelectedXY.BorderBrush = Values.colorList[Values.ColorSetting4];

            Values.SelectedX = mouseXY[0];
            Values.SelectedY = mouseXY[1];
        }

        /// <summary>
        /// キーが押されたときの処理
        /// </summary>
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsOn.IsOnTheBoard(Values.SelectedX, Values.SelectedY)) return;

            if (e.Key == Key.Subtract)
            {
                ToMemo();
                ShowBoard(MainWindow.MainBoard);
                return;
            }

            sbyte pressedKey = -1;

            for (sbyte i = 0; i < 20; i++)
            {
                if (e.Key == Values.keyesList[i])
                {
                    pressedKey = i;
                    break;
                }
            }

            if (pressedKey == -1) return;

            if (pressedKey % 10 == 0)
            {
                isMemo[Values.SelectedY, Values.SelectedX] = false;
                Memo[Values.SelectedY, Values.SelectedX] = "";
                MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number = 0;
                ShowBoard(MainWindow.MainBoard);
                return;
            }

            if (isMemo[Values.SelectedY, Values.SelectedX])
            {
                string temp = Memo[Values.SelectedY, Values.SelectedX];
                string t = (pressedKey % 10).ToString();
                string outtemp = "";
                for (sbyte i = 1; i < 10; i++)
                {
                    if (temp.Contains(i.ToString()) && t != i.ToString()) outtemp += i.ToString();
                    else if (t == i.ToString() && !temp.Contains(i.ToString())) outtemp += t;
                }

                if (outtemp.Length == 0)
                {
                    isMemo[Values.SelectedY, Values.SelectedX] = false;
                    Memo[Values.SelectedY, Values.SelectedX] = "";
                }
                else Memo[Values.SelectedY, Values.SelectedX] = outtemp;
            }
            else
            {
                List<sbyte> pn = Program.PlaceableNumbers(MainWindow.MainBoard, Values.SelectedX, Values.SelectedY);
                inPN[Values.SelectedY, Values.SelectedX] = pn.Contains((sbyte)(pressedKey % 10));
                if (pressedKey == 0) inPN[Values.SelectedY, Values.SelectedX] = true;

                MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number = (sbyte)(pressedKey % 10);
            }

            ShowBoard(MainWindow.MainBoard);

            if (Values.IsPlaying) return;
            if (Program.IsFin(MainWindow.MainBoard)) return;
            MainWindow.watch.Stop();
            this.Correct.Content = "正解!";
        }


        /// <summary>
        /// メモ化
        /// </summary>
        private void ToMemo()
        {
            isMemo[Values.SelectedY, Values.SelectedX] = true;
            if (Memo[Values.SelectedY, Values.SelectedX] != "") return;
            if (MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number == 0) return;
            Memo[Values.SelectedY, Values.SelectedX] = MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number.ToString();
            MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number = 0;
        }


        /// <summary>
        /// timer.Tick += new EventHandler(TimerMethod);
        /// からの呼び出し
        /// </summary>
        private void TimerMethod(object sender, EventArgs e)
        {
            TimeSpan result = MainWindow.watch.Elapsed;
            this.TimerLabel.Content = $"{result.Hours:00}h {result.Minutes:00}m {result.Seconds:00}s {result.Milliseconds / 10:00}";
        }
    }
}
