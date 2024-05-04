using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using ToDoApplication.Items;
using ToDoApplication.ViewModels;
using ToDoApplication.Views;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public ToDoItemsViewModel ToDoItemsViewModel { get; set; } // I might rename these to make them not clash with the classes of the same name
        public NewTaskViewModel NewTaskViewModel { get; set; } // -||-

        public ToDoListView todoListView {  get; set; }

        public NewTaskView newTaskView { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Initializes the controllers for the models and views (aka widgets and widget controllers)
            ToDoItemsViewModel = new ToDoItemsViewModel();
            NewTaskViewModel = new NewTaskViewModel();
            todoListView = new ToDoListView();
            newTaskView = new NewTaskView();
            DataContext = this;



            Loaded += MainWindow_Loaded;

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load ToDoItems and statuses from the database
            await App.accountManager.InitializeCollections();
            await LoadAllData(); // Loads all data into views and configures the views
        }

        private async Task LoadAllData()
        {

            // This is just waiting for the task to get all todo items and status items to finish and then converting them to a list useable for the observable list to control the contents of the views
            var statusItemTask = App.accountManager.GetStatusItems();
            var satusItemList = await statusItemTask;

            var todoItemTask = App.accountManager.GetToDoItems();
            var todoItemList = await todoItemTask;

            ToDoItemsViewModel.InsertDataToView(todoItemList); // I have a function to load the data into view here instead of variables like you see for the newtaskviewmodel because i initialize the todoitem list with a dummy "entry" item used to create new tasks


            // These might be changed to a single function or just handling the properties better
            NewTaskViewModel.StatusList = new ObservableCollection<Status>(satusItemList[0].GetAllStatuses()); // Getting an index of the obs collection might be incorrect since i need to get the index 0 to retrieve the obs collection -- Maybe change and imporve
            NewTaskViewModel.SelectedStatus = NewTaskViewModel.StatusList[0];
            // Set DataContext of NewTaskView to the NewTaskViewModel instance

            todoListView.SetDataContex(ToDoItemsViewModel);
            newTaskView.SetDataContext(NewTaskViewModel);

            // Add the default view to the widget placement item (Will be needed to change to be able to change the view)
            WidgetPlacement.Children.Add(todoListView);
            App.accountManager.SetHomeView(); // Set to loading view in the future once implemented
        }
    }
}