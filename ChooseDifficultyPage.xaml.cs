
// ChooseDifficultyPage.xaml.cs

using System.Windows;
using System.Windows.Controls;


namespace Sudoku
{
    /// <summary>
    /// ChooseDifficultyPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ChooseDifficultyPage : Page
    {
        /// <summary>
        /// コントラクタ
        /// </summary>
        public ChooseDifficultyPage()
        {
            InitializeComponent();
            MainWindow.difficulty = -1;
        }

        /// <summary>
        /// 入門
        /// </summary>
        private void Lv1(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 0;
        }

        /// <summary>
        /// 超初級
        /// </summary>
        private void Lv2(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 1;
        }

        /// <summary>
        /// 初級
        /// </summary>
        private void Lv3(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 2;
        }

        /// <summary>
        /// 中級
        /// </summary>
        private void Lv4(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 3;
        }

        /// <summary>
        /// 上級
        /// </summary>
        private void Lv5(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 4;
        }

        /// <summary>
        /// 難問
        /// </summary>
        private void Lv6(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 5;
        }

        /// <summary>
        /// 超難問
        /// </summary>
        private void Lv7(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 6;
        }

        /// <summary>
        /// 鬼
        /// </summary>
        private void Lv8(object sender, RoutedEventArgs e)
        {
            MainWindow.difficulty = 7;
        }
    }
}
