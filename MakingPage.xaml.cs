
// MakingPage.xaml.cs

using System;
using System.Windows.Controls;


namespace Sudoku
{
    /// <summary>
    /// MakingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class MakingPage : Page
    {
        /// <summary>
        /// コントラクター
        /// </summary>
        public MakingPage()
        {
            InitializeComponent();
        }

        public static sbyte progressValue = 0;

        private void UpdateProgressBarMain(object sender, EventArgs e)
        {
            this.MakingProgressBar.Value = progressValue;
        }
    }
}
