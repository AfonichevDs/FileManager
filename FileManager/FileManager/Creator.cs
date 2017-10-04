using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileManager
{
    class Creator
    {
        public CreateFileWindow createWindow { get; private set; }

        private string NewName;
        private string CurrentFolder;

        public Creator(string currFolder)
        {
            CurrentFolder = currFolder;
            createWindow = new CreateFileWindow();
            createWindow.OkName.Click += SubmitName;
            createWindow.CancelName.Click += CancelClick;
            createWindow.Title += "  " + currFolder;
            createWindow.Show();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            createWindow.Close();
        }

        private void SubmitName(object sender, RoutedEventArgs e)
        {
            NewName = createWindow.FileNameBox.Text;
            if (((ComboBoxItem)createWindow.SelectBox.SelectedItem).Tag.ToString() == "0")
            {
                CreateFile(NewName);
            }
            else CreateFolder(NewName);
            createWindow.Close();
        }

        private void CreateFile(string name)
        {
            string fullName = CurrentFolder + "//" + name;
            if (File.Exists(fullName))
            {
                MessageBoxResult res = MessageBox.Show("Файл з даним ім'ям вже існує, створити новий?", "", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    int i = 1;
                    fullName = CurrentFolder + "//" + name.Substring(0, name.Length - Path.GetExtension(name).Length) + i.ToString() + Path.GetExtension(name);
                    while (File.Exists(NewName))
                    {
                        i++;
                        fullName = CurrentFolder + "//" + name.Substring(0, name.Length - Path.GetExtension(name).Length) + i.ToString() + Path.GetExtension(name);
                    }
                }
            }
            File.Create(fullName);
        }

        private void CreateFolder(string name)
        {
            string fullName = CurrentFolder + "//" + name;
            if (Directory.Exists(fullName))
            {
                MessageBoxResult res = MessageBox.Show("Папка з даним ім'ям вже існує, створити нову?", "", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    int i = 1;
                    fullName = CurrentFolder + "//" + name + i.ToString();
                    while (Directory.Exists(NewName))
                    {
                        i++;
                        fullName = CurrentFolder + "//" + name + i.ToString();
                    }
                }
                else Directory.Delete(fullName, true);
            }
            Directory.CreateDirectory(fullName);
        }
    }
}
