using Microsoft.Win32;
using System.Windows;

namespace Word
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWindowSettings();
        }

        private void InitializeWindowSettings()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MinWidth = 560;
            this.MinHeight = 600;
        }

        private void CreateWordDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenWordWindow();
        }

        private void OpenWordDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OpenWordWindow(openFileDialog.FileName);
            }
        }

        private void OpenWordWindow(string filePath = null)
        {
            WordWindow wordWindow = new WordWindow();
            if (filePath != null)
            {
                wordWindow.LoadFile(filePath);
            }
            wordWindow.Show();
            this.Close();
        }
    }
}
