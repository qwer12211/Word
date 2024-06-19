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
            OpenRedactorWindow();
        }

        private void OpenWordDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OpenRedactorWindow(openFileDialog.FileName);
            }
        }

        private void OpenRedactorWindow(string filePath = null)
        {
            Redactor redactor = new Redactor();
            if (filePath != null)
            {
                redactor.LoadFile(filePath);
            }
            redactor.Show();
            this.Close();
        }
    }
}
