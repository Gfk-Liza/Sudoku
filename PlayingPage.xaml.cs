
// PlayingPage.xaml.cs

using System;
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


        /// <summary>
        /// コントラクタ
        /// </summary>
        public PlayingPage(Board[,] _board)
        {
            InitializeComponent();

            this.Grid_1.Focus();

            ShowBoard(_board);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(TimerMethod);
            timer.Start();
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
            this.PeculiarText.Content = "";
            this.NonSpecificText.Content = "";
            this.AnswerText.Content = "";

            this.PeculiarText.Foreground = MainWindow.colorList[MainWindow.colorSetting1];
            this.NonSpecificText.Foreground = MainWindow.colorList[MainWindow.colorSetting2];
            this.AnswerText.Foreground = MainWindow.colorList[MainWindow.colorSetting3];

            string[] converter = new string[10] { " ", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    if (board[i, j].IsPeculiar)
                    {
                        this.PeculiarText.Content += converter[board[i, j].Number];
                        this.NonSpecificText.Content += " ";
                        this.AnswerText.Content += " ";
                    }
                    else if (board[i, j].IsAnswer)
                    {
                        this.PeculiarText.Content += " ";
                        this.NonSpecificText.Content += " ";
                        this.AnswerText.Content += converter[board[i, j].Number];
                    }
                    else
                    {
                        this.PeculiarText.Content += " ";
                        this.NonSpecificText.Content += converter[board[i, j].Number];
                        this.AnswerText.Content += " ";
                    }

                    this.PeculiarText.Content += " ";
                    this.NonSpecificText.Content += " ";
                    this.AnswerText.Content += " ";
                }

                this.PeculiarText.Content += "\n";
                this.NonSpecificText.Content += "\n";
                this.AnswerText.Content += "\n";
            }
        }



        /// <summary>
        /// マウスが押されたときの処理
        /// </summary>
        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sbyte[] mouseXY = MouseClass.MouseXYConvert(Mouse.GetPosition(this));

            if (!IsOn.IsOnTheBoard(mouseXY) || (MainWindow.mainBoard[mouseXY[1], mouseXY[0]].IsPeculiar && MainWindow.mainBoard[mouseXY[1], mouseXY[0]].Number != 0))
            {
                this.SelectedXY.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x0, 0x0));

                MainWindow.SelectedX = -1;
                MainWindow.SelectedY = -1;

                return;
            }

            this.SelectedXY.DataContext = new Point { X = mouseXY[0], Y = mouseXY[1] };
            this.SelectedXY.BorderBrush = MainWindow.colorList[MainWindow.colorSetting4];

            MainWindow.SelectedX = mouseXY[0];
            MainWindow.SelectedY = mouseXY[1];
        }

        /// <summary>
        /// キーが押されたときの処理
        /// </summary>
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsOn.IsOnTheBoard(MainWindow.SelectedX, MainWindow.SelectedY)) return;

            Key[] keyesList = new Key[20] {
                Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
                Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9
            };
            sbyte pressedKey = 0;

            for (sbyte i = 0; i < 20; i++)
            {
                if (e.Key == keyesList[i])
                {
                    pressedKey = i;
                    break;
                }
            }

            MainWindow.mainBoard[MainWindow.SelectedY, MainWindow.SelectedX].Number = (sbyte)(pressedKey % 10);

            ShowBoard(MainWindow.mainBoard);

            if (MainWindow.isPlaying)
            {
                if (Program.IsFin(MainWindow.mainBoard))
                {
                    MainWindow.watch.Stop();
                    this.Correct.Content = "正解!";
                }
            }
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
