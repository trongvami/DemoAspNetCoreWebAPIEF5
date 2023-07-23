using DemoWebAPIEF6HienLTH.Entities;
using DemoWebAPIEF6HienLTH.ViewModel;

namespace DemoWebAPIEF6HienLTH.Services
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategoriesAsync();
        public Task<CategoryModel> GetBookByIdAsync(int Id);
        public Task<int> AddNewCategoryAsync(CategoryModel category);
        public Task UpdateCategoryAsync(int Id, CategoryModel category);
        public Task DeleteCategoryAsync(int Id);
    }
}
