using MyFirstWebAPI.Models;
using MyFirstWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Respositories
{
    public class CategoryRepositoryInMemory : ICategoryRepository
    {
        static List<CategoryVM> categoryVMs = new List<CategoryVM> { 
            new CategoryVM {MaLoai = 1, TenLoai = "Television" },
            new CategoryVM {MaLoai = 2, TenLoai = "Laptop" },
            new CategoryVM {MaLoai = 3, TenLoai = "Refrige" },
            new CategoryVM {MaLoai = 4, TenLoai = "Air Condition" }
        };
        public CategoryVM AddNewCategory(CategoryResponse categoryResponse)
        {
            var dt = new CategoryVM { MaLoai = categoryVMs.Max(lo => lo.MaLoai) + 1, TenLoai = categoryResponse.TenLoai };
            categoryVMs.Add(dt);
            return dt;
        }

        public void DeleteCategory(int id)
        {
            categoryVMs.Remove(categoryVMs.SingleOrDefault(x => x.MaLoai == id));
        }

        public List<CategoryVM> GetAllCategories()
        {
            return categoryVMs;
        }

        public CategoryVM GetCategoryById(int id)
        {
            return categoryVMs.SingleOrDefault(x=> x.MaLoai == id);
        }

        public void UpdateCategory(CategoryVM categoryVM)
        {
            var dt = categoryVMs.SingleOrDefault(x => x.MaLoai == categoryVM.MaLoai);
            if (dt != null)
            {
                dt.TenLoai = categoryVM.TenLoai;
            }

        }
    }
}
