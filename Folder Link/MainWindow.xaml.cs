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
        private CollectionView contentCV;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel.MainWindowViewModel();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> result = null;

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

        #region Content Filtering Logic
        private bool FilterContentView(object item)
        {
            string currentFilter = FilterTB.Text;
            if (string.IsNullOrEmpty(currentFilter))
                return true;
            else
                return (item as FileInfo).Name.Contains(currentFilter);
        }

        private void FilterTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (contentCV == null)
            {
                contentCV = (CollectionView)CollectionViewSource.GetDefaultView(contentListView.ItemsSource);
                contentCV.Filter = FilterContentView;
            }
            contentCV.Refresh();
        }

        #endregion

        #region Content Renaming Logic

        private bool EditMode = false;
        private FileInfo currentEditingItem;
        private TextBox currentTextBox;
        private Brush currentTextBoxOriginaBrush;
        private string currentTextBoxOriginalContent;

        private void RenameMI_Click(object sender, RoutedEventArgs e)
        {
            EditMode = true;
            currentTextBox.IsReadOnly = false;
            currentTextBoxOriginaBrush=currentTextBox.Background;
            currentTextBox.Background = new SolidColorBrush(Colors.Yellow);
            currentTextBox.Focus();
        }



        private void TextBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!EditMode)
            {
                currentTextBox = (sender as TextBox);
                currentTextBoxOriginalContent = currentTextBox.Text;
            }
            else
            {
                StopEditing();
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Rename();
            else if (e.Key == Key.Escape)
                StopEditing();
        }

        private void Rename()
        {
            string directory=(currentTextBox.DataContext as FileInfo).DirectoryName;
            string oldFileName = System.IO.Path.Combine(directory, currentTextBoxOriginalContent);
            string newFileName=System.IO.Path.Combine(directory,currentTextBox.Text);
            
            ((ViewModel.MainWindowViewModel)this.DataContext).Rename.Execute(Tuple.Create(oldFileName,newFileName));
            StopEditing();
        }

        private void StopEditing()
        {
            EditMode = false;
            currentTextBox.Background = currentTextBoxOriginaBrush;
            currentTextBox.Text = currentTextBoxOriginalContent;
            currentTextBox.IsReadOnly = true;
        }

        #endregion
    }


}
