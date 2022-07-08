
// AnalysisPage.xaml.cs

using Sudoku.MouseProgram;
using Sudoku.SudokuProgram;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Sudoku
{
    /// <summary>
    /// AnalysisPage.xaml の相互作用ロジック
    /// </summary>
    public partial class AnalysisPage : Page
    {
        /// <summary>
        /// コントラクタ
        /// </summary>
        public AnalysisPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 盤面を描画する
        /// </summary>
        /// 
        /// <param name="board">
        /// 盤面の状態
        /// </param>
        public void ShowBoard(Board[,] board)
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
                    labels[y, x].Text = Values.converter[board[y, x].Number];
                    labels[y, x].FontSize = 40;

                    if (board[y, x].IsPeculiar) labels[y, x].Foreground = Values.colorList[Values.ColorSetting1];
                    else if (board[y, x].IsAnswer) labels[y, x].Foreground = Values.colorList[Values.ColorSetting3];
                    else labels[y, x].Foreground = Values.colorList[Values.ColorSetting2];
                }
            }
        }


        /// <summary>
        /// マウスが押されたときの処理
        /// </summary>
        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sbyte[] mouseXY = MouseClass.MouseXYConvert(Mouse.GetPosition(this));

            if (!IsOn.IsOnTheBoard(mouseXY))
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

            sbyte pressedKey = 0;

            for (sbyte i = 0; i < 20; i++)
            {
                if (e.Key == Values.keyesList[i])
                {
                    pressedKey = i;
                    break;
                }
            }

            MainWindow.MainBoard[Values.SelectedY, Values.SelectedX].Number = (sbyte)(pressedKey % 10);

            ShowBoard(MainWindow.MainBoard);
        }

        /// <summary>
        /// 解析ボタンが押されたときの処理
        /// </summary>
        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            Board[,] subBoard = Solver.SolveSudokuMain(MainWindow.MainBoard);
            ShowBoard(subBoard);

            this.CanSolveLabel.Content = "";
            if (!Program.IsFin(subBoard)) this.CanSolveLabel.Content = "解無し";
            else if (Maker.DuplicateAnswers(MainWindow.MainBoard)) this.CanSolveLabel.Content = "重複解が存在";

            MainWindow.MainBoard = subBoard;
        }
    }
}

