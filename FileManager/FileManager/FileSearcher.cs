using FileManager.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileManager
{
    class FileSearcher
    {
        public FileSearcher()
        {

        }

        public async void Search(string patch, string pattern, string content, Button butt)
        {
            butt.IsEnabled = false;
            List<string> FilesList = await GetFiles(patch, pattern, content);
            FileSearchResult window = new FileSearchResult();
            foreach (var file in FilesList)
            {
                TreeViewItem fileitem = new TreeViewItem();
                fileitem.Header = file;
                fileitem.Tag = file;
                fileitem.KeyDown += FileOperations.FileClick;
                window.ResultTree.Items.Add(fileitem);
            }
            window.Show();
            butt.IsEnabled = true;
        }

        private Task<List<string>> GetFiles(string root, string pattern, string content)
        {
            Queue<string> folders = new Queue<string>();
            List<string> files = new List<string>();
            return Task.Run(() =>
            {
                folders.Enqueue(root);
                while (folders.Count != 0)
                {
                    string currentFolder = folders.Dequeue();
                    try
                    {
                        string[] filesInCurrent = Directory.GetFiles(currentFolder, pattern, SearchOption.TopDirectoryOnly);
                        foreach (var file in filesInCurrent)
                        {
                            if (FileContainsContent(file, content)) files.Add(file);
                        }
                    }
                    catch { }
                    try
                    {
                        string[] foldersInCurrent = Directory.GetDirectories(currentFolder, "*", SearchOption.TopDirectoryOnly);
                        foreach (string _current in foldersInCurrent)
                        {
                            folders.Enqueue(_current);
                        }
                    }
                    catch { }
                }
                return files;
            });
        }

        static bool FileContainsContent(string filepath, string content)
        {
            bool result;
            using(StreamReader reader = new StreamReader(filepath))
            {
                string filecontent = reader.ReadToEnd();
                string keytovalue = "";
                foreach (var key in AbbreviationDictionary.Abbreviations.Keys)
                {
                    if(content.Contains(key,StringComparison.OrdinalIgnoreCase))
                    {
                        string value = AbbreviationDictionary.Abbreviations.FirstOrDefault(x => x.Key == key).Value;
                        keytovalue = content.ReplaceInsensitive(key, value);
                    }
                }
                result = filecontent.Contains(content, StringComparison.OrdinalIgnoreCase)
                    || (filecontent.Contains(keytovalue, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(keytovalue));
            }
            return result;
        }
    }
}
