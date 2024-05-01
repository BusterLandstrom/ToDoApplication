using MongoDB.Driver;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IMongoDatabase Database { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AccountManager account = new AccountManager();
            account.SetAccount();
            string connectionUri = account.GetURI();
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            string accountDatabase = account.GetDatabase();
            Database = client.GetDatabase(accountDatabase);
        }

    }

}
