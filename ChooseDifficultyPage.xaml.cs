
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
            this.InitializeComponent();
        }

        /// <summary>
        /// 前回の値をセットする
        /// </summary>
        public void SetValues()
        {
            this.DifficlutySlider.Value = Values.DifficultyTemp;
            this.DifficlutySlider.Maximum = Values.difficultyList.Length - 1;
            this.ExplanationLabel.Content = Values.EXPLANATION[Values.DifficultyTemp];
        }
        
        /// <summary>
        /// 難易度選択バーの値が変更された時
        /// </summary>
        private void DifficlutySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Values.DifficultyTemp = (sbyte)DifficlutySlider.Value;
            this.ExplanationLabel.Content = Values.EXPLANATION[Values.DifficultyTemp];
        }

        /// <summary>
        /// StartButtonがクリックされた時
        /// </summary>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Values.Difficulty = Values.DifficultyTemp;
        }
    }
}
