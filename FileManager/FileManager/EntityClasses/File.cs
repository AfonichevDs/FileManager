
using System;
using System.Diagnostics;
using System.Windows;

namespace FileManager.EntityClasses
{
    interface IExplorerObject
    {
        string Name { get; set; }

        string Path { get; set; }

        void Copy();

        void Cut();

        void Delete();

        void Paste(string folder);
    }

    class File:IExplorerObject
    {
        public string Name { get { return Path.Substring(Path.LastIndexOf("\\") + 1); } set { } }

        public string Path { get; set; }

        public File(string path)
        {
            Path = path;
        }

        public virtual void Open()
        {
            Process.Start(Path);
        }

        public void Copy()
        {
            Buffer.getInstance().Push(this, false);
        }

        public void Cut()
        {
            Buffer.getInstance().Push(this, true);
        }

        public void Paste(string folder)
        {
            string SourceName = Name;
            string SourcePath = Path;
            bool IsAlreadyFileExist = System.IO.File.Exists(folder + "\\" + SourceName);
            if (!String.IsNullOrEmpty(SourcePath) && !String.IsNullOrEmpty(SourceName))
            {
                try
                {
                    if (IsAlreadyFileExist)
                    {
                        MessageBoxResult res = MessageBox.Show("Файл з даним ім'ям вже існує, створити новий?", "", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                        {
                            string new_name = folder + "\\" + SourceName.Substring(0, SourceName.Length - System.IO.Path.GetExtension(SourceName).Length);
                            int i = 1;
                            while (System.IO.File.Exists(new_name + "(" + i.ToString() + ")" + System.IO.Path.GetExtension(SourceName)))
                            {
                                i++;
                            }
                            if (Buffer.getInstance().IsCuted)
                            {
                                System.IO.File.Move(SourcePath, new_name +"("+i.ToString() +")" + System.IO.Path.GetExtension(SourceName));
                            }
                            else System.IO.File.Copy(SourcePath, new_name + "(" + i.ToString() + ")" + System.IO.Path.GetExtension(SourceName));
                        }
                    }
                    else if (Buffer.getInstance().IsCuted)
                    {
                        System.IO.File.Move(SourcePath, folder + "\\" + SourceName);
                    }
                    else System.IO.File.Copy(SourcePath, folder + "\\" + SourceName, true);
                }
                catch
                {

                }
            }
        }

        public void Delete()
        {
            MessageBoxResult res = MessageBox.Show("Ви впевнені що хочете видалити файл?", "Видалити", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
                System.IO.File.Delete(Path);
        }
    }

    abstract class FileFactory
    {
        public abstract File CreateFile(string path);
    }

    class HtmlFile : File
    {
        public HtmlFile(string path):base (path)
        {
            
        }
        public override void Open()
        {
            FileOpener opener = new FileOpener(this);
            opener.window.ClearHtml.IsEnabled = true;
            opener.Open();
        }
    }

    class HtmlFileFactory : FileFactory
    {
        public override File CreateFile(string path)
        {
            HtmlFile file = new HtmlFile(path);
            return file;
        }
    }

    class XmlFile : File
    {
        public XmlFile(string path):base (path)
        {

        }

        public override void Open()
        {
            FileOpener opener = new FileOpener(this);
            opener.Open();
        }
    }

    class XmlFileFactory : FileFactory
    {
        public override File CreateFile(string path)
        {
            XmlFile file = new XmlFile(path);
            return file;
        }
    }

    class TextFile : File
    {
        public TextFile(string path):base (path)
        {

        }
        public override void Open()
        {
            FileOpener opener = new FileOpener(this);
            opener.Open(true);
        }
    }

    class TextFileFactory : FileFactory
    {
        public override File CreateFile(string path)
        {
            TextFile file = new TextFile(path);
            return file;
        }
    }

    class FileClient
    {
        private File File;

        public FileClient(FileFactory fact, string path)
        {
            File = fact.CreateFile(path);
        }

        public void Open()
        {
            File.Open();
        }
    }

    
}
