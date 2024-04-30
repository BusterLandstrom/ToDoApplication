using System.Windows;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly ToDoItemRepository todoItemRepository;
        private readonly StatusItemRepository statusItemRepository;
        public MainWindow()
        {
            InitializeComponent();


            todoItemRepository = new ToDoItemRepository(App.Database);
            statusItemRepository = new StatusItemRepository(App.Database);

            Loaded += MainWindow_Loaded;

        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load ToDoItems from the database and display them
            await LoadToDoItems();
            await LoadStatusItems();
        }

        private async Task LoadToDoItems()
        {
            try
            {
                // Retrieve all ToDoItems
                var allItems = await todoItemRepository.GetAllAsync();

                // Display ToDoItems in the UI (e.g., in a ListBox or DataGrid)
                foreach (var item in allItems)
                {
                    // Add logic here to display ToDoItems in your UI
                    // For example:
                    // listBox.Items.Add(item.Name);
                    ToDoList.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ToDoItems: {ex.Message}");
            }
        }

        private async Task LoadStatusItems()
        {
            try
            {
                // Retrieve all ToDoItems
                var allItems = await statusItemRepository.GetAllAsync();

                // Display ToDoItems in the UI (e.g., in a ListBox or DataGrid)
                foreach (var item in allItems)
                {
                    // Add logic here to display ToDoItems in your UI
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ToDoItems: {ex.Message}");
            }
        }
    }
}