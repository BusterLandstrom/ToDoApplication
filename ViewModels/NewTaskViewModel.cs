using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using ToDoApplication.Items;

namespace ToDoApplication.ViewModels
{
    public class NewTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ToDoItemRepository todoItemRepository { get; set; }

        public ICommand AddTaskCommand { get; set; }

        private ObservableCollection<Status> _statusList;
        public ObservableCollection<Status> StatusList
        {
            get { return _statusList; }
            set
            {
                _statusList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusList)));
            }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
            }
        }

        private string _taskName;
        public string TaskName
        {
            get { return _taskName; }
            set
            {
                _taskName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskName)));
            }
        }


        private string _description;

        public string Description 
        {
            get { return _description; }
            set 
            {
                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        private List<Step> _steps;

        public List<Step> Steps 
        {
            get { return _steps; }
            set 
            {
                _steps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Steps)));
            }
        }


        public NewTaskViewModel( ToDoItemRepository repo)
        {
            todoItemRepository = repo;
            AddTaskCommand = new RelayCommand(AddTask);

        }

        public void AddTask()
        {
            var newTodo = new ToDoItem
            {
                Name = TaskName,
                Description = Description,
                Status = SelectedStatus,
                Done = false,
                Steps = new List<Step>() // This is just creating an empty steps i need to create the steps items first :)

            };

            todoItemRepository.CreateAsync(newTodo);
        }
    }
}
