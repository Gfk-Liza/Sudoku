
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
        // 各レベルごとの説明
        private static readonly string[] EXPLANATION = new string[8]
        {
            $"入門レベル\n{Values.difficultyList[0]}個程の穴が空いています。\n数独を始めたばかりの人にオススメです。",
            $"超初級レベル\n{Values.difficultyList[1]}個程の穴が空いています。\nある程度慣れた人は、瞬殺。",
            $"初級レベル\n{Values.difficultyList[2]}個程の穴が空いています。\n仮定法を取得中の方へ。",
            $"中級レベル\n{Values.difficultyList[3]}個程の穴が空いています。\n数独についてある程度基礎の解き方が分かっている人にオススメです。",
            $"上級レベル\n{Values.difficultyList[4]}個程の穴が空いています。\n数独について慣れたら。",
            $"難問レベル\n{Values.difficultyList[5]}個程の穴が空いています。\n時間をかければできるかも。",
            $"超難問レベル\n{Values.difficultyList[6]}個程の穴が空いています。\nできたら、超すごい。",
            $"鬼レベル\n可能な限り穴が空いています。\nプロか天才。",
        };

        /// <summary>
        /// コントラクタ
        /// </summary>
        public ChooseDifficultyPage()
        {
            this.InitializeComponent();

            this.DifficlutySlider.Value = Values.DifficultyTemp;
            this.DifficlutySlider.Maximum = Values.difficultyList.Length - 1;
            this.ExplanationLabel.Content = EXPLANATION[Values.DifficultyTemp];
        }
        
        /// <summary>
        /// 難易度選択バーの値が変更された時
        /// </summary>
        private void DifficlutySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Values.DifficultyTemp = (sbyte)DifficlutySlider.Value;
            this.ExplanationLabel.Content = EXPLANATION[Values.DifficultyTemp];
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
