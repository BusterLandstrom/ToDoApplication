using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Input;
using System.Xml.Linq;
using ToDoApplication.Items;

namespace ToDoApplication.ViewModels
{
    public class NewTaskViewModel : INotifyPropertyChanged // I think I might refactor and rename all items to fit this porperty
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveTaskCommand { get; set; }
        public ICommand RemoveTaskCommand { get; set; }

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


        public NewTaskViewModel()
        {
            SaveTaskCommand = new RelayCommand(SaveOrAddTask);
            RemoveTaskCommand = new RelayCommand(RemoveTask);
        }



        public async void SaveOrAddTask()
        {
            App.accountManager.currentTodoitem.Name = TaskName;
            App.accountManager.currentTodoitem.Description = Description;
            App.accountManager.currentTodoitem.Status = SelectedStatus;
            App.accountManager.currentTodoitem.Done = App.accountManager.currentTodoitem.IsDone();
            App.accountManager.currentTodoitem.Steps = Steps;
            App.accountManager.currentTodoitem.StepsDone = App.accountManager.currentTodoitem.GetStepsDone();
            App.accountManager.currentTodoitem.TotalSteps = App.accountManager.currentTodoitem.GetStepsTotal();
            if (App.accountManager.ToDoItemExists())
            {
                await App.accountManager.GetToDoRepo().UpdateAsync(App.accountManager.currentTodoitem.Id, App.accountManager.currentTodoitem); // Waiting for the update of the todo item
            }
            else
            {
                await App.accountManager.GetToDoRepo().CreateAsync(App.accountManager.currentTodoitem); // Waiting for creation of the new Todo item into the list
            }
        }

        public async void RemoveTask() 
        {
            if (App.accountManager.ToDoItemExists())
            {
                await App.accountManager.GetToDoRepo().DeleteAsync(App.accountManager.currentTodoitem.Id); // Waiting for the update of the todo item it might be null but it technically couldnt be null if this view is visible but ill  need to handle it later anyway
            }
            else 
            {
                App.accountManager.currentTodoitem = null;
            }
        }
    }
}
