
// CountDownPage.xaml.cs

using System;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Sudoku
{
    /// <summary>
    /// CountDownPage.xaml の相互作用ロジック
    /// </summary>
    public partial class CountDownPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private sbyte countNumber = 3;


        /// <summary>
        /// コントラクタ
        /// </summary>
        public CountDownPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// カウントダウンをはじめる
        /// </summary>
        public void StartCountDown()
        {
            countNumber = Values.INITAL_COUNTDOWN;
            this.CountDownLabel.Content = countNumber;

            // timer 起動
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1, 0) };
            timer.Tick += new EventHandler(TimerMethod);
            timer.Start();
        }

        /// <summary>
        /// timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
        /// からの呼び出し
        /// </summary>
        private void TimerMethod(object sender, EventArgs e)
        {
            if (countNumber <= 0)
            {
                timer.Stop();
                return;
            }

            this.CountDownLabel.Content = --countNumber;
        }
    }
}
