using HR_System.Models;
using MongoDB.Driver;

namespace HR_System.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoriesCollection;

        public CategoryService(IMongoDatabase database)
        {
            _categoriesCollection = database.GetCollection<Category>("Categories");
        }

        public async Task<List<Category>> GetAllCategoriesAsync() =>
            await _categoriesCollection.Find(_ => true).ToListAsync();

        public async Task<Category?> GetCategoryByIdAsync(string id) =>
            await _categoriesCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateCategoryAsync(Category category) =>
            await _categoriesCollection.InsertOneAsync(category);
    }
}