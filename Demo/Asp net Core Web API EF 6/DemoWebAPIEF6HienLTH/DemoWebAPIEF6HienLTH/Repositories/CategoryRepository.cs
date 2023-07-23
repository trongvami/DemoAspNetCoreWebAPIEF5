using AutoMapper;
using DemoWebAPIEF6HienLTH.Entities;
using DemoWebAPIEF6HienLTH.Services;
using DemoWebAPIEF6HienLTH.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAPIEF6HienLTH.Repositories
{
    
    public class CategoryRepository : ICategoryRepository
    {

        private readonly MyShopHienLTHAspNetCoreEF6Context _context;
        private readonly IMapper _mapper;

        public CategoryRepository(MyShopHienLTHAspNetCoreEF6Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<int> AddNewCategoryAsync(CategoryModel category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var result = await _context.Categories.Include(x => x.Products).ToListAsync();
            return _mapper.Map<List<CategoryModel>>(result);
        }

        public Task<CategoryModel> GetBookByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(int Id, CategoryModel category)
        {
            throw new NotImplementedException();
        }
    }
}
