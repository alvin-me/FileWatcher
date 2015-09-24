using System;
using System.Windows.Input;

namespace FileWatcher
{
    class MainWindowViewModel : ViewModel
    {
        #region Private Variables
        /*The Variables are meant to be readonly as we mustnot change the address of any of them by creating new instances.
         *Problem with new istances is that since address changes the binding becomes invalid.
         *Instantiate all the variables in the constructor.
         */
        private bool _watching;
        private readonly ICommand _toggleWatchingCmd;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            _toggleWatchingCmd = new RelayCommand(ToggleWatching, CanToggleWatching);
        }

        #endregion

        #region Properties

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

        #endregion

        #region Commands

        /// <summary>
        /// Gets the AddPatientCommand. Used for Add patient Button Operations
        /// </summary>
        public ICommand ToggleWatchingCmd { get { return _toggleWatchingCmd; } }

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
        /// Add operation of the AddPatientCmd.
        /// Operation that will be performormed on the control click.
        /// </summary>
        /// <param name="obj"></param>
        public void ToggleWatching(object obj)
        {
            Watching = !Watching;
        }

        #endregion
    }
}
