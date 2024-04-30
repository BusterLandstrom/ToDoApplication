using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApplication
{
    public class StatusItem
    {
        public ObjectId Id { get; set; }
        public List<Status> Statuses { get; set; } = new List<Status>() {
                                                    new Status { statusText = "To Do", colorValue = "#E9B44C" },
                                                    new Status { statusText = "In Progress", colorValue = "#7692FF" },
                                                    new Status { statusText = "Done", colorValue = "#008148" }
                                                        };

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
    }
}
