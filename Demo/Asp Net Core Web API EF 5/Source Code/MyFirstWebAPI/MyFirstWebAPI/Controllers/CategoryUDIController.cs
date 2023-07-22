using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPI.Models;
using MyFirstWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryUDIController : ControllerBase
    {
        private readonly ICategoryRepository _service;
        public CategoryUDIController(ICategoryRepository service) {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() {
            try
            {
                var lCategories = _service.GetAllCategories();
                return Ok(lCategories);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _service.GetCategoryById(id);
                if (category != null)
                {
                    return Ok(category);
                }
                else {
                    return NotFound();
                }
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryVM categoryVM)
        {
            if (id != categoryVM.MaLoai) {
                return BadRequest();
            }
            else
            {
                try
                {
                    _service.UpdateCategory(categoryVM);
                    return NoContent();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = _service.GetCategoryById(id);
                if (category != null)
                {
                    _service.DeleteCategory(id);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CategoryResponse categoryResponse)
        {
            try
            {
                return Ok(_service.AddNewCategory(categoryResponse));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
