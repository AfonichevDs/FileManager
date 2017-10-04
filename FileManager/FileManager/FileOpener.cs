using FileManager.EntityClasses;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using FileManager.Extensions;

namespace FileManager
{
    class FileOpener
    {
        private EntityClasses.File File;
        public TextEditorWindow window { get; private set; }

        private string StartText;
        private string EditedText;
        TextRange doc;

        public FileOpener(EntityClasses.File file)
        {
            File = file;
            window = new TextEditorWindow();
            window.Show();
            window.SearchButt.Click += SearchWord;
            window.ClearTagButt.Click += ParseHtml;
            window.Closing += EditorShutDown;
        }

        public void Open(bool EnableSpellCheck = false)
        {
            window.MainContent.SpellCheck.IsEnabled = EnableSpellCheck;
            doc = new TextRange(window.MainContent.Document.ContentStart, window.MainContent.Document.ContentEnd);
            using (FileStream stream = new FileStream(File.Path, FileMode.Open))
            {
                doc.Load(stream, DataFormats.Text);
                StartText = doc.Text;
                window.MainContent.AppendAbbreviationText();
            }
        }

        private void SaveChanges(string path)
        {
            string content = new TextRange(window.MainContent.Document.ContentStart, window.MainContent.Document.ContentEnd).Text;
            try
            {
                StreamWriter writer = new StreamWriter(path);
                writer.Write(content);
                writer.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditorShutDown(object sender, CancelEventArgs e)
        {
            EditedText = doc.Text;
            if(EditedText!=StartText || StartText =="")
            {
                MessageBoxResult res = MessageBox.Show("Save Changes?", "Close File", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        SaveChanges(File.Path);
                        break;
                }
            }
        }

        private void ParseHtml(object sender, RoutedEventArgs e)
        {
            string html = doc.Text;
            window.MainContent.Document.Blocks.Clear();
            string pattern = String.Format(@"<{0}\s+([\s\S]*?)>([\s\S]*?)</{0}>", window.TagWordBox.Text);
            html = Regex.Replace(html, pattern, string.Empty, RegexOptions.IgnoreCase);
            pattern = String.Format(@"<{0}\s+([\s\S]*?)/?>", window.TagWordBox.Text);
            html = Regex.Replace(html, pattern, string.Empty, RegexOptions.IgnoreCase);
            window.MainContent.Document.Blocks.Add(new Paragraph(new Run(html)));
        }

        private void SearchWord(object sender, RoutedEventArgs e)
        {
            string content = doc.Text;
            int count = FileOperations.FindWordsQuantity(window.SearchWordBox.Text, content);
            window.Results.Text = count.ToString() + " раз(-и)";
        }
    }
}
