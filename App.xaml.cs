using MongoDB.Driver;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ToDoApplication.Items;

namespace ToDoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static AccountManager accountManager { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            accountManager = new AccountManager(); // Creating and connecting the database connection and account manager for the mongodb database
        }

    }

}
