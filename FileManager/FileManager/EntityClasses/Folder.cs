using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.EntityClasses
{
    class Folder : IExplorerObject
    {
        public string Name { get { return Path.Substring(Path.LastIndexOf("\\") + 1); } set { } }

        public string Path { get; set; }

        public Folder(string path)
        {
            Path = path;
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
            bool IsAlreadyFileExist = Directory.Exists(folder + "\\" + SourceName) && Buffer.getInstance().IsCuted;
            if (!String.IsNullOrEmpty(SourcePath) && !String.IsNullOrEmpty(SourceName))
            {
                try
                {
                    if (IsAlreadyFileExist)
                    {
                        MessageBoxResult res = MessageBox.Show("Файл з даним ім'ям вже існує, створити новий?", "", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                        {
                            string new_name = folder + "\\" + SourceName;
                            int i = 1;
                            while (Directory.Exists(new_name + i.ToString()))
                            {
                                i++;
                            }
                            if (Buffer.getInstance().IsCuted)
                            {
                                Directory.Move(SourcePath, new_name + i.ToString());
                            }
                        }
                    }
                    else if (Buffer.getInstance().IsCuted)
                    {
                        Directory.Move(SourcePath, folder + "\\" + SourceName);
                    }
                    else CopyTo(folder + "\\" + SourceName, true);
                }
                catch
                {

                }
            }
        }

        public void Delete()
        {
            MessageBoxResult res = MessageBox.Show("Ви впевнені що хочете видалити папку?", "Видалити", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
                Directory.Delete(Path,true);
        }

        private void CopyTo(string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    CopyTo(temppath, copySubDirs);
                }
            }
        }
    }
}
