using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace ToDoApplication.Items
{
    public class AccountManager
    {
        private string appDataLocal { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ToDo";
        private string credentialsPath { get; set; } = "/account.json";
        private string user { get; set; } = "None";
        private string password { get; set; } = "None";
        private string application { get; set; } = "None";
        private string connectionUri { get; set; } = "None";
        private MongoClient client {  get; set; }
        private MongoClientSettings clientSettings { get; set; }
        private bool success { get; set; } = false;
        private static IMongoDatabase Database { get; set; }
        private string databaseName { get; set; } = "ToDo";
        private List<string> databaseCollections { get; set; } = new List<string>() { "ToDoItems", "StatusItems" };

        private ToDoItemRepository todoItemRepository { get; set; }

        private StatusItemRepository statusItemRepository { get; set; }

        // These two comming variables might need do get removed or rewored depending on how i handle the initialization of the collections and the loading of the items
        private Task<List<ToDoItem>> todoItems {  get; set; } // This one ^^
        private Task<List<StatusItem>> statusItems { get; set; } // This one ^^

        public ToDoItem? currentTodoitem { get; set; }
        public Step? currentStepitem { get; set; }

        public MainWindow mainWindow { get; set; }

        public AccountManager()
        {
            success = SetAccount();
            if (success)
            {
                SetURI();
                ConnectDatabase();
                todoItemRepository = new ToDoItemRepository(Database);
                statusItemRepository = new StatusItemRepository(Database);
            }
        }

        public string GetToDoCollectionName()
        {
            return databaseCollections[0]; 
        }

        public string GetStatusCollectionName()
        {
            return databaseCollections[1];
        }

        public string GetDatabaseName()
        {
            return databaseName;
        }

        public void SetDatabaseName(string name)
        {
            databaseName = name;
        }

        public bool SetAccount()
        {
            try
            {
                JObject accountJson = JObject.Parse(File.ReadAllText(appDataLocal + credentialsPath));

                if (accountJson.TryGetValue("user", out JToken userToken)) { user = Uri.EscapeDataString(userToken.ToString()); } else { throw new ItemNotFound("user"); }
                if (accountJson.TryGetValue("password", out JToken passwordToken)) { password = Uri.EscapeDataString(passwordToken.ToString()); } else { throw new ItemNotFound("password"); }
                if (accountJson.TryGetValue("mongodbpath", out JToken applicationToken)) { application = applicationToken.ToString(); } else { throw new ItemNotFound("mongodbpath"); }
                return true;
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine($"The account file not found: ({e})");
                return false;
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.WriteLine($"The directory '{appDataLocal}' was not found: ({e})");
                return false;
            }
            catch (IOException e)
            {
                Debug.WriteLine($"The account file could not be opened: ({e})");
                return false;
            }
            catch (ItemNotFound e)
            {
                Debug.WriteLine($"The item '{e.Item}' does not exist in the JSON file");
                return false;
            }
        }

        public void SetURI()
        {
            connectionUri = $"mongodb+srv://{user}:{password}{application}";
        }

        public string GetURI()
        {
            return connectionUri;
        }

        public void ConnectDatabase() 
        {
            clientSettings = MongoClientSettings.FromConnectionString(connectionUri);

            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

            client = new MongoClient(clientSettings);

            Database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase GetClientDatabase() 
        {
            return Database;
        }

        public ToDoItemRepository GetToDoRepo()
        {
            return todoItemRepository;
        }
        public StatusItemRepository GetStatusRepo()
        {
            return statusItemRepository;
        }

        public async Task InitializeCollections()
        {
            // Check if collections exist
            var toDoItemsExist = await EnsureCollectionExists(GetToDoCollectionName());
            var statusItemsExist = await EnsureCollectionExists(GetStatusCollectionName());

            var statusCollectionEmpty = await IsCollectionEmpty(GetStatusCollectionName());
            if (statusCollectionEmpty)
            {
                StatusItem newStatusItem = new StatusItem();

                await statusItemRepository.CreateAsync(newStatusItem);

                foreach (Status status in newStatusItem.Statuses)
                {
                    Debug.WriteLine($"Added status '{status.statusText}' with the color value '{status.colorValue}'");
                }
            }
            else
            {

                /*foreach (Status status in statusItemRepository.)
                {
                    Debug.WriteLine($"Loaded status '{status.statusText}' with the color value '{status.colorValue}'");
                }*/

            }

            todoItems =  todoItemRepository.GetAllAsync();
            statusItems = statusItemRepository.GetAllAsync();
        }

        public Task<List<StatusItem>> GetStatusItems() 
        {
            return statusItems;
        }

        public Task<List<ToDoItem>> GetToDoItems() // Convert to use function underneath in all of the scripts
        {
            return todoItems;
        }

        public List<ToDoItem> GetToDoItemList()
        {
            return todoItems.Result;
        }

        public async Task UpdateCollections() 
        {
            todoItems = todoItemRepository.GetAllAsync();
            statusItems = statusItemRepository.GetAllAsync();
        }

        private async Task<bool> EnsureCollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collectionsCursor = await Database.ListCollectionNamesAsync(new ListCollectionNamesOptions { Filter = filter });
            var collectionExists = await collectionsCursor.AnyAsync();

            if (!collectionExists)
            {
                await Database.CreateCollectionAsync(collectionName);
            }

            return collectionExists;
        }

        public async Task<bool> IsCollectionEmpty(string collectionName)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var count = await collection.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);
            return count == 0;
        }

        public bool ToDoItemExists() 
        {
            foreach (ToDoItem item in GetToDoItemList()) 
            {
                if (currentTodoitem.Id == item.Id)
                {
                    return true;
                }
            }

            return false;
        }

        public void CreateBlankTask(string TaskName, string Description, Status SelectedStatus) 
        {
            currentTodoitem = new ToDoItem
            {
                Name = TaskName,
                Description = Description,
                Status = SelectedStatus,
                Done = false,
                Steps = new List<Step>() // This is just creating an empty steps i need to create the steps items first

            };
        }


    }

    // Custom classes

    public class ItemNotFound : Exception
    {
        public string Item { get; }
        public ItemNotFound(string item)
        {
            Item = item;
            base.Data.Add("Item", Item);
        }
    }
}
