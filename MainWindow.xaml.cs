
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
        private static DispatcherTimer timer1 = new DispatcherTimer();
        private static DispatcherTimer timer2 = new DispatcherTimer();
        private static DispatcherTimer timer3 = new DispatcherTimer();

        private static bool isTimeAttack = false;

        Thread thread = new Thread(new ThreadStart(() => { MainBoard = Maker.MakeSudokuMain(Values.difficultyList[Values.Difficulty]); }));


        /// <summary>
        /// ゲームを始める
        /// タイムアタックボタンが押されたときの処理
        /// </summary>
        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            isTimeAttack = true;

            GameMain();
        }

        /// <summary>
        /// 新規生成する
        /// </summary>
        private void NewGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            isTimeAttack = false;

            GameMain();
        }
        
        /// <summary>
        /// ゲームのMain処理
        /// </summary>
        private void GameMain()
        {
            MenuButton_Uncheck();
            NavigateChooseDifficultyPage();
            Values.IsPlaying = false;
            if (timer1.IsEnabled) return;
            timer1.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer1.Tick += new EventHandler(TimerMethod_IsSelected);
            timer1.Start();
        }


        /// <summary>
        /// BeginButton_Click >> timer2
        /// </summary>
        private void TimerMethod_IsSelected(object sender, EventArgs e)
        {
            if (Values.Difficulty == -1) return;

            StopAllTimers();
            MakeNewSudoku();
        }

        /// <summary>
        /// 迷路を生成する関数
        /// </summary>
        private void MakeNewSudoku()
        {
            StopAllTimers();

            NavigateMakingPage();

            thread = new Thread(new ThreadStart(() => { MainBoard = Maker.MakeSudokuMain(Values.difficultyList[Values.Difficulty]); }));
            thread.Interrupt();
            thread.Start();
            
            timer2.Interval = new TimeSpan(0, 0, 0, 0, Values.UPDATE_RATE);
            timer2.Tick += new EventHandler(TimerMethod_IsFinished);
            timer2.Start();
        }

        /// <summary>
        /// 3秒後に呼ばれるメソッド
        /// </summary>
        private void TimerMethod_CountDown(object sender, EventArgs e)
        {
            StopAllTimers();
            Values.IsPlaying = true;

            NavigatePlayingPage();
            watch.Restart();
        }


        /// <summary>
        /// MakeNewSudoku >> timer3
        /// </summary>
        private void TimerMethod_IsFinished(object sender, EventArgs e)
        {
            if (thread.ThreadState == System.Threading.ThreadState.Running) return;
            
            StopAllTimers();

            if (isTimeAttack)
            {
                NavigateCountDownPage();
                
                timer3.Interval = new TimeSpan(0, 0, 0, Values.INITAL_COUNTDOWN, 0);
                timer3.Tick += new EventHandler(TimerMethod_CountDown);
                timer3.Start();

                isTimeAttack = false;
            }
            else
            {
                NavigatePlayingPage();
                watch.Restart();
            }
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
            NavigateSettingsPage();

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
        }

        /// <summary>
        /// SettingsPage を this.frame に表示する
        /// </summary>
        private void NavigateSettingsPage()
        {
            this.frame.Navigate(new SettingsPage(), this);
        }

        /// <summary>
        /// CountDownPage を this.frame に表示する
        /// </summary>
        private void NavigateCountDownPage()
        {
            this.frame.Navigate(new CountDownPage(), this);
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

        /// <summary>
        /// 全てのタイマーを停止させる
        /// </summary>
        private void StopAllTimers()
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
        }


        /// <summary>
        /// window が閉じていく時のイベント
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (thread.ThreadState == System.Threading.ThreadState.Running) thread.Abort();
        }
    }
}
