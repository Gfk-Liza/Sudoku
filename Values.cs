
// Values.xaml.cs

using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;


namespace Sudoku
{
    /// <summary>
    /// 様々な値(すべて public)を入れておくクラス
    /// </summary>
    public static class Values
    {
        // レベル選択画面のボタンの更新速度(ミリ秒)
        public const byte UPDATE_RATE = 50;

        // 画面遷移時の速度(ミリ秒)
        public const byte ANIMATION_TIME_SPAN = 200;

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

        // 各レベルごとの説明
        public static readonly string[] EXPLANATION = new string[8]
        {
            $"入門レベル\n{difficultyList[0]}個程の穴が空いています。\n数独を始めたばかりの人にオススメです。",
            $"超初級レベル\n{difficultyList[1]}個程の穴が空いています。\nある程度慣れた人は、瞬殺。",
            $"初級レベル\n{difficultyList[2]}個程の穴が空いています。\n仮定法を取得中の方へ。",
            $"中級レベル\n{difficultyList[3]}個程の穴が空いています。\n数独についてある程度基礎の解き方が分かっている人にオススメです。",
            $"上級レベル\n{difficultyList[4]}個程の穴が空いています。\n数独について慣れたら。",
            $"難問レベル\n{difficultyList[5]}個程の穴が空いています。\n時間をかければできるかも。",
            $"超難問レベル\n{difficultyList[6]}個程の穴が空いています。\nできたら、超すごい。",
            $"鬼レベル\n可能な限り穴が空いています。\nプロか天才。",
        };

        public static readonly Key[] keyesList = new Key[20]
        {
            Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
            Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
            Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9
        };

        public static readonly string[] converter = new string[10] { " ", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

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

        // タイムアタックモードか
        public static bool isTimeAttack = false;

        // バージョン情報
        public static readonly string VersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        // カウントダウンの値
        public const sbyte INITAL_COUNTDOWN = 3;
    }
}
