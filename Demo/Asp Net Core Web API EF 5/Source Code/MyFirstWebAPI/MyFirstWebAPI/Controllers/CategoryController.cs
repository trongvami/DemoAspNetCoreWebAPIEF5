using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstWebAPI.Context;
using MyFirstWebAPI.Data;
using System.Threading.Tasks;
using MyFirstWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private MyDbContext _myDbContext;
        public CategoryController(MyDbContext myDbContext)
        {
            this._myDbContext = myDbContext;
        }

        [HttpGet]
        public IActionResult GetAllCategories() {
            try
            {
                var listCategories = _myDbContext.Categories.ToList();
                return Ok(listCategories);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _myDbContext.Categories.SingleOrDefault(x=>x.MaLoai == id);
            if (category != null) {
                return Ok(category);
            }
            else {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateNewCategory(CategoryResponse categoryResponse)
        {
            try
            {
                var newCategory = new Category
                {
                    TenLoai = categoryResponse.TenLoai
                };
                _myDbContext.Add(newCategory);
                _myDbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, newCategory);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut("{id}")]
        public IActionResult EditCategoryById(int id, CategoryResponse categoryResponse)
        {
            try
            {
                var category = _myDbContext.Categories.SingleOrDefault(x => x.MaLoai == id);
                if (category != null)
                {
                    category.TenLoai = categoryResponse.TenLoai;
                    _myDbContext.SaveChanges();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                } 
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var category = _myDbContext.Categories.SingleOrDefault(x => x.MaLoai == id);
            if (category != null)
            {
                _myDbContext.Remove(category);
                _myDbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
