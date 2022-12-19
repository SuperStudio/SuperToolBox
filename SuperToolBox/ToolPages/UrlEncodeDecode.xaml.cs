using ICSharpCode.AvalonEdit;
using Newtonsoft.Json;
using SuperControls.Style;
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
                MessageCard.Success("复制成功");
            }
        }

        private void CopyTargetText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(targetTextBox.Text)) return;
            if (ClipBoard.TrySetDataObject(targetTextBox.Text))
            {
                MessageCard.Success("复制成功");
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
            Border border = sender as Border;
            TextEditor textEditor = border.Child as TextEditor;
            if (e.Key == Key.F3)
            {
                textEditor.Text = textEditor.Text.Trim()
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");
            }
            else if (Keyboard.IsKeyDown(Key.F) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                // 格式化
                //textEditor.SyntaxHighlighting = null;
                //textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.HighlightingDefinitions;
                string text = textEditor.Text;
                Dictionary<string, object> dictionary = JsonUtils.TryDeserializeObject<Dictionary<string, object>>(textEditor.Text);
                if (dictionary != null)
                {
                    string json_text = JsonUtils.TrySerializeObject(dictionary, Formatting.Indented);
                    if (!string.IsNullOrEmpty(json_text))
                        textEditor.Text = json_text;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextEditorOptions textEditorOptions = new TextEditorOptions();
            textEditorOptions.EnableHyperlinks = false;
            textEditorOptions.HighlightCurrentLine = true;
            originTextBox.Options = textEditorOptions;
        }
    }
}
