using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using Newtonsoft.Json;
using SuperControls.Style;
using SuperToolBox.Config;
using SuperUtils.Common;
using SuperUtils.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for UrlEncodeDecode.xaml
    /// </summary>
    public partial class HeaderFormat : Page
    {
        public HeaderFormat()
        {
            InitializeComponent();
        }

        private void EncodeText(object sender, RoutedEventArgs e)
        {
            string value = originTextBox.Text;
            if (string.IsNullOrEmpty(value))
                targetTextBox.Text = "";
            else
                targetTextBox.Text = Uri.EscapeDataString(value);
        }

        private void DecodeText(object sender, RoutedEventArgs e)
        {
            string value = targetTextBox.Text;
            if (string.IsNullOrEmpty(value))
                originTextBox.Text = "";
            else
                originTextBox.Text = Uri.UnescapeDataString(value);
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
            if (string.IsNullOrEmpty(targetTextBox.Text)) return;
            if (ClipBoard.TrySetDataObject(targetTextBox.Text))
            {
                MessageNotify.Success("复制成功");
            }
        }

        private void ClearOriginText(object sender, RoutedEventArgs e)
        {
            originTextBox.Clear();
        }

        private void ClearTargetText(object sender, RoutedEventArgs e)
        {
            targetTextBox.Clear();
        }
        private static double MAX_FONTSIZE = 25;
        private static double MIN_FONTSIZE = 5;
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

            originTextBox.Text = ConfigManager.HeaderFormat.OriginText;
            //targetTextBox.Text = ConfigManager.HeaderFormat.TargetText;

            SearchPanel.Install(originTextBox);
            SearchPanel.Install(targetTextBox);

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveText();
        }

        private void SaveText()
        {

            ConfigManager.HeaderFormat.OriginText = originTextBox.Text;
            ConfigManager.HeaderFormat.TargetText = targetTextBox.Text;
            ConfigManager.HeaderFormat.Save();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageNotify.Info("Alt+Shift+F 格式化为 json 或 转为一行");
        }

        private void originTextBox_TextChanged(object sender, EventArgs e)
        {
            TextEditor textEditor = sender as TextEditor;
            if (originLineTextBlock != null && textEditor != null)
                originLineTextBlock.Text = $"总长度：{textEditor.Text.Length} 总行数：{textEditor.LineCount}";

            // 转换
            string text = FormatString(originTextBox.Text);
            targetTextBox.Text = text;
        }


        public string FormatString(string origin)
        {
            if (string.IsNullOrEmpty(origin))
                return "";
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var item in origin.Split(Environment.NewLine.ToCharArray()))
            {
                if (item.IndexOf(":") > 0 && item.IndexOf(":") < item.Length - 1)
                {
                    int idx = item.IndexOf(":");
                    string key = item.Substring(0, idx).Trim();
                    string value = item.Substring(idx + 1).Trim();
                    if (!dict.ContainsKey(key))
                        dict.Add(key, value);
                }
            }
            return JsonUtils.TrySerializeObject(dict);
        }



        private void targetTextBox_TextChanged(object sender, EventArgs e)
        {
            TextEditor textEditor = sender as TextEditor;
            if (targetLineTextBlock != null && textEditor != null)
                targetLineTextBlock.Text = $"总长度：{textEditor.Text.Length} 总行数：{textEditor.LineCount}";
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
    }
}
