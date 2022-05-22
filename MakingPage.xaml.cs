
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

            this.MakingProgressBar.IsIndeterminate = true;
        }
    }
}
