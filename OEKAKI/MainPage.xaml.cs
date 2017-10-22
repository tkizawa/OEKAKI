using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace OEKAKI
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private InkDrawingAttributes inkAttr;
        private int penSize = 5;
        RadialController myController;  // Surface Dialのコントローラー

        public MainPage()
        {
            this.InitializeComponent();

            // ペンの初期化
            Init();
            // SUrface Dial初期化
            InitDial();
        }

        /// <summary>
        /// ペンの初期化処理
        /// </summary>
        private void Init()
        {
            // 入力デバイスの指定
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse |  // マウス
                Windows.UI.Core.CoreInputDeviceTypes.Pen |                                          // ペン
                Windows.UI.Core.CoreInputDeviceTypes.Touch;                                         // タッチ

            //　ペンの属性
            inkAttr = new InkDrawingAttributes();
            inkAttr.Color = Windows.UI.Colors.Black;
            inkAttr.PenTip = PenTipShape.Circle;
            inkAttr.Size = new Size(penSize, penSize);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkAttr);
        }

        /// <summary>
        /// ペンの太さを変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderPenSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (inkAttr == null)
            {
                return;
            }

            // ペンの太さ
            var PenSize1 = (SliderPenSize.Value);
            inkAttr.Size = new Size(PenSize1, PenSize1);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkAttr);
        }

        /// <summary>
        /// ペンの色の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPenColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inkAttr == null)
            {
                return;
            }

            // ペンの色
            switch (cboPenColor.SelectedIndex)
            {
                case 0:
                    inkAttr.Color = Windows.UI.Colors.Black;
                    break;
                case 1:
                    inkAttr.Color = Windows.UI.Colors.Red;
                    break;
                case 2:
                    inkAttr.Color = Windows.UI.Colors.Blue;
                    break;
                default:
                    inkAttr.Color = Windows.UI.Colors.Black;
                    break;
            }
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkAttr);
        }

        /// <summary>
        /// Surface Dialの初期化
        /// </summary>
        private void InitDial()
        {
            // RadialControllerのインスタンスを作成する
            myController = RadialController.CreateForCurrentView();

            // カスタムツールのアイコンを作成する
            RandomAccessStreamReference icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/StoreLogo.png"));

            // メニューの作成
            RadialControllerMenuItem myItem =
                RadialControllerMenuItem.CreateFromIcon("お絵かきペンサイズ", icon);
            // メニューの追加
            myController.Menu.Items.Add(myItem);

            // メニューの作成
            RadialControllerMenuItem myItem1 =
                RadialControllerMenuItem.CreateFromIcon("お絵かきペンカラー", icon);
            // メニューの追加
            myController.Menu.Items.Add(myItem1);

            // 標準メニューの削除
            var config = RadialControllerConfiguration.GetForCurrentView();
            config.SetDefaultMenuItems(Enumerable.Empty<RadialControllerSystemMenuItemKind>());

            // 回転の単位
            myController.RotationResolutionInDegrees = 1;

            // ハンドラの追加
            myController.ButtonClicked += MyController_ButtonClicked;
            myController.RotationChanged += MyController_RotationChanged;
        }

        // ハンドラ
        private void MyController_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            RadialControllerMenuItem selected = myController.Menu.GetSelectedMenuItem();
            var MenuText = selected.DisplayText;

            if (MenuText.Equals("お絵かきペンサイズ") == true)
            {
                if (SliderPenSize.Value + args.RotationDeltaInDegrees > 100)
                {
                    SliderPenSize.Value = 10;
                    return;
                }
                else if (SliderPenSize.Value + args.RotationDeltaInDegrees < 0)
                {
                    SliderPenSize.Value = 1;
                    return;
                }
                SliderPenSize.Value += args.RotationDeltaInDegrees;
            }
            else if (MenuText.Equals("お絵かきペンカラー") == true)
            {
                var idx = cboPenColor.SelectedIndex;
                idx = idx + 1;
                if (idx > 2)
                {
                    idx = 0;
                }
                cboPenColor.SelectedIndex = idx;
            }
        }

        private void MyController_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
//            ButtonToggle.IsOn = !ButtonToggle.IsOn;
        }

    }
}
