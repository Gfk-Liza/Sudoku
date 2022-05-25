
// Values.xaml.cs

using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;


namespace Sudoku
{
    /// <summary>
    /// 様々な値(すべて public)を入れておくクラス
    /// </summary>
    public static class Values
    {
        // レベル選択画面のボタンの更新速度(ミリ秒)
        public const int UPDATE_RATE = 50;

        // 画面遷移時の速度(ミリ秒)
        public const int ANIMATION_TIME_SPAN = 200;

        // 色
        public static readonly SolidColorBrush[] colorList = new SolidColorBrush[10]
        {
            Brushes.Black, Brushes.Gray, Brushes.Pink, Brushes.Red, Brushes.Orange,
            Brushes.Yellow, Brushes.YellowGreen, Brushes.Green, Brushes.Blue, Brushes.SkyBlue
        };

        // 色の名前
        public static readonly string[] colorNamesList = new string[10]
        {
            "黒", "灰", "ピンク", "赤", "オレンジ", "黄", "黄緑", "緑", "青", "水"
        };

        // あける穴の数（難易度別）
        public static readonly sbyte[] difficultyList = new sbyte[8] { 20, 27, 35, 42, 46, 50, 54, 81 };


        // play中か
        public static bool IsPlaying = false;

        // 難易度
        public static sbyte Difficulty = 3;
        // 難易度バックアップ
        public static sbyte DifficultyTemp = Difficulty;
        // 選択されている X 座標
        public static sbyte SelectedX = -1;
        // 選択されている Y 座標
        public static sbyte SelectedY = -1;

        // 元の数字の色の設定
        public static sbyte ColorSetting1 = 0;
        // 記入する数字の色の設定
        public static sbyte ColorSetting2 = 4;
        // 回答の数字の色の設定
        public static sbyte ColorSetting3 = 3;
        // 選択枠の色の設定
        public static sbyte ColorSetting4 = 5;
        // メモの数字の色の設定
        public static sbyte ColorSetting5 = 1;

        // 入力時にルールに違反していたら、下線を引く
        public static bool CheckSetting1 = false;

        // バージョン情報
        public static readonly string VersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        // カウントダウンの値
        public const sbyte INITAL_COUNTDOWN = 3;
    }
}
