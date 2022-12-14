using SuperUtils.IO;
using SuperUtils.WPF.VisualTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SuperUtils.Security.Encrypt;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for Encrypt.xaml
    /// </summary>
    public partial class Encrypt : Page
    {
        private MainWindow mainWindow;



        public Encrypt()
        {
            InitializeComponent();
            mainWindow = WindowHelper.GetWindowByName("MainWindow", App.Current.Windows) as MainWindow;
        }

        private void CalcStringMd5(object sender, RoutedEventArgs e)
        {
            if (md5OriginTextBlock == null || stringComboBox == null || md5ResultTextBlock == null)
                return;
            if (string.IsNullOrEmpty(md5OriginTextBlock.Text))
                md5ResultTextBlock.Text = "";
            else
            {
                SHAType type = SuperUtils.Security.Encrypt.SHA_TYPES[stringComboBox.SelectedIndex];
                md5ResultTextBlock.Text = CalcHash(type, md5OriginTextBlock.Text);
            }
        }

        public static string CalcHash(SHAType shaType, string origin)
        {
            if (shaType == SHAType.MD5)
            {
                return SuperUtils.Security.Encrypt.CalculateMD5Hash(origin);
            }
            else
            {
                return SuperUtils.Security.Encrypt.SHA(shaType, origin);
            }
        }

        private void CopyMd5ResultText(object sender, RoutedEventArgs e)
        {
            SuperUtils.IO.ClipBoard.TrySetDataObject(md5ResultTextBlock.Text);
        }



        private void stringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems?.Count == 0) return;
            CalcStringMd5(null, null);
        }

        private void SelectHashFile(object sender, RoutedEventArgs e)
        {
            string filePath = FileHelper.SelectFile();
            if (!string.IsNullOrEmpty(filePath))
            {
                hashFilePathTextBlock.Text = filePath;
            }
        }
        private void CalcFileMd5(object sender, RoutedEventArgs e)
        {
            if (hashFilePathTextBlock == null || fileComboBox == null || md5FileResultTextBlock == null)
                return;
            if (!File.Exists(hashFilePathTextBlock.Text))
                md5FileResultTextBlock.Text = "";
            else
            {
                SHAType type = SuperUtils.Security.Encrypt.SHA_TYPES[fileComboBox.SelectedIndex];
                md5FileResultTextBlock.Text = CalcFileHash(type, hashFilePathTextBlock.Text);
            }
        }

        public static string CalcFileHash(SHAType shaType, string fileName)
        {
            if (shaType == SHAType.MD5)
            {
                return SuperUtils.Security.Encrypt.GetFileMD5(fileName);
            }
            else
            {
                return SuperUtils.Security.Encrypt.FileSHA(shaType, fileName);
            }
        }


        private void fileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcFileMd5(null, null);
        }

        private void CopyFileResultText(object sender, RoutedEventArgs e)
        {
            SuperUtils.IO.ClipBoard.TrySetDataObject(md5FileResultTextBlock.Text);
        }

        private void GenerateRandomPwd(object sender, RoutedEventArgs e)
        {
            string password = Membership.GeneratePassword((int)lenSlider.Value, 0);
            pwdTextBox.Text = password;
        }

        private void CopyPwdText(object sender, RoutedEventArgs e)
        {
            SuperUtils.IO.ClipBoard.TrySetDataObject(pwdTextBox.Text);
        }
    }
}
