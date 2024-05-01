using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoApplication.ViewModels
{
    public class ToDoItemsViewModel : INotifyPropertyChanged
    { 
        // Define a collection to hold ToDoItemViewModel instances
        public ObservableCollection<ToDoItemsViewModel> Items { get; } = new ObservableCollection<ToDoItemsViewModel>();

        private string _name;
        private string _description;
        private Status _status;
        private bool _done;
        private List<Step> _steps;
        private int _stepsDone;
        private int _totalSteps;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public bool Done
        {
            get => _done;
            set
            {
                _done = value;
                OnPropertyChanged();
            }
        }

        public List<Step> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
                OnPropertyChanged();
            }
        }

        
        public int StepsDone
        {
            get => _stepsDone;
            set
            {
                _stepsDone = value;
                OnPropertyChanged();
            }
        }

        public int TotalSteps
        {
            get => _totalSteps;
            set
            {
                _totalSteps = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
