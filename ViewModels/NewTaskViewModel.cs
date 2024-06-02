using MongoDB.Bson;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using ToDoApplication.Items;
using Xceed.Wpf.Toolkit;

namespace ToDoApplication.ViewModels
{
    public class NewTaskViewModel : INotifyPropertyChanged // I think I might refactor and rename all items to fit this porperty
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveTaskCommand { get; set; }
        public ICommand RemoveTaskCommand { get; set; }
        public ICommand addStep { get; set; }
        public ICommand toggleStepDone { get; set; }
        public ICommand changeStepName { get; set; }
        public ICommand selectStep { get; set; }
        public ICommand removeStep { get; set; }

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

        private ObservableCollection<StepViewModel> _stepItems;

        public ObservableCollection<StepViewModel> StepItems 
        {
            get { return _stepItems; }
            set 
            {
                _stepItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepItems)));
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

        public StepEntry stepEntry { get; set; } = new StepEntry();

        public NewTaskViewModel()
        {
            SaveTaskCommand = new RelayCommand(SaveOrAddTask);
            RemoveTaskCommand = new RelayCommand(RemoveTask);
            toggleStepDone = new RelayTypeCommand<Step>(ToggleDoneStep);
            addStep = new RelayTypeCommand<WatermarkTextBox>(AddStep);
            selectStep = new RelayTypeCommand<Step>(SelectStep);
            removeStep = new RelayTypeCommand<Step>(RemoveStep);
            changeStepName = new RelayTypeCommand<TextBox>(ChangeStepName);

            InitializeNewTask();
        }

        public async void InitializeNewTask() 
        {
            // This is just waiting for the task to get all todo items and status items to finish and then converting them to a list useable for the observable list to control the contents of the views
            var statusItemTask = App.accountManager.GetStatusItems();
            var satusItemList = await statusItemTask;


            // These might be changed to a single function or just handling the properties better
            StatusList = new ObservableCollection<Status>(satusItemList[0].GetAllStatuses()); // Getting an index of the obs collection might be incorrect since i need to get the index 0 to retrieve the obs collection -- Maybe change and imporve
            SelectedStatus = StatusList[0];
        }

        public void LoadIntoView()
        {
            TaskName = App.accountManager.currentTodoitem.Name;
            Description = App.accountManager.currentTodoitem.Description;
            SelectedStatus = App.accountManager.currentTodoitem.Status;
            Steps = App.accountManager.currentTodoitem.Steps;
            StepItems = InitializeSteps();
        }

        public ObservableCollection<StepViewModel> InitializeSteps()
        {
            if (StepItems != null) 
            {
                StepItems.Clear();
            }
            var stepList = new ObservableCollection<StepViewModel>() { stepEntry };
            foreach (var step in Steps)
            {
                stepList.Add(step);
            }
            return stepList;
        }

        public async void ToggleDoneStep(Step step) 
        {
            foreach (var stepEntry in Steps)
            {
                if (step.Id == stepEntry.Id) 
                {
                    stepEntry.ToggleDone();
                    break;
                }
            }
        }

        public async void SelectStep(Step selectedStep)
        {
            App.accountManager.currentStepitem = selectedStep;
        }

        // The two adjacent functions are for redundancy so you can save steps by pressing enter on them and close the window without having to save the task
        public async void ChangeStepName(TextBox stepNameTextbox)
        {
            App.accountManager.currentStepitem.StepName = stepNameTextbox.Text;

            var currentTodoitem = App.accountManager.currentTodoitem;
            for (int i = 0; i < currentTodoitem.Steps.Count; i++)
            {
                if (currentTodoitem.Steps[i].Id == App.accountManager.currentStepitem.Id)
                {
                    currentTodoitem.Steps[i] = App.accountManager.currentStepitem;
                    await App.accountManager.GetToDoRepo().UpdateAsync(App.accountManager.currentTodoitem.Id, App.accountManager.currentTodoitem); // Waiting for the update of the todo item
                    break;
                }
            }
        }

        public async void AddStep(WatermarkTextBox stepTextbox) 
        {

            var statusItems = await App.accountManager.GetStatusItems();
            var doneStatus = statusItems[0];
            var databaseDoneStatus = doneStatus.GetStatus(0); // This is just the "To do" status from the database it is bad to do it this way not valid at all so I need to fix it later!!
            Status newStepStatus = databaseDoneStatus;
            Step newStep = new Step()
            {
                Id = ObjectId.GenerateNewId(),
                StepName = stepTextbox.Text,
                StepDone = false,
                StepStatus = newStepStatus
            };

            App.accountManager.currentTodoitem.Steps.Add(newStep);
            App.accountManager.currentTodoitem.UpdateItem();
            StepItems = InitializeSteps();
        }

        public async void RemoveStep(Step step)
        {
            var currentTodoitem = App.accountManager.currentTodoitem;
            for (int i = 0; i < currentTodoitem.Steps.Count; i++)
            {
                if (currentTodoitem.Steps[i].Id == step.Id)
                {
                    currentTodoitem.Steps.RemoveAt(i);
                    await App.accountManager.GetToDoRepo().UpdateAsync(App.accountManager.currentTodoitem.Id, App.accountManager.currentTodoitem); // Waiting for the update of the todo item
                    App.accountManager.mainWindow.UpdateView(3);
                    break;
                }
            }
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

            App.accountManager.mainWindow.UpdateView(1);
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

            App.accountManager.mainWindow.UpdateView(1); // Changes to regular view
        }
    }
}
