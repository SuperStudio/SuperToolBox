using SuperUtils.IO;
using SuperUtils.WPF.VisualTools;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static SuperUtils.Security.Encrypt;

namespace SuperToolBox.ToolPages
{
    /// <summary>
    /// Interaction logic for Encrypt.xaml
    /// </summary>
    public partial class Encrypt : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private MainWindow mainWindow;


        private bool _Calculating { get; set; }
        public bool Calculating
        {
            get { return _Calculating; }
            set
            {
                _Calculating = value;
                RaisePropertyChanged();
            }
        }

        public Encrypt()
        {
            InitializeComponent();
            mainWindow = WindowHelper.GetWindowByName("MainWindow", App.Current.Windows) as MainWindow;
            this.DataContext = this;
        }

        private async void CalcStringMd5(object sender, RoutedEventArgs e)
        {
            if (md5OriginTextBlock == null || stringComboBox == null || md5ResultTextBlock == null)
                return;
            if (string.IsNullOrEmpty(md5OriginTextBlock.Text))
                md5ResultTextBlock.Text = "";
            else
            {
                SHAType type = SuperUtils.Security.Encrypt.SHA_TYPES[stringComboBox.SelectedIndex];
                Calculating = true;
                string result = await CalcHash(type, md5OriginTextBlock.Text);
                md5ResultTextBlock.Text = result;
                Calculating = false;
            }
        }

        public static async Task<string> CalcHash(SHAType shaType, string origin)
        {
            return await Task.Run(() =>
            {
                if (shaType == SHAType.MD5)
                {
                    return SuperUtils.Security.Encrypt.TryGetMD5(origin);
                }
                else
                {
                    return SuperUtils.Security.Encrypt.SHA(shaType, origin);
                }

            });
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
        private async void CalcFileMd5(object sender, RoutedEventArgs e)
        {
            if (hashFilePathTextBlock == null || fileComboBox == null || md5FileResultTextBlock == null)
                return;
            if (!File.Exists(hashFilePathTextBlock.Text))
                md5FileResultTextBlock.Text = "";
            else
            {
                SHAType type = SuperUtils.Security.Encrypt.SHA_TYPES[fileComboBox.SelectedIndex];
                Calculating = true;
                string result = await CalcFileHash(type, hashFilePathTextBlock.Text);
                md5FileResultTextBlock.Text = result;
                Calculating = false;
            }
        }

        public static async Task<string> CalcFileHash(SHAType shaType, string fileName)
        {
            return await Task.Run(() =>
            {
                if (shaType == SHAType.MD5)
                {
                    return SuperUtils.Security.Encrypt.TryGetFileMD5(fileName);
                }
                else
                {
                    return SuperUtils.Security.Encrypt.FileSHA(shaType, fileName);
                }
            });
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

        private void hashFilePathTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcFileMd5(null, null);
        }

        private void md5OriginTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcStringMd5(null, null);
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            string[] dragdropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = dragdropFiles[0];
            if (!string.IsNullOrEmpty(file))
                hashFilePathTextBlock.Text = file;

        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true; // 必须加
        }


        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {

            ((sender as TextBox).Parent as Border).BorderBrush = (SolidColorBrush)Application.Current.Resources["Button.Selected.BorderBrush"];

        }

        private void TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            ((sender as TextBox).Parent as Border).BorderBrush = Brushes.Transparent;
        }
    }
}
