using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public MainPage()
        {
            this.InitializeComponent();

            // ペンの初期化
            Init();
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
            var PenSize1 = (sliderPenSize.Value);
            inkAttr.Size = new Size(PenSize1, PenSize1);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkAttr);
        }

        private void cmbPenColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inkAttr == null)
            {
                return;
            }

            // ペンの色
            switch(cmbPenColor.SelectedIndex)
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
    }
}
