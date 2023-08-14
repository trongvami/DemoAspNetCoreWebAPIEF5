using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Entities;
using ServerSide.Models.ResponseModels;
using ServerSide.Models.ViewModels.Authentication.SignUp;
using System.Security.Policy;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly DbNet6Context _dbNet6Context;
        public WarehouseController(DbNet6Context dbNet6Context) { 
           _dbNet6Context = dbNet6Context;
        }

        #region Unit

        [HttpGet]
        [Route("UnitsList")]
        public async Task<IActionResult> UnitsList()
        {
            var units = await _dbNet6Context.TbUnits.AsNoTracking().ToListAsync();
            List<UnitsListResponse> unitList = new List<UnitsListResponse>();
            if (units != null)
            {
                foreach (var item in units)
                {
                    UnitsListResponse unit = new UnitsListResponse
                    {
                        UnitID = item.UnitId.ToString(),
                        IsActive = (bool)item.IsActive,
                        IsDelete = item.IsDelete,
                        UnitName = item.UnitName
                    };

                    unitList.Add(unit);
                }
            }
            return Ok(unitList);
        }

        [HttpPost]
        [Route("AddNewUnit")]
        public async Task<IActionResult> AddNewUnit([FromBody] UnitsListResponse unit)
        {
            TbUnit tbUnit = new TbUnit
            {
                IsActive = (bool)unit.IsActive,
                UnitName = unit.UnitName,
                IsDelete = unit.IsDelete
            };

            await _dbNet6Context.TbUnits.AddAsync(tbUnit);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Unit Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Unit !", Status = "Error" });
            }
        }

        [HttpPut]
        [Route("UpdateUnit")]
        public async Task<IActionResult> UpdateUnit([FromBody] UnitsListResponse unit)
        {
            var oldUnit = await _dbNet6Context.TbUnits.SingleOrDefaultAsync(x => x.UnitId.ToString() == unit.UnitID);

            oldUnit.UnitName = unit.UnitName;
            oldUnit.IsDelete = unit.IsDelete;
            oldUnit.IsActive = unit.IsActive;

            _dbNet6Context.TbUnits.Update(oldUnit);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Update Unit Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Update Unit !", Status = "Error" });
            }
        }

        [HttpDelete]
        [Route("DeleteUnit/{id}")]
        public async Task<IActionResult> DeleteUnit(string? id)
        {
            var unit = await _dbNet6Context.TbUnits.SingleOrDefaultAsync(x => x.UnitId.ToString() == id);
            var rs = _dbNet6Context.TbUnits.Remove(unit);
            var rs2 = await _dbNet6Context.SaveChangesAsync();
            if (rs2 > 0) {
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Delete Unit Successfully", Status = "Success" });
            }else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Delete Unit !", Status = "Error" });
            }

        }

        #endregion

        #region Category

        [HttpGet]
        [Route("CategoriesList")]
        public async Task<IActionResult> CategoriesList()
        {
            var categories = await _dbNet6Context.TbCategories.AsNoTracking().ToListAsync();
            List<CategoriesListResponse> cateList = new List<CategoriesListResponse>();
            if (categories != null) {
                foreach (var item in categories)
                {
                    CategoriesListResponse cate = new CategoriesListResponse
                    {
                        CatId = item.CatId.ToString(),
                        CatName = item.CatName.ToString(),
                        Alias = item.Alias,
                        Description = item.Description,
                        Levels = (int)(item.Levels == null ? 0 : item.Levels),
                        MetaDesc = item.MetaDesc,
                        MetaKey = item.MetaKey,
                        Ordering = (int)(item.Ordering == null ? 0 : item.Ordering),
                        ParentID = (int)(item.ParentId == null ? 0 : item.ParentId),
                        SchemaMarkup = item.SchemaMarkup,
                        Published = (bool)item.Published,
                        ShortContent = item.ShortContent,
                        Thumb = item.Thumb,
                        Title = item.Title
                    };

                    cateList.Add(cate);
                }
            }

            return Ok(cateList);
        }

        [HttpPost]
        [Route("AddNewCategory")]
        public async Task<IActionResult> AddNewCategory([FromBody] CategoriesListResponse category)
        {
            TbCategory tbCategory = new TbCategory {
                CatName = category.CatName,
                Alias = category.Alias,
                Description = category.Description,
                Levels = category.Levels,
                MetaDesc = category.MetaDesc,
                MetaKey = category.MetaKey,
                Ordering = category.Ordering,
                IsDeleted = false,
                ParentId = category.ParentID,
                SchemaMarkup = category.SchemaMarkup,
                Published = category.Published,
                ShortContent = category.ShortContent,
                Thumb = category.Thumb,
                Title = category.Title
            };
            
            await _dbNet6Context.TbCategories.AddAsync(tbCategory);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Category Successfully !", Status = "Success" });
            }
            else {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Category !", Status = "Error" });
            }
        }

        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoriesListResponse category)
        {
            var oldCategory = await _dbNet6Context.TbCategories.SingleOrDefaultAsync(x=>x.CatId.ToString() == category.CatId);
            oldCategory.CatName = category.CatName;
            oldCategory.Alias = category.Alias;
            oldCategory.Description = category.Description;
            oldCategory.Levels = category.Levels;
            oldCategory.MetaDesc = category.MetaDesc;
            oldCategory.MetaKey = category.MetaKey;
            oldCategory.Ordering = category.Ordering;
            oldCategory.IsDeleted = false;
            oldCategory.ParentId = category.ParentID;
            oldCategory.SchemaMarkup = category.SchemaMarkup;
            oldCategory.Published = category.Published;
            oldCategory.ShortContent = category.ShortContent;
            oldCategory.Thumb = category.Thumb;
            oldCategory.Title = category.Title;

            _dbNet6Context.TbCategories.Update(oldCategory);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Update Category Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Update Category !", Status = "Error" });
            }
        }

        #endregion

        #region Product

        [HttpGet]
        [Route("ProductsList")]
        public async Task<IActionResult> ProductsList()
        {
            var products = await _dbNet6Context.TbProducts.AsNoTracking().ToListAsync();
            List<ProductsListResponse> prosList = new List<ProductsListResponse>();
            if (products != null)
            {
                foreach (var item in products)
                {
                    ProductsListResponse product = new ProductsListResponse
                    {
                        ProductId = item.ProductId.ToString(),
                        Active = (bool)item.Active,
                        Alias = item.Alias,
                        BestSellers = (bool)item.BestSellers,
                        CatID = item.CatId.ToString(),
                        DateCreated = (DateTime)item.DateCreated,
                        DateModified = (DateTime)item.DateModified,
                        Description = item.Description,
                        Discount = (int)item.Discount,
                        HomeFlag = (bool)item.HomeFlag,
                        Image1 = item.Image1,
                        Image2 = item.Image2,
                        Image3 = item.Image3,
                        Image4 = item.Image4,
                        Image5 = item.Image5,
                        Image6 = item.Image6,
                        MetaDesc = item.MetaDesc,
                        MetaKey = item.MetaKey,
                        Price = (int)item.Price,
                        ProductName = item.ProductName,
                        ShortDesc = item.ShortDesc,
                        SoLuongBanDau = (int)item.SoLuongBanDau,
                        SoLuongDaBan = (int)item.SoLuongDaBan,
                        Tags = item.Tags,
                        Thumb = item.Thumb,
                        Title = item.Title,
                        UnitsInStock = (int)item.UnitsInStock,
                        Video = item.Video
                    };

                    prosList.Add(product);
                }
            }

            return Ok(prosList);
        }

        [HttpPost]
        [Route("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductsListResponse product)
        {
            var cat = await _dbNet6Context.TbCategories.SingleOrDefaultAsync(x=>x.CatId == int.Parse(product.CatID));
            TbProduct tp = new TbProduct
            {
                ProductName = product.ProductName,
                MetaDesc = product.MetaDesc,
                Price = product.Price,
                Image4 = product.Image4,
                Image5 = product.Image5,
                Image3 = product.Image3,
                Image6 = product.Image6,
                Image1 = product.Image1,
                Image2 = product.Image2,
                Active = product.Active,
                Alias = product.Alias,
                BestSellers = product.BestSellers,
                CatId = int.Parse(product.CatID),
                DateCreated = product.DateCreated,
                DateModified = product.DateModified,
                Description = product.Description,
                Discount = product.Discount,
                HomeFlag = product.HomeFlag,
                MetaKey = product.MetaKey,
                ShortDesc = product.ShortDesc,
                SoLuongBanDau = product.SoLuongBanDau,
                SoLuongDaBan = product.SoLuongDaBan,
                Tags = product.Tags,
                Thumb = product.Thumb,
                Title = product.Title,
                UnitsInStock = product.UnitsInStock,
                Video = product.Video,
                UnitId = product.UnitID,
                Cat = cat
            };

            await _dbNet6Context.TbProducts.AddAsync(tp);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Product Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Category !", Status = "Error" });
            }
        }

        #endregion
    }
}
