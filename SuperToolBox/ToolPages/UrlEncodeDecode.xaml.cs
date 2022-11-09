using SuperControls.Style;
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
    }
}
