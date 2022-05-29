
// SettingsPage.xaml.cs


using System.Windows.Controls;


namespace Sudoku
{
    /// <summary>
    /// SettingsPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingsPage : Page
    {
        /// <summary>
        /// コントラクタ
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 値を更新する
        /// </summary>
        public void SetValues()
        {
            foreach (string color in Values.colorNamesList)
            {
                this.ComboBox1.Items.Add(color);
                this.ComboBox2.Items.Add(color);
                this.ComboBox3.Items.Add(color);
                this.ComboBox4.Items.Add(color);
                this.ComboBox5.Items.Add(color);
            }

            this.ComboBox1.SelectedIndex = Values.ColorSetting1;
            this.ComboBox2.SelectedIndex = Values.ColorSetting2;
            this.ComboBox3.SelectedIndex = Values.ColorSetting3;
            this.ComboBox4.SelectedIndex = Values.ColorSetting4;
            this.ComboBox5.SelectedIndex = Values.ColorSetting5;

            this.CheackBox1.IsChecked = Values.CheckSetting1;

            this.VersionInfoLabel.Content = $"バージョン: {Values.VersionInfo}";
        }

        /// <summary>
        /// 適用ボタンが押されたときの処理
        /// </summary>
        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Values.ColorSetting1 = (sbyte)this.ComboBox1.SelectedIndex;
            Values.ColorSetting2 = (sbyte)this.ComboBox2.SelectedIndex;
            Values.ColorSetting3 = (sbyte)this.ComboBox3.SelectedIndex;
            Values.ColorSetting4 = (sbyte)this.ComboBox4.SelectedIndex;
            Values.ColorSetting5 = (sbyte)this.ComboBox5.SelectedIndex;

            Values.CheckSetting1 = (bool)this.CheackBox1.IsChecked;
        }
    }
}
