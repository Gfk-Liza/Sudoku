
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
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private sbyte countNumber = 2;


        /// <summary>
        /// コントラクタ
        /// </summary>
        public CountDownPage()
        {
            InitializeComponent();

            // timer 起動
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
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

            this.CountDownLabel.Content = countNumber;
            countNumber--;
        }
    }
}
