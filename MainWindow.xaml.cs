
// MainWindow.xaml.cs

using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
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

        // Main timer
        public static readonly Stopwatch watch = new Stopwatch();

        // タイマー
        private readonly DispatcherTimer timer1 = new DispatcherTimer();
        private readonly DispatcherTimer timer2 = new DispatcherTimer();

        private static bool isTimeAttack = false;

        Thread thread = new Thread(new ThreadStart(() => { MainBoard = Maker.MakeSudokuMain(Values.difficultyList[Values.Difficulty]); }));


        /// <summary>
        /// ゲームを始める
        /// タイムアタックボタンが押されたときの処理
        /// </summary>
        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton_Uncheck();
            NavigateChooseDifficultyPage();

            isTimeAttack = true;

            timer1.Stop();
            timer1.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer1.Tick += new EventHandler(TimerMethod1);
            timer1.Start();
        }

        /// <summary>
        /// 新規生成する
        /// </summary>
        private void NewGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton_Uncheck();
            NavigateChooseDifficultyPage();

            isTimeAttack = false;

            timer1.Stop();
            timer1.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer1.Tick += new EventHandler(TimerMethod3);
            timer1.Start();
        }


        /// <summary>
        /// BeginButton_Click >> timer2
        /// </summary>
        private void TimerMethod1(object sender, EventArgs e)
        {
            if (Values.Difficulty == -1) return;

            Debug.WriteLine("<<<PASS>>>");

            timer1.Stop();
            MakeNewSudoku();
        }

        /// <summary>
        /// 迷路を生成する関数
        /// </summary>
        private void MakeNewSudoku()
        {
            timer2.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod4);
            timer2.Start();

            NavigateMakingPage();

            thread = new Thread(new ThreadStart(() => { MainBoard = Maker.MakeSudokuMain(Values.difficultyList[Values.Difficulty]); }));
            thread.Interrupt();
            thread.Start();
        }

        /// <summary>
        /// 3秒後に呼ばれるメソッド
        /// </summary>
        private void TimerMethod2(object sender, EventArgs e)
        {
            timer1.Stop();
            Values.IsPlaying = true;

            NavigatePlayingPage();
            watch.Restart();
        }

        /// <summary>
        /// 0.05秒ごとに呼ばれるメソッド
        /// </summary>
        private void TimerMethod3(object sender, EventArgs e)
        {
            // 値が変更されていないなら関数をぬける
            if (Values.Difficulty == -1) return;

            timer1.Stop();
            MakeNewSudoku();
            watch.Restart();
        }

        /// <summary>
        /// MakeNewSudoku >> timer3
        /// </summary>
        private void TimerMethod4(object sender, EventArgs e)
        {
            if (thread.ThreadState == System.Threading.ThreadState.Running) return;

            timer2.Stop();

            if (isTimeAttack)
            {
                this.frame.Navigate(new CountDownPage(), this);
                timer1.Interval = new TimeSpan(0, 0, 0, Values.INITAL_COUNTDOWN, 0);
                timer1.Tick += new EventHandler(TimerMethod2);
                timer1.Start();
            }
            else NavigatePlayingPage();

            isTimeAttack = false;
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
            MenuButton_Uncheck();
            this.frame.Navigate(new SettingsPage(), this);

            ClearBTNsEnabled(false);
        }


        /// <summary>
        /// 解析モードにする
        /// </summary>
        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            watch.Stop();
            Values.IsPlaying = false;

            MenuButton_Uncheck();

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

            MenuButton_Uncheck();

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

            MenuButton_Uncheck();

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
        /// ChooseDifficultyPage を this.frame に表示する
        /// </summary>
        private void NavigateChooseDifficultyPage()
        {
            this.frame.Navigate(new ChooseDifficultyPage(), this);
            Values.Difficulty = -1;

            Debug.WriteLine("<<<ChoosePage>>>");
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

        /// <summary>
        /// メニューのボタンのチェックを外す
        /// </summary>
        private void MenuButton_Uncheck()
        {
            this.MenuToggleButton.IsChecked = false;
        }
    }
}
