using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebAPI.Context;
using MyFirstWebAPI.Data;
using MyFirstWebAPI.Models;
using MyFirstWebAPI.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MyFirstWebAPI.Respositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _myDbContext;

        public ProductRepository(MyDbContext myDbContext) { 
            _myDbContext = myDbContext;
        }

        public CategoryVM AddNewCategory(CategoryResponse categoryResponse)
        {
            throw new System.NotImplementedException();
        }

        public List<ProductModel> GetAll(string search, int page = 1)
        {
            var list = _myDbContext.Products.Include(hh => hh.Category).AsQueryable();
            var list2 = _myDbContext.Products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                list = _myDbContext.Products.Include(hh => hh.Category).Where(o => o.TenHH.Contains(search));
            }
            //double? asd = 0;
            //if (asd.HasValue) { 
            int Page_Size = 2;
            var lists = list2.Skip((page - 1) * Page_Size).Take(Page_Size);
            //}
            var results = lists.Select(hh => new ProductModel
            {
                MaHangHoa = hh.MaHH,
                TenHangHoa = hh.TenHH,
                DonGia = hh.DonGia,
                TenLoai = hh.Category.TenLoai
            });
            var res = PaginatedList<Product>.Create(list2, page, Page_Size);
            return res.Select(hh => new ProductModel {
                MaHangHoa = hh.MaHH,
                TenHangHoa = hh.TenHH,
                DonGia = hh.DonGia,
                TenLoai = hh.Category.TenLoai
            }).ToList();
        }
    }
}
