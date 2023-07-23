using DemoWebAPIEF6HienLTH.Entities;

namespace DemoWebAPIEF6HienLTH.Services
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> getAllCategoriesAsync();
        public Task<Category> getBookByIdAsync(int Id);
        public Task<int> AddNewCategoryAsync(Category category);
        public Task UpdateCategoryAsync(int Id, Category category);
        public Task DeleteCategoryAsync(int Id);
    }
}
