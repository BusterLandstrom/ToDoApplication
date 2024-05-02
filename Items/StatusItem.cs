using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApplication.Items
{
    public class StatusItem
    {
        public ObjectId Id { get; set; }
        public List<Status> Statuses { get; set; } = new List<Status>();

        public StatusItem()
        {
            // Add default statuses if the list is empty
            if (Statuses.Count == 0)
            {
                Statuses.Add(new Status { statusText = "To Do", colorValue = "#E9B44C" });
                Statuses.Add(new Status { statusText = "In Progress", colorValue = "#7692FF" });
                Statuses.Add(new Status { statusText = "Done", colorValue = "#008148" });
            }
        }

        public void AddStatus(Status status)
        {
            Statuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            Statuses.Remove(status);
        }
    }

    public class StatusItemRepository
    {
        private readonly IMongoCollection<StatusItem> collection;

        public StatusItemRepository(IMongoDatabase database)
        {
            collection = database.GetCollection<StatusItem>("StatusItems");
        }

        public async Task CreateAsync(StatusItem item)
        {
            await collection.InsertOneAsync(item);
        }

        public async Task<List<StatusItem>> GetAllAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<StatusItem> GetByIdAsync(ObjectId id)
        {
            return await collection.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ObjectId id, StatusItem item)
        {
            await collection.ReplaceOneAsync(i => i.Id == id, item);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await collection.DeleteOneAsync(i => i.Id == id);
        }

        public async Task<List<Status>> GetAllStatusesAsync()
        {
            var statusItems = await collection.Find(_ => true).ToListAsync();
            var statuses = new List<Status>();

            foreach (var statusItem in statusItems)
            {
                statuses.AddRange(statusItem.Statuses);
            }

            return statuses;
        }
    }
}
