using MyFirstWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Services
{
    public interface ICategoryRepository
    {
        List<CategoryVM> GetAllCategories();
        CategoryVM GetCategoryById(int id);
        CategoryVM AddNewCategory(CategoryResponse categoryResponse);
        void UpdateCategory(CategoryVM categoryVM);
        void DeleteCategory(int id);
    }
}
