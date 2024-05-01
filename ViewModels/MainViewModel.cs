using System.ComponentModel;
using System.Windows.Input;

namespace ToDoApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand SwitchToToDoListCommand { get; }
        public ICommand SwitchToNewTaskCommand { get; }
        public ICommand SwitchToEditTaskCommand { get; }

        public MainViewModel()
        {
            // Initialize commands
            SwitchToToDoListCommand = new RelayCommand(SwitchToToDoList);
            SwitchToNewTaskCommand = new RelayCommand(SwitchToNewTask);
            SwitchToEditTaskCommand = new RelayCommand(SwitchToEditTask);

            // Set default view
            SwitchToToDoList();
        }

        private void SwitchToToDoList()
        {
            CurrentViewModel = new ToDoItemsViewModel();
        }

        private void SwitchToNewTask()
        {
            //CurrentViewModel = new NewTaskViewModel();
        }

        private void SwitchToEditTask()
        {
            //CurrentViewModel = new EditTaskViewModel();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
