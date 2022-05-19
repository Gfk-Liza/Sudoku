
// ChooseDifficultyPage.xaml.cs

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace Sudoku
{
    /// <summary>
    /// ChooseDifficultyPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ChooseDifficultyPage : Page
    {
        private static sbyte difficultyTemp = 0;
        private static sbyte MAX_DIFFICULTY = (sbyte)MainWindow.difficultyList.Length;

        /// <summary>
        /// 直近の遷移の方向
        /// 0: null
        /// 1: ←
        /// 2: →
        /// </summary>
        private static sbyte mostRecentActivity = 0;

        /// <summary>
        /// コントラクタ
        /// </summary>
        public ChooseDifficultyPage()
        {
            difficultyTemp = MainWindow.difficulty;
            MainWindow.difficulty = -1;

            InitializeComponent();
        }


        private void BeforeButton_Click(object sender, RoutedEventArgs e)
        {
            difficultyTemp = DifficultyMod((sbyte)(difficultyTemp - 1));
            mostRecentActivity = 1;
        }


        /// <summary>
        /// 難易度(difficulty)を難易度の量(MAX_DIFFICULTY)で割った余りを返す
        /// </summary>
        /// 
        /// <param name="_difficulty">
        /// 処理前の難易度値
        /// </param>
        /// 
        /// <returns>
        /// 難易度を難易度の量で割った余り
        /// </returns>
        private sbyte DifficultyMod(sbyte _difficulty)
        {
            return (sbyte)(_difficulty % MAX_DIFFICULTY);
        }

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;
        private void Frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (this.DifficultyFrame.Content != null && !_allowDirectNavigation)
            {
                e.Cancel = true;
                _navArgs = e;

                // 遷移前のページを画像に変換しイメージに設定
                Frame visual = this.DifficultyFrame;
                Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
                RenderTargetBitmap bitmap = new RenderTargetBitmap(
                    (int)bounds.Width,
                    (int)bounds.Height,
                    96.0,
                    96.0,
                    PixelFormats.Pbgra32);
                DrawingVisual dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(visual);
                    dc.DrawRectangle(vb, null, bounds);
                }
                bitmap.Render(dv);
                bitmap.Freeze();

                // フレームに遷移先のページを設定
                _allowDirectNavigation = true;
                this.DifficultyFrame.Navigate(_navArgs.Content);

                // フレームを右(又は左)からスライドさせるアニメーション
                ThicknessAnimation animation0 = new ThicknessAnimation();
                if (mostRecentActivity == 1) animation0.From = new Thickness(this.DifficultyFrame.ActualWidth, 60, -1 * this.DifficultyFrame.ActualWidth, 0);
                else animation0.From = new Thickness(-1 * this.DifficultyFrame.ActualWidth, 60, this.DifficultyFrame.ActualWidth, 0);
                animation0.To = new Thickness(10, 60, 10, 10);
                animation0.Duration = TimeSpan.FromMilliseconds(MainWindow.ANIMATION_TIME_SPAN);
                this.DifficultyFrame.BeginAnimation(MarginProperty, animation0);

                // 遷移前ページを画像可した要素を左(又は右)にスライドするアニメーション
                ThicknessAnimation animation1 = new ThicknessAnimation { From = new Thickness(10, 60, 10, 10) };
                if (mostRecentActivity == 1) animation1.To = new Thickness(-1 * this.DifficultyFrame.ActualWidth, 60, this.DifficultyFrame.ActualWidth, 0);
                else animation1.To = new Thickness(this.DifficultyFrame.ActualWidth, 60, -1 * this.DifficultyFrame.ActualWidth, 0);
                animation1.Duration = TimeSpan.FromMilliseconds(MainWindow.ANIMATION_TIME_SPAN);
            }

            _allowDirectNavigation = false;
        }
    }
}
