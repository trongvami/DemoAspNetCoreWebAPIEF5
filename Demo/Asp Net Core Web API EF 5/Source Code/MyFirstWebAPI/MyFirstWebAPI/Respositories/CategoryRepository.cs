using MyFirstWebAPI.Context;
using MyFirstWebAPI.Data;
using MyFirstWebAPI.Models;
using MyFirstWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Respositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public MyDbContext _myDbContext;

        public CategoryRepository(MyDbContext myDbContext) {
            _myDbContext = myDbContext;
        }

        public CategoryVM AddNewCategory(CategoryResponse categoryResponse)
        {
            var _category = new Category
            {
                TenLoai = categoryResponse.TenLoai
            };
            _myDbContext.Add(_category);
            _myDbContext.SaveChanges();

            return new CategoryVM
            {
                MaLoai = _category.MaLoai,
                TenLoai = _category.TenLoai
            };
        }

        public void DeleteCategory(int id)
        {
            var category = _myDbContext.Categories.SingleOrDefault(lo => lo.MaLoai == id);
            if (category != null) {
                _myDbContext.Remove(category);
                _myDbContext.SaveChanges();
            }
        }

        public List<CategoryVM> GetAllCategories()
        {
            var categories = _myDbContext.Categories.Select(lo => new CategoryVM { 
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai
            });

            return categories.ToList();
        }

        public CategoryVM GetCategoryById(int id)
        {
            var category = _myDbContext.Categories.SingleOrDefault(lo => lo.MaLoai == id);
            if (category != null)
            {
                return new CategoryVM { MaLoai = category.MaLoai, TenLoai = category.TenLoai };
            }
            else {
                return null;
            }
        }

        public void UpdateCategory(CategoryVM categoryVM)
        {
            var category = _myDbContext.Categories.SingleOrDefault(lo => lo.MaLoai == categoryVM.MaLoai);
            if (category != null)
            {
                category.TenLoai = categoryVM.TenLoai;
                _myDbContext.SaveChanges();
            }
        }
    }
}
