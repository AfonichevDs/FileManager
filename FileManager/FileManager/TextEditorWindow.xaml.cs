using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для TextEditorWindow.xaml
    /// </summary>
    public partial class TextEditorWindow : Window
    {
        public TextEditorWindow()
        {
            InitializeComponent();
            MainContent.Document.LineHeight = 1;
            SearchWordBox.TextChanged += ClearResultBlock;
        }

        private void ClearResultBlock(object sender, TextChangedEventArgs e)
        {
            Results.Text = "";
        }
    }
}
