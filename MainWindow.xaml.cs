using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using ToDoApplication.Items;
using ToDoApplication.ViewModels;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly IMongoDatabase database;
        private readonly ToDoItemRepository todoItemRepository;
        private readonly StatusItemRepository statusItemRepository;
        public ToDoItemsViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ToDoItemsViewModel();
            DataContext = ViewModel;

            database = App.Database;

            todoItemRepository = new ToDoItemRepository(database);
            statusItemRepository = new StatusItemRepository(database);

            Loaded += MainWindow_Loaded;

            LoadData(todoItemRepository);

        }

        private async void LoadData(ToDoItemRepository repository)
        {
            var items = await repository.GetAllAsync();
            foreach (var item in items)
            {
                ViewModel.Items.Add(new ToDoItemViewModel
                {
                    Name = item.Name,
                    Description = item.Description,
                    Status = item.Status,
                    Done = item.Done,
                    Steps = new List<Step>(item.Steps)
                });
            }
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load ToDoItems from the database and display them
            await InitializeDatabase();
        }

        private async Task InitializeDatabase()
        {
            // Check if collections exist
            var toDoItemsExist = await EnsureCollectionExists("ToDoItems");
            var statusItemsExist = await EnsureCollectionExists("StatusItems");

            var statusCollectionEmpty = await IsCollectionEmpty("StatusItems");
            if (statusCollectionEmpty) 
            {
                StatusItem newStatusItem = new StatusItem();

                await statusItemRepository.CreateAsync(newStatusItem);

                foreach (Status status in newStatusItem.Statuses)
                {
                    Debug.WriteLine($"Added status '{status.statusText}' with the color value '{status.colorValue}'");
                }
            }

            
        }

        private async Task<bool> EnsureCollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collectionsCursor = await database.ListCollectionNamesAsync(new ListCollectionNamesOptions { Filter = filter });
            var collectionExists = await collectionsCursor.AnyAsync();

            if (!collectionExists)
            {
                await database.CreateCollectionAsync(collectionName);
            }

            return collectionExists;
        }

        public async Task<bool> IsCollectionEmpty(string collectionName)
        {
            var collection = database.GetCollection<BsonDocument>(collectionName);
            var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);
            return count == 0;
        }

        private async Task CreateCollection(string collectionName)
        {
            await database.CreateCollectionAsync(collectionName);
        }
    }
}