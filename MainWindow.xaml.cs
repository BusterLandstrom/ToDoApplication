using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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

        private UserControl currentControl; // This is just where the current view is stored

        public MainWindow()
        {
            InitializeComponent();

            // Initializes the controllers for the models and views (aka widgets and widget controllers)
            DataContext = this;

            Loaded += MainWindow_Loaded;
            App.accountManager.mainWindow = this;

        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load ToDoItems and statuses from the database
            await App.accountManager.InitializeCollections();
            LoadAllData(); // Loads all data into views and configures the views
        }

        public async Task UpdateView(int viewID) 
        {
            if (currentControl == null)
            {
                ((Storyboard)FindResource("AnimateWidgetTransition")).Begin(todoListView);
                WidgetPlacement.Children.Add(todoListView); // Change to loading view in the future
                currentControl = todoListView;
            }
            else 
            { 
                WidgetPlacement.Children.Clear();
                await App.accountManager.UpdateCollections();

                switch (viewID)
                {
                    case 0: // Change this to the loading view later
                        currentControl = todoListView;
                        ((Storyboard)FindResource("AnimateWidgetTransition")).Begin(currentControl);
                        WidgetPlacement.Children.Add(currentControl);
                        break;
                    case 1:
                        todoListView.GetDataContex().InitializeTodo();
                        currentControl = todoListView;
                        ((Storyboard)FindResource("AnimateWidgetTransition")).Begin(currentControl);
                        WidgetPlacement.Children.Add(currentControl);
                        break;
                    case 2:
                        newTaskView.GetDataContext().LoadIntoView();
                        currentControl = newTaskView;
                        ((Storyboard)FindResource("AnimateWidgetTransition")).Begin(currentControl);
                        WidgetPlacement.Children.Add(currentControl);
                        break;
                }
            }
        }

        private void LoadAllData()
        {

            ToDoItemsViewModel = new ToDoItemsViewModel();
            NewTaskViewModel = new NewTaskViewModel();
            todoListView = new ToDoListView();
            newTaskView = new NewTaskView();

            todoListView.SetDataContex(ToDoItemsViewModel);
            newTaskView.SetDataContext(NewTaskViewModel);

            UpdateView(1);
        }
    }
}