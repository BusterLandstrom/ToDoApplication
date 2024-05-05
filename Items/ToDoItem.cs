using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Items
{
    public class ToDoItem : IToDoViewModel, INotifyPropertyChanged // This item is converted to the ToDoItemViewModel to remove redundancy and actually fix a baseline issue in the code
    {
        public ObjectId Id { get; set; }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set { 
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
        }
        private string _description;
        public string Description 
        { 
            get { return _description; } 
            set 
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private Status _status;
        public Status Status 
        {
            get { return _status; }
            set 
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private bool _done;
        public bool Done 
        {
            get { return _done; }
            set 
            {
                _done = value;
                OnPropertyChanged(nameof(Done));
            }
        }

        private List<Step> _steps;
        public List<Step> Steps 
        { 
            get { return _steps; }
            set 
            {
                _steps = value;
                OnPropertyChanged(nameof(Steps));
            }        
        }

        private int _stepsDone;
        public int StepsDone 
        { 
            get { return _stepsDone; }
            set 
            {
                _stepsDone = value;
                OnPropertyChanged(nameof(StepsDone));
            }
            
        }

        private int _totalSteps;
        public int TotalSteps
        {
            get { return _totalSteps; }
            set 
            {
                _totalSteps = value;
                OnPropertyChanged(nameof(TotalSteps));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsDone() 
        {
            if (Done || Status.statusText == "Done") 
            {
                return true;
            }

            return false;
        }

        public async Task ToggleDone() // This function is bad right now and needs to now get all status items when trying to toggle the done variable
        {
            Done = !Done;

            if (Done)
            {
                var statusItems = await App.accountManager.GetStatusItems();
                var doneStatus = statusItems[0]; 
                var databaseDoneStatus = doneStatus.GetStatus(2); // This is just the "Done" status from the database it is bad to do it this way not valid at all so I need to fix it later!!
                Status = databaseDoneStatus;
            } else // This else function is currently just revereting the Status to a set status (in this case it is the "To do" status from the database) think about revisioning this function to revert to last set function before "Done"
            {
                var statusItems = await App.accountManager.GetStatusItems();
                var doneStatus = statusItems[0];
                var databaseDoneStatus = doneStatus.GetStatus(0); // This is just the "To do" status from the database it is bad to do it this way not valid at all so I need to fix it later!!
                Status = databaseDoneStatus;
            }

            UpdateItem();
        }

        public async void UpdateItem() 
        {
            await App.accountManager.GetToDoRepo().UpdateAsync(Id, this);
        }

        public int GetStepsDone() 
        {
            var doneCounter = 0;
            foreach (var step in Steps) 
            {
                if (step.StepDone == true) { doneCounter++; }
            }
            return doneCounter;
        }

        public int GetStepsTotal()
        {
            return Steps.Count;
        }

    }

    public class ToDoItemRepository
    {
        private readonly IMongoCollection<ToDoItem> collection;

        public ToDoItemRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<ToDoItem>("ToDoItems");
        }

        public async Task CreateAsync(ToDoItem item)
        {
            await collection.InsertOneAsync(item);
        }

        public async Task<List<ToDoItem>> GetAllAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<ToDoItem> GetByIdAsync(ObjectId id)
        {
            return await collection.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ObjectId id, ToDoItem item)
        {
            await collection.ReplaceOneAsync(i => i.Id == id, item);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await collection.DeleteOneAsync(i => i.Id == id);
        }
    }
}
