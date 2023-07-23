using DemoWebAPIEF6HienLTH.Entities;
using DemoWebAPIEF6HienLTH.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace DemoWebAPIEF6HienLTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _services;
        public CategoriesController(ICategoryRepository services) {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories() {
            var result = await _services.GetAllCategoriesAsync();
            return Ok(result);
        }
    }
}
