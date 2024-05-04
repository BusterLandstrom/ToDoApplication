using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml;
using ToDoApplication.Items;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ToDoApplication.ViewModels
{

    public interface IToDoViewModel
    {
        // This is just a commmon ground for all viewmodels (I might reconstruct this if need in the future to remove some redundancy)
    }

    public class ToDoItemsViewModel : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<IToDoViewModel> _items = new ObservableCollection<IToDoViewModel>();

        public ObservableCollection<IToDoViewModel> Items
        {
            get { return _items; }
            private set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public ICommand createTask {  get; set; }
        public ICommand editTask { get; set; }
        public ICommand toggleDone { get; set; }

        public ToDoEntryViewModel TaskEntry { get; } = new ToDoEntryViewModel();

        public ToDoItemsViewModel() 
        {
            createTask = new RelayTypeCommand<WatermarkTextBox>(CreateTask);
            editTask = new RelayTypeCommand<ToDoItem>(EditTask);
            toggleDone = new RelayTypeCommand<ToDoItem>(ToggleDone);

            InitializeTodo();        
        }

        public async void InitializeTodo() 
        {
            if (Items != null)
            {
                Items.Clear();
            }

            Items.Add(TaskEntry); // Just adds the entry "add new task" item to the view in the first line of the list

            var todoItemTask = await App.accountManager.GetToDoItems();
            var todoItemList = todoItemTask;

            InsertDataToView(todoItemList);

        }

        public void InsertDataToView(List<ToDoItem> itemList)
        {
            foreach (var item in itemList)
            {
                Items.Add(item);
            }

        }

        // Insert functions to compute and handle the management of the steps calculation etc for the items in Items

        public static async void CreateTask(WatermarkTextBox todoTextbox) 
        {
            if (todoTextbox != null)
            {
                string todoName = todoTextbox.Text;
                var statusItems = await App.accountManager.GetStatusItems();
                var todoStatus = statusItems[0].GetStatus(0); // Gets the "To do" status in the database (This should be made more flexible)
                App.accountManager.currentTodoitem = new ToDoItem
                {
                    Name = todoName,
                    Description = "",
                    Status = todoStatus,
                    Done = false,
                    Steps = new List<Step>(),
                    StepsDone = 0,
                    TotalSteps = 0
                };
                App.accountManager.mainWindow.UpdateView(2);
            }
        }

        public static void EditTask(ToDoItem item) 
        {
            App.accountManager.currentTodoitem = item;
            App.accountManager.mainWindow.UpdateView(2);
        }

        public async void ToggleDone(ToDoItem item)
        {
            foreach (var toDoItem in Items) 
            {
                if (toDoItem is ToDoItem && toDoItem == item) 
                {
                    await ((ToDoItem)toDoItem).ToggleDone();
                    // PropertyChanged raise for the item
                    OnPropertyChanged(nameof(Items));
                    break;
                }
            }
        }

    }
}
