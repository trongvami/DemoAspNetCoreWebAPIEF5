using MyFirstWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Services
{
    public interface IProductRepository
    {
        List<ProductModel> GetAll(string search, int page);
        CategoryVM AddNewCategory(CategoryResponse categoryResponse);
    }
}
