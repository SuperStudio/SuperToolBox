using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using SuperControls.Style;
using SuperToolBox.Config;
using SuperUtils.Common;
using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using static SuperToolBox.App;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for CharTransform.xaml
    /// </summary>
    public partial class CharTransform : Page
    {
        private const double MAX_FONTSIZE = 25;
        private const double MIN_FONTSIZE = 5;
        public enum TransformType
        {
            STR = 0,
            BIN = 2,
            OCT = 8,
            DEC = 10,
            HEX = 16,
        }

        public List<TransformType> TRANS_FORM_TYPE_DICT = new List<TransformType>()
        {
            TransformType.BIN,
            TransformType.OCT,
            TransformType.DEC,
            TransformType.HEX,
            TransformType.STR,
        };

        public static List<SearchBox> SEARCHBOX_DICT { get; set; }



        public CharTransform()
        {
            InitializeComponent();
        }

        private void CopyOriginText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(originTextBox.Text)) return;
            if (ClipBoard.TrySetDataObject(originTextBox.Text))
            {
                MessageNotify.Success("复制成功");
            }
        }

        private void CopyTargetText(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = (sender as FrameworkElement).Parent as StackPanel;
            if (stackPanel != null)
            {
                SearchBox searchBox = (stackPanel.Parent as StackPanel).Children
                    .OfType<SearchBox>().Last();
                if (searchBox != null)
                    ClipBoard.TrySetDataObject(searchBox.Text);
            }
        }

        private void ClearOriginText(object sender, RoutedEventArgs e)
        {
            originTextBox.Clear();
        }


        private void originTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Border border = sender as Border;
                TextEditor textEditor = border.Child as TextEditor;
                double fontSize = textEditor.FontSize;
                if (e.Delta > 0)
                {
                    fontSize++;
                }
                else
                {
                    fontSize--;
                }
                if (fontSize > MAX_FONTSIZE) fontSize = MAX_FONTSIZE;
                if (fontSize < MIN_FONTSIZE) fontSize = MIN_FONTSIZE;

                textEditor.FontSize = fontSize;
                e.Handled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextEditorOptions textEditorOptions = new TextEditorOptions();
            textEditorOptions.EnableHyperlinks = false;
            textEditorOptions.HighlightCurrentLine = true;
            originTextBox.Options = textEditorOptions;

            originTextBox.Text = ConfigManager.CharTransformConfig.OriginText;
            SearchPanel.Install(originTextBox);

            SEARCHBOX_DICT = new List<SearchBox>()
            {
                searchBox1,
                searchBox2,
                searchBox3,
                searchBox4,
            };

            List<RadioButton> radioButtons = stackPanel1.Children.OfType<RadioButton>().ToList();
            for (int i = 0; i < radioButtons.Count; i++)
            {
                if (i == ConfigManager.CharTransformConfig.TypeIndex)
                {
                    radioButtons[i].IsChecked = true;
                    break;
                }
            }


        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveText();
        }

        private void SaveText()
        {

            ConfigManager.CharTransformConfig.OriginText = originTextBox.Text;

            List<RadioButton> radioButtons = stackPanel1.Children.OfType<RadioButton>().ToList();
            for (int i = 0; i < radioButtons.Count; i++)
            {
                if ((bool)radioButtons[i].IsChecked)
                {
                    ConfigManager.CharTransformConfig.TypeIndex = i;
                    break;
                }
            }
            ConfigManager.CharTransformConfig.Save();
        }



        private void originTextBox_TextChanged(object sender, EventArgs e)
        {
            TextEditor textEditor = sender as TextEditor;
            if (originLineTextBlock != null && textEditor != null)
                originLineTextBlock.Text = $"总长度：{textEditor.Text.Length} 总行数：{textEditor.LineCount}";
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveText();
            ((sender as TextEditor).Parent as Border).BorderBrush = Brushes.Transparent;
        }



        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((sender as TextEditor).Parent as Border).BorderBrush = (SolidColorBrush)Application.Current.Resources["Button.Selected.BorderBrush"];
        }



        private void Transform(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SEARCHBOX_DICT.Count; i++)
                SEARCHBOX_DICT[i].Text = "";
            strTextBox.Text = "";

            string originText = originTextBox.Text;
            if (string.IsNullOrEmpty(originText))
                return;

            originText = originText.Trim();
            if (string.IsNullOrEmpty(originText))
                return;

            TransformType originType = GetTransformType(stackPanel1);
            long numValue = -1;
            try
            {
                if (originType != TransformType.STR)
                    numValue = Convert.ToInt64(originText, (int)originType);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }



            for (int i = 0; i < TRANS_FORM_TYPE_DICT.Count; i++)
            {
                string result = "";
                TransformType type = TRANS_FORM_TYPE_DICT[i];
                if (type == TransformType.STR)
                {
                    if (originType != TransformType.HEX)
                    {
                        result = originText;
                    }
                    else
                    {
                        // HEX 转字符串
                        result = TransformHelper.HexToStr(originText);
                    }
                    strTextBox.Text = result;
                }
                else
                {
                    if (originType != TransformType.STR)
                        result = Convert.ToString(numValue, (int)type).ToUpper();
                    else
                    {
                        if (type == TransformType.HEX)
                            result = TransformHelper.StrToHex(originText);
                        else
                            result = "";
                    }
                    if (numValue == -1)
                        result = "";
                }
                if (i < SEARCHBOX_DICT.Count)
                    SEARCHBOX_DICT[i].Text = result;
            }
        }


        public TransformType GetTransformType(StackPanel stackPanel)
        {
            List<RadioButton> list = stackPanel.Children.OfType<RadioButton>().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if ((bool)list[i].IsChecked)
                    return TRANS_FORM_TYPE_DICT[i];
            }
            return 0;
        }

        private void CopySTRText(object sender, RoutedEventArgs e)
        {
            ClipBoard.TrySetDataObject(strTextBox.Text);
        }
    }
}
