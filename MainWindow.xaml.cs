
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
            ClearBTNsEnabled(false);

            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        

        // MainBoard
        public static Board[,] MainBoard = NewBoard.MakeNewBoard(false);

        // あける穴の数（難易度別）
        public static readonly sbyte[] difficultyList = new sbyte[8] { 20, 27, 35, 42, 46, 50, 54, 81 };

        // Main timer
        public static readonly Stopwatch watch = new Stopwatch();

        // カウントダウン復帰用のタイマー
        private readonly DispatcherTimer timer = new DispatcherTimer();

        // カウントダウン復帰用のタイマー
        private readonly DispatcherTimer timer2 = new DispatcherTimer();

        private readonly DispatcherTimer timer3 = new DispatcherTimer();

        private readonly Thread thread = new Thread(new ThreadStart(() => { MainBoard = Maker.MakeSudokuMain(difficultyList[Values.Difficulty]); }));



        /// <summary>
        /// ゲームを始める
        /// タイムアタックボタンが押されたときの処理
        /// </summary>
        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            this.MenuToggleButton.IsChecked = false;
            this.frame.Navigate(new ChooseDifficultyPage(), this);

            timer2.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod1);
            timer2.Start();
        }

        /// <summary>
        /// 0.05秒ごとに呼ばれるメソッド
        /// </summary>
        private void TimerMethod1(object sender, EventArgs e)
        {
            if (Values.Difficulty == -1) return;

            timer2.Stop();

            MakeNewSudoku();
            
            this.frame.Navigate(new CountDownPage(), this);
            timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            timer.Tick += new EventHandler(TimerMethod2);
            timer.Start();

            Values.Difficulty = -1;
        }

        /// <summary>
        /// 3秒後に呼ばれるメソッド
        /// </summary>
        private void TimerMethod2(object sender, EventArgs e)
        {
            timer.Stop();
            Values.IsPlaying = true;

            watch.Restart();
            NavigatePlayingPage();
        }


        /// <summary>
        /// 迷路を生成する関数
        /// </summary>
        private void MakeNewSudoku()
        {
            timer3.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer3.Tick += new EventHandler(TimerMethod4);
            timer3.Start();

            NavigateMakingPage();

            thread.Start();
            
        }

        /// <summary>
        /// MakeNewSudoku >> timer3
        /// </summary>
        private void TimerMethod4(object sender, EventArgs e)
        {
            if (thread.ThreadState == System.Threading.ThreadState.Running) return;

            timer3.Stop();
            NavigatePlayingPage();
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

            ClearBTNsEnabled(false);
        }

        /// <summary>
        /// 新規生成する
        /// </summary>
        private void NewGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            this.MenuToggleButton.IsChecked = false;
            this.frame.Navigate(new ChooseDifficultyPage(), this);

            timer2.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod3);
            timer2.Start();
        }

        /// <summary>
        /// 0.05秒ごとに呼ばれるメソッド
        /// </summary>
        private void TimerMethod3(object sender, EventArgs e)
        {
            // 値が変更されていないなら関数をぬける
            if (Values.Difficulty == -1) return;

            timer2.Stop();
            MakeNewSudoku();
            watch.Restart();
        }


        /// <summary>
        /// 解析モードにする
        /// </summary>
        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            Values.IsPlaying = false;

            this.MenuToggleButton.IsChecked = false;

            NavigateAnalysisPage();

            ClearBTNsEnabled(true);
        }


        /// <summary>
        /// 答えを表示する
        /// </summary>
        private void SolutionButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            Values.IsPlaying = false;

            MainBoard = Solver.SolveSudokuMain(MainBoard);

            this.MenuToggleButton.IsChecked = false;

            NavigatePlayingPage();
        }

        /// <summary>
        /// まっさらにする
        /// </summary>
        private void BeBrandNewButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            MainBoard = NewBoard.MakeNewBoard(false);
            if (Values.IsPlaying)
            {
                Values.IsPlaying = false;
                NavigatePlayingPage();
                return;
            }
            NavigateAnalysisPage();
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
                    if (!MainBoard[i, j].IsPeculiar) MainBoard[i, j] = new Board(0, false, false);
                }
            }

            if (Values.IsPlaying)
            {
                NavigatePlayingPage();
                return;
            }
            NavigateAnalysisPage();
        }

        /// <summary>
        /// 数独のルールを表示する
        /// </summary>
        private void RuleOfSudokuButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            Values.IsPlaying = false;

            this.MenuToggleButton.IsChecked = false;

            NavigateRulePage();

            ClearBTNsEnabled(false);
        }


        /// <summary>
        /// PlaingPage を this.frame に表示する
        /// </summary>
        private void NavigatePlayingPage()
        {
            this.frame.Navigate(new PlayingPage(), this);
            ClearBTNsEnabled(false);
        }

        /// <summary>
        /// RulePage を this.frame に表示する
        /// </summary>
        private void NavigateRulePage()
        {
            this.frame.Navigate(new RulePage(), this);
        }

        /// <summary>
        /// AnalysisPage を this.frame に表示する
        /// </summary>
        private void NavigateAnalysisPage()
        {
            this.frame.Navigate(new AnalysisPage(), this);
        }

        /// <summary>
        /// MakingPage を this.frame に表示する
        /// </summary>
        private void NavigateMakingPage()
        {
            this.frame.Navigate(new MakingPage(), this);
        }

        /// <summary>
        /// Clearボタンを押せるようにするか
        /// </summary>
        /// 
        /// <param name="_isEnabled">
        /// ボタンの .isEnabled
        /// </param>
        private void ClearBTNsEnabled(bool _isEnabled)
        {
            this.ClearAllButton.IsEnabled = _isEnabled;
            this.ClearButton.IsEnabled = _isEnabled;
        }

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;
        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
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
                using (DrawingContext dc = dv.RenderOpen())
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
                animation0.Duration = TimeSpan.FromMilliseconds(Values.ANIMATION_TIME_SPAN);
                this.frame.BeginAnimation(MarginProperty, animation0);

                // 遷移前ページを画像可した要素を左にスライドするアニメーション
                ThicknessAnimation animation1 = new ThicknessAnimation();
                animation1.From = new Thickness(10, 60, 10, 10);
                animation1.To = new Thickness(-1 * this.frame.ActualWidth, 60, this.frame.ActualWidth, 0);
                animation1.Duration = TimeSpan.FromMilliseconds(Values.ANIMATION_TIME_SPAN);
            }

            _allowDirectNavigation = false;
        }
    }
}
