using System.Windows;
using System.IO;
using System.Windows.Markup;
using System.Security.Principal;
using System.Windows.Controls;
using System.ComponentModel;
using MyFile = FileManager.EntityClasses.File;
using FileManager.EntityClasses;
using System.Threading;
using System;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        ToolSide left;
        ToolSide right;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            left = new ToolSide(comboleft, leftFileBox, leftFileTree, leftFolderTree, leftSearchBox, leftSearchButt);
            right = new ToolSide(comboright, rightFileBox, rightFileTree, rightFolderTree, rightSearchBox,rightSearchButt);
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader("Help.txt");
            MessageBox.Show(reader.ReadToEnd());
            reader.Close();
        }

        private void ShowInfo(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader("AboutApp.txt");
            MessageBox.Show(reader.ReadToEnd());
            reader.Close();
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = (TreeViewItem)leftFileTree.SelectedItem ?? (TreeViewItem)rightFileTree.SelectedItem;
            if(selected!=null)
            {
                MyFile file = new MyFile(selected.Tag.ToString());
                file.Delete();
                left.FillFileTree(left.CurrentLocation);
                right.FillFileTree(right.CurrentLocation);
                return;
            }
            TreeViewItem lselected = (TreeViewItem)leftFolderTree.SelectedItem ?? (TreeViewItem)rightFolderTree.SelectedItem;
            if (lselected != null)
            {
                Folder folder = new Folder(lselected.Tag.ToString());
                folder.Delete();
                try
                {
                    TreeViewItem parent = ((TreeViewItem)lselected.Parent);
                    left.RefreshSubFolders(parent);
                }
                catch(InvalidCastException)
                {
                    left.FillFolderTree();
                }
                return;
            }
            TreeViewItem rselected = (TreeViewItem)rightFolderTree.SelectedItem;
            if (rselected != null)
            {
                Folder folder = new Folder(rselected.Tag.ToString());
                TreeViewItem parent = ((TreeViewItem)rselected.Parent);
                try
                {
                    right.RefreshSubFolders(parent);
                    folder.Delete();
                }
                catch(InvalidCastException)
                {
                    right.FillFolderTree();
                }
                return;
            }
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            left.Refresh();
            right.Refresh();
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = (TreeViewItem)leftFileTree.SelectedItem ?? (TreeViewItem)rightFileTree.SelectedItem;
            if(selected != null) {
                MyFile file = new MyFile(selected.Tag.ToString());
                file.Copy();
                return;
            }
            selected = (TreeViewItem)leftFolderTree.SelectedItem ?? (TreeViewItem)rightFolderTree.SelectedItem;
            if (selected != null)
            {
                Folder folder = new Folder(selected.Tag.ToString());
                folder.Copy();
                return;
            }
        }

        private void CutClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = (TreeViewItem)leftFileTree.SelectedItem ?? (TreeViewItem)rightFileTree.SelectedItem;
            if(selected != null)
            {
                MyFile file = new MyFile(selected.Tag.ToString());
                file.Cut();
                return;
            }
            selected = (TreeViewItem)leftFolderTree.SelectedItem ?? (TreeViewItem)rightFolderTree.SelectedItem;
            if(selected!=null)
            {
                new Folder(selected.Tag.ToString()).Cut();
            }
        }

        private void PasteClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem lselected = (TreeViewItem)leftFolderTree.SelectedItem;
            TreeViewItem rselected = (TreeViewItem)rightFolderTree.SelectedItem;
            if(lselected != null)
            {
                IExplorerObject obj = Buffer.getInstance().ExpObj;
                if(obj!=null)
                {
                    obj.Paste(lselected.Tag.ToString());
                    left.FillFileTree(left.CurrentLocation);
                    left.RefreshSubFolders(lselected);
                    return;
                }
            }
            if (rselected != null)
            {
                IExplorerObject obj = Buffer.getInstance().ExpObj;
                if (obj != null)
                {
                    obj.Paste(lselected.Tag.ToString());
                    right.FillFileTree(left.CurrentLocation);
                    right.RefreshSubFolders(lselected);
                    return;
                }
            }
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = (TreeViewItem)leftFolderTree.SelectedItem ?? (TreeViewItem)rightFolderTree.SelectedItem;
            if(selected != null) {
                Creator creator = new Creator(selected.Tag.ToString());
                creator.createWindow.Closing += (object Sender, CancelEventArgs E) => {
                    left.RefreshSubFolders(selected);
                    left.FillFileTree(left.CurrentLocation);
                    right.RefreshSubFolders(selected);
                    right.FillFileTree(right.CurrentLocation);
                };
            }
        }
    }
}
