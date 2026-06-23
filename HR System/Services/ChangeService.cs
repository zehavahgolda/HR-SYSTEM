using HR_System.Models;
using MongoDB.Driver;

namespace HR_System.Services
{
    public class ChangeService : IChangeService
    {
        private readonly IMongoCollection<Change> _changesCollection;

        public ChangeService(IMongoDatabase database)
        {
            _changesCollection = database.GetCollection<Change>("Changes");
        }

        public async Task<List<Change>> GetChangesBySystemIdAsync(string systemId) =>
            await _changesCollection.Find(c => c.SystemId == systemId).ToListAsync();

        public async Task CreateChangeAsync(Change change) =>
            await _changesCollection.InsertOneAsync(change);
    }
}