using DemoWebAPIEF6HienLTH.Entities;
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
        private readonly MyShopHienLTHAspNetCoreEF6Context _context;
        public CategoriesController(MyShopHienLTHAspNetCoreEF6Context context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories() {
            return Ok(await _context.Categories.Include(x=>x.Products).ToListAsync());
        }
    }
}
