using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileManager.EntityClasses;
using System.Windows.Input;

namespace FileManager
{
    class ToolSide
    {
        private object dummyNode = null;

        public ComboBox DriversBox { get; set; }
        public ComboBox FileFormatBox { get; set; }
        public TreeView FolderTree { get; set; }
        public TreeView FileTree { get; set; }
        public TextBox SearchBox { get; set; }
        public Button SearchButton { get; set; }
        public List<FileFormat> FileFormats { get; private set; }
        public string CurrentLocation { get; private set; }

        public TreeViewItem SelectedItem {
            get
            {
                return (TreeViewItem)(FolderTree.SelectedItem ?? FileTree.SelectedItem);
            }
        }

        public ToolSide(ComboBox drbox, ComboBox filebox, TreeView fileTree, TreeView folderTree, TextBox searchBox, Button searchButton)
        {
            FileFormats = new List<FileFormat>();
            DriversBox = drbox;
            FileTree = fileTree;
            FolderTree = folderTree;
            FileFormatBox = filebox;
            SearchBox = searchBox;
            SearchButton = searchButton;
            SearchButton.Click += SearchClick;

            FillFormatBox();
            FillDriverBox();
            FillFolderTree(); ;
            FillFileTree(CurrentLocation);
        }

        public void FillDriverBox()
        {
            List<string> titles = new List<string>();
            foreach (var item in Directory.GetLogicalDrives())
            {
                titles.Add(item);
            }
            DriversBox.ItemsSource = titles;
            DriversBox.SelectedItem = DriversBox.Items.GetItemAt(0);
            if (String.IsNullOrEmpty(CurrentLocation)) CurrentLocation = DriversBox.SelectedItem.ToString();
            DriversBox.SelectionChanged += DriverChange;
        }

        public void FillFolderTree()
        {
            FolderTree.Items.Clear();
            foreach (string s in Directory.GetDirectories(DriversBox.SelectedItem.ToString()))
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.Items.Add(dummyNode);
                item.Expanded += FolderExpanded;
                item.MouseDoubleClick += FolderExpanded;
                item.KeyDown += FolderClick;
                FolderTree.Items.Add(item);
            }
        }

        public void FillFileTree(string path)
        {
            string format = "";
            FileTree.Items.Clear();
            GetCurrentExtension(ref format);
            foreach (var f in FileFormats)
            {
                if (f.FullName == FileFormatBox.SelectedItem.ToString())
                {
                    format = f.Format;
                }
            }
            try
            {
                foreach (var file in Directory.GetFiles(path, format, SearchOption.TopDirectoryOnly))
                {
                    TreeViewItem fileitem = new TreeViewItem();
                    fileitem.Header = file.Substring(file.LastIndexOf("\\") + 1);
                    fileitem.Tag = file;
                    fileitem.KeyDown += FileOperations.FileClick;
                    FileTree.Items.Add(fileitem);
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Выбранная вами папка удалена или не существует");
            }
        }

        public void FillFormatBox()
        {
            FileFormats.Add(EntityClasses.FileFormats.TextFormat);
            FileFormats.Add(EntityClasses.FileFormats.XmlFormat);
            FileFormats.Add(EntityClasses.FileFormats.HtmlFormat);
            FileFormats.Add(EntityClasses.FileFormats.AnyFormat);
            FileFormatBox.ItemsSource = from f in FileFormats
                                     select f.FullName;
            FileFormatBox.SelectedItem = FileFormatBox.Items.GetItemAt(0);
            FileFormatBox.SelectionChanged += FormatChange;
        }

        public string GetContentToSearch()
        {
            return SearchBox.Text;
        }

        public void Refresh()
        {
            CurrentLocation = DriversBox.SelectedItem.ToString();
            FillDriverBox();
            FillFolderTree();
            FillFileTree(CurrentLocation);
        }

        public void RefreshSubFolders(TreeViewItem item)
        {
            item.Items.Clear();
            foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
            {
                TreeViewItem subitem = new TreeViewItem();
                subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                subitem.Tag = s;
                subitem.Items.Add(dummyNode);
                subitem.Expanded += FolderExpanded;
                subitem.MouseDoubleClick += FolderExpanded;
                subitem.KeyDown += FolderClick;
                item.Items.Add(subitem);
            }
        }

        private void DriverChange(object sender, SelectionChangedEventArgs e)
        {
            CurrentLocation = DriversBox.SelectedItem.ToString();
            FillFolderTree();
            FillFileTree(((ComboBox)sender).SelectedItem.ToString());
        }

        private void FolderExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            CurrentLocation = item.Tag.ToString();
            item.Items.Clear();
            try
            {
                FileTree.Items.Clear();
                FillFileTree(CurrentLocation);
                foreach (string s in Directory.GetDirectories(CurrentLocation))
                {
                    TreeViewItem subitem = new TreeViewItem();
                    subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                    subitem.Tag = s;
                    subitem.Items.Add(dummyNode);
                    subitem.Expanded += FolderExpanded;
                    subitem.MouseDoubleClick += FolderExpanded;
                    subitem.KeyDown += FolderClick;
                    item.Items.Add(subitem);
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormatChange(object sender, SelectionChangedEventArgs e)
        {
            FillFileTree(CurrentLocation);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            string content = SearchBox.Text;
            if (!String.IsNullOrEmpty(content))
            {
                FileSearcher searcher = new FileSearcher();
                string format = "";
                GetCurrentExtension(ref format);
                if (format == "*.txt" || format == "*.html" || format == "*.xml")
                    searcher.Search(CurrentLocation ?? DriversBox.SelectedItem.ToString(), format, content, (Button)sender);
                else MessageBox.Show("Поиск по заданым параметрам невозможен");
            }
        }

        private void GetCurrentExtension(ref string format)
        {
            foreach (var f in FileFormats)
            {
                if (f.FullName == FileFormatBox.SelectedItem.ToString())
                {
                    format = f.Format;
                }
            }
        }

        public void FolderClick(object sender, KeyEventArgs e)
        {
            string folder = ((TreeViewItem)sender).Tag.ToString();
            if (e.Key == Key.F6)
            {
                IExplorerObject obj = Buffer.getInstance().ExpObj;
                if (obj != null)
                {
                    obj.Paste(folder);
                    FillFileTree(CurrentLocation);
                    RefreshSubFolders((TreeViewItem)sender);
                }
            }
            if (e.Key == Key.F5)
            {
                new Folder(folder).Copy();
            }
            if (e.Key == Key.F4)
            {
                new Folder(folder).Cut();
            }
            if (e.Key == Key.F8)
            {
                new Folder(folder).Delete();
                var parent = (TreeViewItem)((TreeViewItem)sender).Parent;
                RefreshSubFolders(parent);
                FileTree.Items.Clear();
            }
            e.Handled = true;
        }
    }
}
