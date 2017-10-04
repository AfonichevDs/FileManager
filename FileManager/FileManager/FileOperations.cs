using FileManager.EntityClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyFile = FileManager.EntityClasses.File;

namespace FileManager
{
    static class FileOperations
    {
        public static void FileClick(object sender, KeyEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            string path = item.Tag.ToString();
            if (e.Key == Key.F3)
            {
                string ext = Path.GetExtension(path);
                FileFactory factory;
                FileClient client;
                switch (ext)
                {
                    case ".txt":
                        factory = new TextFileFactory();
                        client = new FileClient(factory, path);
                        client.Open();
                        break;
                    case ".html":
                        factory = new HtmlFileFactory();
                        client = new FileClient(factory, path);
                        client.Open();
                        break;
                    case ".xml":
                        factory = new XmlFileFactory();
                        client = new FileClient(factory, path);
                        client.Open();
                        break;
                    default:
                        try
                        {
                            Process.Start(path);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                }
                e.Handled = true;
            }
            if (e.Key == Key.F8)
            {
                new MyFile(path).Delete();
            }
            if (e.Key == Key.F5)
            {
                new MyFile(path).Copy();
            }
            if (e.Key == Key.F4)
            {
                new MyFile(path).Cut();
            }
        }

        public static int FindWordsQuantity(string word, string content)
        {
            List<string> words = Regex.Split(content, @"[.,!?;:]?\s+[.,!?;:]?").ToList();
            int result = (from t in words
                          where t == word select t).Count();
            return result;
        }
    }
}
