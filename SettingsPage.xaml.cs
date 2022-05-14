
// SettingsPage.xaml.cs

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;


namespace Sudoku
{
    /// <summary>
    /// SettingsPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingsPage : Page
    {
        private static readonly string[] indexes = new string[8] {
            "とてもかんたん", "かんたん", "すこしかんたん", "ふつう",
            "すこしむずかしい", "むずかしい", "とてもむずかしい", "鬼" };

        private static readonly string[] colorList = new string[10] {
            "Black", "Gray", "Pink", "Red", "Orange", "Yellow", "YellowGreen", "Green", "Blue", "SkyBlue" };


        /// <summary>
        /// コントラクタ
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            foreach (string index in indexes)
            {
                this.DifficultyComboBox.Items.Add(index);
            }
            this.DifficultyComboBox.SelectedIndex = MainWindow.difficulty;

            foreach (string color in colorList)
            {
                this.ComboBox1.Items.Add(color);
                this.ComboBox2.Items.Add(color);
                this.ComboBox3.Items.Add(color);
                this.ComboBox4.Items.Add(color);
            }

            this.ComboBox1.SelectedIndex = MainWindow.colorSetting1;
            this.ComboBox2.SelectedIndex = MainWindow.colorSetting2;
            this.ComboBox3.SelectedIndex = MainWindow.colorSetting3;
            this.ComboBox4.SelectedIndex = MainWindow.colorSetting4;

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.VersionInfoLabel.Content = $"バージョン: {fileVersionInfo.ProductVersion}";
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.difficulty = (sbyte)this.DifficultyComboBox.SelectedIndex;
            
            MainWindow.colorSetting1 = (sbyte)this.ComboBox1.SelectedIndex;
            MainWindow.colorSetting2 = (sbyte)this.ComboBox2.SelectedIndex;
            MainWindow.colorSetting3 = (sbyte)this.ComboBox3.SelectedIndex;
            MainWindow.colorSetting4 = (sbyte)this.ComboBox4.SelectedIndex;
        }
    }
}
