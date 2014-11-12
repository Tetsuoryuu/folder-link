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
        public ICommand AddSource { get { return new RelayCommand<List<String>>(AddDirectory, CanAddDirectory); } }

        public event PropertyChangedEventHandler PropertyChanged;

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


    }
}
