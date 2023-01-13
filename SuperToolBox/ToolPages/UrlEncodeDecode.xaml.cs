using ICSharpCode.AvalonEdit;
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
    public partial class UrlEncodeDecode : Page
    {
        public UrlEncodeDecode()
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

        private void Border_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.F) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                // 格式化
                //textEditor.SyntaxHighlighting = null;
                //textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.HighlightingDefinitions;
                FormatString(originTextBox);
            }
        }

        private void FormatString(TextEditor textEditor)
        {
            string text = textEditor.Text;
            if (text.IndexOf("\r") >= 0 || text.IndexOf("\n") >= 0)
            {
                textEditor.Text = text.Trim()
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace(" ", "");
            }
            else
            {
                Dictionary<string, object> dictionary = JsonUtils.TryDeserializeObject<Dictionary<string, object>>(textEditor.Text);
                if (dictionary != null)
                {
                    string json_text = JsonUtils.TrySerializeObject(dictionary);
                    if (!string.IsNullOrEmpty(json_text))
                        textEditor.Text = json_text;
                }

            }
        }

        private void FormatOriginText(object sender, RoutedEventArgs e)
        {
            FormatString(originTextBox);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextEditorOptions textEditorOptions = new TextEditorOptions();
            textEditorOptions.EnableHyperlinks = false;
            textEditorOptions.HighlightCurrentLine = true;
            originTextBox.Options = textEditorOptions;

            originTextBox.Text = ConfigManager.UrlEncodePage.OriginText;
            targetTextBox.Text = ConfigManager.UrlEncodePage.TargetText;

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveText();
        }

        private void SaveText()
        {

            ConfigManager.UrlEncodePage.OriginText = originTextBox.Text;
            ConfigManager.UrlEncodePage.TargetText = targetTextBox.Text;
            ConfigManager.UrlEncodePage.Save();
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
        }
    }
}
