using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApplication.Items
{
    public class ToDoItem
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public bool Done { get; set; } = false;
        public List<Step> Steps { get; set; }
        public void SetDone(Status done) { Done = true; Status = done; }
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
