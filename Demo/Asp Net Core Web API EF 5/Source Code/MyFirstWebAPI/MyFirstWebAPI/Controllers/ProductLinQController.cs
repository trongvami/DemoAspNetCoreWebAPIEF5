using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPI.Respositories;
using MyFirstWebAPI.Services;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLinQController : ControllerBase
    {
        IProductRepository _productRepository;
        public ProductLinQController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }
        [HttpGet("{search}/{page}")]
        public IActionResult GetProductsByName(string search, int page)
        {
            try
            {
                return Ok(_productRepository.GetAll(search, page));
            }
            catch (System.Exception)
            {
                return BadRequest("We cannot get any product by this word" + search);
            }
        }
    }
}
