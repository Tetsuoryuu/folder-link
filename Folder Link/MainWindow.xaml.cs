using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;

namespace Folder_Link
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel.MainWindowViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> result=null;

            var ofd = new CommonOpenFileDialog();
            ofd.EnsureFileExists = true;
            ofd.EnsurePathExists = true;
            ofd.EnsureReadOnly = false;
            ofd.IsFolderPicker = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                result = ofd.FileNames.ToList();
            }
            if (result != null)
                ((ViewModel.MainWindowViewModel)this.DataContext).AddSource.Execute(result);
        }

        private void SortContentButton_Click(object sender, RoutedEventArgs e)
        {
            contentListView.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        private void OpenMI_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as FileInfo;
            InputOutputOperations.OpenFile(selected.FullName);
        }

        private void OpenDirectoryMI_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as FileInfo;
            InputOutputOperations.OpenLocation(selected.FullName);
        }

  
        private void CopyMI_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as FileInfo;
            InputOutputOperations.Copy(selected.FullName);
        }

        
    }
}
