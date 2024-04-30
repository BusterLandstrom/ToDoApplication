using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication
{
    public class AccountManager
    {
        private string appDataLocal { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ToDo";
        private string credentialsPath { get; set; } =  "/account.json";
        private string user { get; set; } = "None";
        private string password { get; set; } = "None";
        private string application { get; set; } = "None";
        private string connectionUri { get; set; } = "None";
        private bool success { get; set; } = false;
        private string database {  get; set; } = "ToDo";
        private List<string> databaseCollections { get; set; } = new List<string>(){ "Items", "Status" };

        public AccountManager()
        {
            success = SetAccount();
            if (success)
            {
                SetURI();
            }
        }

        public string GetItemCollection() 
        {
            return databaseCollections[0];
        }

        public string GetStatusCollection()
        {
            return databaseCollections[1];
        }

        public string GetDatabase() 
        {
            return database;
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
