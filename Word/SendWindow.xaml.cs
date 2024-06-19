using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Spire.Doc;
using Spire.Doc.Documents;

namespace Word
{
    public partial class SendWindow : Window
    {
        private readonly RichTextBox _richTextBox;

        public SendWindow(RichTextBox richTextBox)
        {
            InitializeComponent();
            InitializeWindowSettings();

            _richTextBox = richTextBox;
        }

        private void InitializeWindowSettings()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MinWidth = 550;
            this.MinHeight = 350;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string tempFilePath = SaveRichTextBoxContentToTempFile();
            if (!string.IsNullOrEmpty(tempFilePath))
            {
                SendEmailWithAttachment(Login.Text, Password.Password, Login_Friend.Text, Topic.Text, tempFilePath);
            }
        }

        private string SaveRichTextBoxContentToTempFile()
        {
            try
            {
                string tempFileName = Path.GetTempFileName();
                string tempFilePath = Path.ChangeExtension(tempFileName, ".docx"); 

                SaveAsDocx(tempFilePath);
                return tempFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void SaveAsDocx(string fileName)
        {
            
            Document document = new Document();

            Spire.Doc.Section section = document.AddSection();

            TextRange textRange = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);

            foreach (var block in _richTextBox.Document.Blocks)
            {
                if (block is Paragraph)
                {
                    string paragraphText = new TextRange(block.ContentStart, block.ContentEnd).Text;
                    section.AddParagraph().AppendText(paragraphText);
                }
            }
            
            document.SaveToFile(fileName, FileFormat.Docx);
        }

        private void SendEmailWithAttachment(string fromEmail, string password, string toEmail, string subject, string attachmentFilePath)
        {
            try
            {
                SmtpClient client = GetSmtpClient(fromEmail, password);

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = "The attachment contains your file.",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);
                mailMessage.Attachments.Add(new Attachment(attachmentFilePath));

                client.Send(mailMessage);

                MessageBox.Show("Email sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SmtpClient GetSmtpClient(string fromEmail, string password)
        {
            SmtpClient client = new SmtpClient("smtp.mail.ru", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            return client;
        }
    }
}
