using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Folder_Link.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Commands
        public ICommand AddSource { get { return new RelayCommand<List<String>>(AddDirectory, CanAddDirectory); } }
        public ICommand Rename { get { return new RelayCommand<Tuple<string,string>>(RenameFile, CanRenameFile); } }

        public ICommand Delete { get { return new RelayCommand<string>(DeleteFile, CanDeleteFile); } }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        #region Bindable Collections

        private ObservableCollection<DirectoryInfo> _sourcesList = new ObservableCollection<DirectoryInfo>();
        private ObservableCollection<FileInfo> _contentList = new ObservableCollection<FileInfo>();

        public ObservableCollection<DirectoryInfo> SourcesList
        {
            get { return _sourcesList; }
            set { _sourcesList = value; }
        }

        public ObservableCollection<FileInfo> ContentList
        {
            get { return _contentList; }
            set { _contentList = value; }

        }
        #endregion


        private void AddDirectory(List<String> directoriesPath)
        {
            foreach (var directoryPath in directoriesPath)
            {
                if(Directory.Exists(directoryPath))
                {
                    DirectoryInfo di = new DirectoryInfo(directoryPath);
                    _sourcesList.Add(di);
                    foreach (var file in di.GetFiles())
                        _contentList.Add(file);
                }
            }
        }

        private bool CanAddDirectory(List<String> directoriesPath)
        {
            return directoriesPath.Any(x => Directory.Exists(x));
        }

        private void RenameFile(Tuple<string,string> files)
        {

            string originalFileName = files.Item1;
            string newFileName = files.Item2;

            
            
            bool success = InputOutputOperations.Rename(originalFileName, newFileName);


            if (success)
            {
                //Update the current state of the FileInfo associated to the renamed file
                FileInfo file = _contentList.Single(x => x.FullName == originalFileName);
                int ind = _contentList.IndexOf(file);
                _contentList[ind] = new FileInfo(newFileName);
            }
            else
                throw new Exception("Impossible to Rename the File"); //TODO: Improve Exception handling

        }

        private bool CanRenameFile(Tuple<string,string> files)
        {
            string originalFileName = files.Item1;
            string newFileName = files.Item2;
            return _contentList.Any(x => x.Name == originalFileName) && string.IsNullOrEmpty(newFileName);
        }

        private void DeleteFile(string fileName)
        {
            bool success = InputOutputOperations.Delete(fileName);
            if (success)
            {
                FileInfo file = _contentList.Single(x => x.FullName == fileName);
                _contentList.Remove(file);
            }
            else
                throw new Exception("Impossible to Delete the File"); //TODO: Improve Exception handling
        }

        private bool CanDeleteFile(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}
