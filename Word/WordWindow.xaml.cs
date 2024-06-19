using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Spire.Doc;
using Spire.Doc.Documents;

namespace Word
{
    public partial class WordWindow : Window
    {
        public WordWindow()
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

        public void LoadFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();
            switch (fileExtension)
            {
                case ".rtf":
                    LoadRtfFile(filePath);
                    break;
                case ".docx":
                    LoadDocxFile(filePath);
                    break;
                default:
                    ShowErrorMessage("Unsupported file format");
                    break;
            }
        }

        private void LoadRtfFile(string filePath)
        {
            TextRange range = new TextRange(RichTextBox.Document.ContentStart, RichTextBox.Document.ContentEnd);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void LoadDocxFile(string filePath)
        {
            Document document = new Document();
            document.LoadFromFile(filePath);

            RichTextBox.Document.Blocks.Clear();
            foreach (Section section in document.Sections)
            {
                foreach (Spire.Doc.Documents.Paragraph paragraph in section.Paragraphs)
                {
                    AddParagraphToRichTextBox(paragraph);
                }
            }
        }

        private void AddParagraphToRichTextBox(Spire.Doc.Documents.Paragraph spireParagraph)
        {
            System.Windows.Documents.Paragraph newParagraph = new System.Windows.Documents.Paragraph();

            foreach (DocumentObject docObject in spireParagraph.ChildObjects)
            {
                if (docObject is Spire.Doc.Fields.TextRange textRange)
                {
                    Run run = CreateRunFromTextRange(textRange);
                    newParagraph.Inlines.Add(run);
                }
            }

            RichTextBox.Document.Blocks.Add(newParagraph);
        }

        private Run CreateRunFromTextRange(Spire.Doc.Fields.TextRange textRange)
        {
            Run run = new Run(textRange.Text)
            {
                FontWeight = textRange.CharacterFormat.Bold ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = textRange.CharacterFormat.Italic ? FontStyles.Italic : FontStyles.Normal,
                TextDecorations = textRange.CharacterFormat.UnderlineStyle != UnderlineStyle.None ? TextDecorations.Underline : null
            };
            return run;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|Word Document (*.docx)|*.docx|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileExtension = Path.GetExtension(saveFileDialog.FileName).ToLower();
                switch (fileExtension)
                {
                    case ".rtf":
                        SaveAsRtf(saveFileDialog.FileName);
                        break;
                    case ".docx":
                        SaveAsDocx(saveFileDialog.FileName);
                        break;
                    default:
                        ShowErrorMessage("Unsupported file format");
                        break;
                }
            }
        }

        private void SaveAsRtf(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                TextRange range = new TextRange(RichTextBox.Document.ContentStart, RichTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
            ShowSuccessMessage("Файл успешно сохранен как RTF!");
        }

        private void SaveAsDocx(string fileName)
        {
            Document document = new Document();
            Section section = document.AddSection();

            foreach (var block in RichTextBox.Document.Blocks)
            {
                if (block is System.Windows.Documents.Paragraph)
                {
                    string paragraphText = new TextRange(block.ContentStart, block.ContentEnd).Text;
                    section.AddParagraph().AppendText(paragraphText);
                }
            }

            document.SaveToFile(fileName, FileFormat.Docx);
            ShowSuccessMessage("Файл успешно сохранен как DOCX!");
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
