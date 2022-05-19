
// MainWindow.xaml.cs

using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;


namespace Sudoku
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// MainWindowを生成する
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            NavigatePlayingPage();
            this.ClearAllButton.IsEnabled = false;
            this.ClearButton.IsEnabled = false;

            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }


        // play中か
        public static bool isPlaying = false;

        // 難易度
        public static sbyte difficulty = -1;
        // 選択されている X 座標
        public static sbyte SelectedX = -1;
        // 選択されている Y 座標
        public static sbyte SelectedY = -1;

        // 元の数字の色の設定
        public static sbyte colorSetting1 = 0;
        // 記入する数字の色の設定
        public static sbyte colorSetting2 = 4;
        // 回答の数字の色の設定
        public static sbyte colorSetting3 = 3;
        // 選択枠の色の設定
        public static sbyte colorSetting4 = 5;
        // メモの数字の色の設定
        public static sbyte colorSetting5 = 1;

        // 入力時にルールに違反していたら、下線を引く
        public static bool checkSetting1 = false;

        // 色
        public static readonly SolidColorBrush[] colorList = new SolidColorBrush[10] {
            Brushes.Black, Brushes.Gray, Brushes.Pink, Brushes.Red, Brushes.Orange,
            Brushes.Yellow, Brushes.YellowGreen, Brushes.Green, Brushes.Blue, Brushes.SkyBlue
        };

        // MainBoard
        public static Board[,] mainBoard = NewBoard.MakeNewBoard(false);

        // あける穴の数（難易度別）
        public static readonly sbyte[] difficultyList = new sbyte[8] { 20, 27, 35, 42, 50, 54, 58, 81 };

        // Main timer
        public static readonly Stopwatch watch = new Stopwatch();

        // カウントダウン復帰用のタイマー
        private readonly DispatcherTimer timer = new DispatcherTimer();

        // カウントダウン復帰用のタイマー
        private readonly DispatcherTimer timer2 = new DispatcherTimer();


        // レベル選択画面のボタンの更新速度(ミリ秒)
        private const sbyte UPDATE_RATE = 50;

        // 画面遷移時の速度(ミリ秒)
        public const int ANIMATION_TIME_SPAN = 200;



        /// <summary>
        /// ゲームを始める
        /// タイムアタックボタンが押されたときの処理
        /// </summary>
        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            difficulty = -1;
            this.MenuToggleButton.IsChecked = false;
            this.frame.Navigate(new ChooseDifficultyPage(), this);

            timer2.Interval = new TimeSpan(0, 0, 0, 0, UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod1);
            timer2.Start();
        }

        /// <summary>
        /// 0.05秒ごとに呼ばれるメソッド
        /// </summary>
        private void TimerMethod1(object sender, EventArgs e)
        {
            if (difficulty == -1) return;

            timer2.Stop();

            MakeNewSudoku();
            
            this.frame.Navigate(new CountDownPage(), this);
            timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            timer.Tick += new EventHandler(TimerMethod2);
            timer.Start();
            
            difficulty = -1;
        }

        /// <summary>
        /// 3秒後に呼ばれるメソッド
        /// </summary>
        private void TimerMethod2(object sender, EventArgs e)
        {
            timer.Stop();
            isPlaying = true;

            watch.Restart();
            NavigatePlayingPage();
        }


        /// <summary>
        /// 迷路を生成する関数
        /// </summary>
        private void MakeNewSudoku()
        {
            Thread thread = new Thread(new ThreadStart(() => { mainBoard = Maker.MakeSudokuMain(difficultyList[difficulty]); }));
            thread.Start();
            thread.Join();
        }


        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        private void FinButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 設定ページを開く
        /// </summary>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.MenuToggleButton.IsChecked = false;
            this.frame.Navigate(new SettingsPage(), this);

            this.ClearAllButton.IsEnabled = false;
            this.ClearButton.IsEnabled = false;
        }

        /// <summary>
        /// 新規生成する
        /// </summary>
        private void NewGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            difficulty = -1;
            this.MenuToggleButton.IsChecked = false;
            this.frame.Navigate(new ChooseDifficultyPage(), this);

            timer2.Interval = new TimeSpan(0, 0, 0, 0, UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod3);
            timer2.Start();
        }

        /// <summary>
        /// 0.05秒ごとに呼ばれるメソッド
        /// </summary>
        private void TimerMethod3(object sender, EventArgs e)
        {
            if (difficulty == -1) return;

            timer2.Stop();
            MakeNewSudoku();
            watch.Restart();
            NavigatePlayingPage();
        }


        /// <summary>
        /// 解析モードにする
        /// </summary>
        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            isPlaying = false;

            this.MenuToggleButton.IsChecked = false;

            this.frame.Navigate(new AnalysisPage(), this);

            this.ClearAllButton.IsEnabled = true;
            this.ClearButton.IsEnabled = true;
        }


        /// <summary>
        /// 答えを表示する
        /// </summary>
        private void SolutionButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            isPlaying = false;

            mainBoard = Solver.SolveSudokuMain(mainBoard);

            this.MenuToggleButton.IsChecked = false;

            NavigatePlayingPage();
        }

        /// <summary>
        /// まっさらにする
        /// </summary>
        private void BeBrandNewButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            mainBoard = NewBoard.MakeNewBoard(false);
            if (isPlaying)
            {
                isPlaying = false;
                NavigatePlayingPage();
                return;
            }
            this.frame.Navigate(new AnalysisPage(), this);
        }

        /// <summary>
        /// 記入した数字以外を消す
        /// </summary>
        private void EraseAllAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            for (sbyte i = 0; i < 9; i++)
            {
                for (sbyte j = 0; j < 9; j++)
                {
                    if (!mainBoard[i, j].IsPeculiar) mainBoard[i, j] = new Board(0, false, false);
                }
            }

            if (isPlaying)
            {
                NavigatePlayingPage();
                return;
            }
            this.frame.Navigate(new AnalysisPage(), this);
        }

        /// <summary>
        /// 数独のルールを表示する
        /// </summary>
        private void RuleOfSudokuButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            isPlaying = false;

            this.MenuToggleButton.IsChecked = false;

            this.frame.Navigate(new RulePage(), this);

            this.ClearAllButton.IsEnabled = false;
            this.ClearButton.IsEnabled = false;
        }

        /// <summary>
        /// PlaingPage を this.frame に表示する
        /// </summary>
        private void NavigatePlayingPage()
        {
            this.frame.Navigate(new PlayingPage(mainBoard), this);
            this.ClearAllButton.IsEnabled = true;
            this.ClearButton.IsEnabled = true;
        }

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;
        private void Frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (frame.Content != null && !_allowDirectNavigation)
            {
                e.Cancel = true;
                _navArgs = e;

                // 遷移前のページを画像に変換しイメージに設定
                Frame visual = this.frame;
                Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
                RenderTargetBitmap bitmap = new RenderTargetBitmap(
                    (int)bounds.Width,
                    (int)bounds.Height,
                    96.0,
                    96.0,
                    PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(visual);
                    dc.DrawRectangle(vb, null, bounds);
                }
                bitmap.Render(dv);
                bitmap.Freeze();

                // フレームに遷移先のページを設定
                _allowDirectNavigation = true;
                this.frame.Navigate(_navArgs.Content);

                // フレームを右からスライドさせるアニメーション
                ThicknessAnimation animation0 = new ThicknessAnimation();
                animation0.From = new Thickness(this.frame.ActualWidth, 60, -1 * this.frame.ActualWidth, 0);
                animation0.To = new Thickness(10, 60, 10, 10);
                animation0.Duration = TimeSpan.FromMilliseconds(ANIMATION_TIME_SPAN);
                this.frame.BeginAnimation(MarginProperty, animation0);

                // 遷移前ページを画像可した要素を左にスライドするアニメーション
                ThicknessAnimation animation1 = new ThicknessAnimation();
                animation1.From = new Thickness(10, 60, 10, 10);
                animation1.To = new Thickness(-1 * this.frame.ActualWidth, 60, this.frame.ActualWidth, 0);
                animation1.Duration = TimeSpan.FromMilliseconds(ANIMATION_TIME_SPAN);
            }

            _allowDirectNavigation = false;
        }
    }
}
