using System;
using System.Windows.Input;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows;

namespace FileWatcher
{
    class MainWindowViewModel : ViewModel
    {
        #region Private Variables

        private bool _watching;
        private string[] _watchingFilter;
        private string _message;
        private string _messageBackup;
        /*The Variables are meant to be readonly as we mustnot change the address of any of them by creating new instances.
         *Problem with new istances is that since address changes the binding becomes invalid.
         *Instantiate all the variables in the constructor.
         */
        private readonly FileInformation _domObject;
        private readonly ObservableCollection<FileInformation> _informations;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly ICommand _toggleWatchingCmd;
        private readonly ICommand _openSettingWindowCmd;
        private readonly ICommand _clearCollectionCmd;
        private readonly ICommand _saveCollectionCmd;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            _domObject = new FileInformation();
            _informations = new ObservableCollection<FileInformation>();
            _toggleWatchingCmd = new RelayCommand(ToggleWatching, CanToggleWatching);
            _openSettingWindowCmd = new RelayCommand(OpenSettingWindow, CanOpenSettingWindow);
            _clearCollectionCmd = new RelayCommand(ClearCollection, CanClearCollection);
            _saveCollectionCmd = new RelayCommand(SaveCollection, CanSaveCollection);
            _fileSystemWatcher = createWatcher();
            updateWatcherFromConfigFile();
            LogInfo("未开始监听");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets File Id. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public int Id
        {
            get { return _domObject.Id; }
            set
            {
                _domObject.Id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or Sets File wathcing TimeStamp. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public string TimeStamp
        {
            get { return _domObject.TimeStamp; }
            set
            {
                _domObject.TimeStamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }

        /// <summary>
        /// Gets or Sets Watching event type of the File. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public string EventType
        {
            get { return _domObject.EventType; }
            set
            {
                _domObject.EventType = value;
                OnPropertyChanged("EventType");
            }
        }

        /// <summary>
        /// Gets or Sets Watching File name. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public string FileName
        {
            get { return _domObject.FileName; }
            set
            {
                _domObject.FileName = value;
                OnPropertyChanged("FileName");
            }
        }

        /// <summary>
        /// Gets or Sets Watching File Path. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public string FilePath
        {
            get { return _domObject.FilePath; }
            set
            {
                _domObject.FilePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        /// <summary>
        /// Gets the FileInformations. Used to maintain the File Information List.
        /// Since this observable collection it makes sure all changes will automatically reflect in UI 
        /// as it implements both CollectionChanged, PropertyChanged;
        /// </summary>
        public ObservableCollection<FileInformation> FileInformations { get { return _informations; } }

        /// <summary>
        /// Sets the Selected File Information. Used to identify the selected File Information from the list. 
        /// </summary>
        public FileInformation SelectedFileInformation
        {
            set
            {
                Id = value.Id;
                TimeStamp = value.TimeStamp;
                EventType = value.EventType;
                FileName = value.FileName;
                FilePath = value.FilePath;
            }
        }

        /// <summary>
        /// Gets or Sets Watching state. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public bool Watching
        {
            get { return _watching; }
            set
            {
                if (_watching != value)
                {
                    _watching = value;
                    OnPropertyChanged("Watching");
                }
            }
        }

        /// <summary>
        /// Gets or Sets Log Message. Ready to be binded to UI.
        /// Impelments INotifyPropertyChanged which enables the binded element to refresh itself whenever the value changes.
        /// </summary>
        public string LogMsg
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("LogMsg");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the ToggleWatchingCmd. Used for toggle watching Operations
        /// </summary>
        public ICommand ToggleWatchingCmd { get { return _toggleWatchingCmd; } }

        /// <summary>
        /// Gets the OpenSettingWindowCmd. Used for Open setting window Operations
        /// </summary>
        public ICommand OpenSettingWindowCmd { get { return _openSettingWindowCmd; } }

        /// <summary>
        /// Gets the ClearCollectionCmd. Used for Open setting window Operations
        /// </summary>
        public ICommand ClearCollectionCmd { get { return _clearCollectionCmd; } }

        /// <summary>
        /// Gets the SaveCollectionCmd. Used for Open setting window Operations
        /// </summary>
        public ICommand SaveCollectionCmd { get { return _saveCollectionCmd; } }

        /// <summary>
        /// CanToggleWatching operation of the ToggleWatching.
        /// Tells us if the control is to be enabled or disabled.
        /// This method will be fired repeatedly as long as the view is open.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanToggleWatching(object obj)
        {
            return true;
        }

        /// <summary>
        /// Add operation of the ToggleWatchingCmd.
        /// Operation that will be performormed on the control click.
        /// </summary>
        /// <param name="obj"></param>
        public void ToggleWatching(object obj)
        {
            Watching = !Watching;
            if (Watching)
            {
                _fileSystemWatcher.EnableRaisingEvents = true;
                LogInfo("正在监听...");
            }
            else
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                LogInfo("未开始监听");
            }
        }

        /// <summary>
        /// CanOpenSettingWindow operation of the OpenSettingWindow.
        /// Tells us if the control is to be enabled or disabled.
        /// This method will be fired repeatedly as long as the view is open.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanOpenSettingWindow(object obj)
        {
            return true;
        }

        /// <summary>
        /// Add operation of the OpenSettingWindowCmd.
        /// Operation that will be performormed on the control click.
        /// </summary>
        /// <param name="obj"></param>
        public void OpenSettingWindow(object obj)
        {
            SettingWindow window = new SettingWindow();
            if (window.ShowDialog() == true)
                updateWatcherFromConfigFile();
        }

        /// <summary>
        /// CanClearCollection operation of the ClearCollection.
        /// Tells us if the control is to be enabled or disabled.
        /// This method will be fired repeatedly as long as the view is open.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanClearCollection(object obj)
        {
            return FileInformations.Count > 0;
        }

        /// <summary>
        /// Add operation of the ClearCollectionCmd.
        /// Operation that will be performormed on the control click.
        /// </summary>
        /// <param name="obj"></param>
        public void ClearCollection(object obj)
        {
            FileInformations.Clear();
            ResetFileInformation();
        }

        /// <summary>
        /// CanSaveCollection operation of the SaveCollection.
        /// Tells us if the control is to be enabled or disabled.
        /// This method will be fired repeatedly as long as the view is open.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanSaveCollection(object obj)
        {
            return FileInformations.Count > 0;
        }

        /// <summary>
        /// Add operation of the SaveCollectionCmd.
        /// Operation that will be performormed on the control click.
        /// </summary>
        /// <param name="obj"></param>
        public void SaveCollection(object obj)
        {
            StreamWriter sw = null;
            try
            {
                string fileName = Properties.Settings.Default.LogFolder + "\\" + DateTime.Now.ToString("yyyyMMdd hh-mm-ss") + ".log";
                sw = new StreamWriter(fileName);
                foreach (var item in FileInformations)
                {
                    string str = "[" + item.Id + "][" + item.EventType + "][" + item.FileName + "][" + item.FilePath + "]";
                    sw.WriteLine(str);
                }
                LogInfo("日志已保存到" + fileName, 5000);
            }
            catch (Exception e)
            {
                LogInfo(e.Message + "," + e.StackTrace);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create FileSystemWatcher
        /// </summary>
        /// <returns></returns>
        private FileSystemWatcher createWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            watcher.Created += new FileSystemEventHandler(this.fileSystemWatching);
            watcher.Deleted += new FileSystemEventHandler(this.fileSystemWatching);
            watcher.Changed += new FileSystemEventHandler(this.fileSystemWatching);
            watcher.Renamed += new RenamedEventHandler(this.fileSystemWatching);
            return watcher;
        }

        /// <summary>
        /// Update FileSystemWatcher from Config File
        /// </summary>
        private void updateWatcherFromConfigFile()
        {
            this._fileSystemWatcher.IncludeSubdirectories = Properties.Settings.Default.WatchSubDir;
            this._fileSystemWatcher.Path = Properties.Settings.Default.WatchFolder;

            if (MainWindowModel.FileTypeDict.ContainsKey(Properties.Settings.Default.WatchFileType))
            {
                string value = MainWindowModel.FileTypeDict[Properties.Settings.Default.WatchFileType];
                this._watchingFilter = value.Split(';');
            }
        }

        /// <summary>
        /// FileSystemEventHandler Delegate
        /// </summary>
        private void fileSystemWatching(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                string[] strArr = e.FullPath.Split(new char[] { '\\' });
                if (strArr.Length < 2 || strArr[1].Equals("$RECYCLE.BIN"))
                    return;

                //Always create a new instance of patient before adding. 
                //Otherwise we will endup sending the same instance that is binded, to the BL which will cause complications
                var information = new FileInformation { Id = FileInformations.Count, 
                    TimeStamp = DateTime.Now.ToString(), 
                    EventType = e.ChangeType.ToString(), 
                    FileName = strArr[strArr.Length - 1], 
                    FilePath = e.FullPath };
                FileInformations.Add(information);
            }));
        }

        /// <summary>
        /// Resets the FileInformation properties which will in turn auto reset the UI aswell
        /// </summary>
        private void ResetFileInformation()
        {
            Id = 0;
            TimeStamp = string.Empty;
            EventType = string.Empty;
            FileName = string.Empty;
            FilePath = string.Empty;
        }

        /// <summary>
        /// Set LogMsg properties which will in turn auto reset statusbar
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="delay">delay time of message rollback</param>
        private void LogInfo(string msg, int delay = 0)
        {
            _messageBackup = LogMsg;
            LogMsg = msg;

            // set delay timer to roll back message.
            if (delay > 0)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(delay);
                    LogInfo(_messageBackup);
                });
            }
        }

        #endregion
    }
}
