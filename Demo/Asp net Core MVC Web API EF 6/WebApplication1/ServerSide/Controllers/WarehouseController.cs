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

        #region Parent

        [HttpGet]
        [Route("ParentsList")]
        public async Task<IActionResult> ParentsList()
        {
            var units = await _dbNet6Context.TbParents.AsNoTracking().Include(x=>x.TbLevels).Include(x=>x.TbCategories).ToListAsync();
            List<ParentsListResponse> parentList = new List<ParentsListResponse>();
            if (units != null)
            {
                foreach (var item in units)
                {
                    ParentsListResponse parent = new ParentsListResponse
                    {
                        ParentID = item.ParentId.ToString(),
                        ParentActive = (bool)item.ParentActive,
                        ParentDelete = item.ParentDelete,
                        ParentName = item.ParentName,
                        AmountLevel = item.TbLevels.Count > 0 ? item.TbLevels.Count : 0,
                        AmountCategory = item.TbCategories.Count > 0 ? item.TbCategories.Count : 0
                    };

                    parentList.Add(parent);
                }
            }
            return Ok(parentList);
        }

        [HttpPost]
        [Route("AddNewParent")]
        public async Task<IActionResult> AddNewParent([FromBody] ParentsListResponse parent)
        {
            TbParent tbParent = new TbParent
            {
                ParentActive = (bool)parent.ParentActive,
                ParentDelete = parent.ParentDelete,
                ParentName = parent.ParentName
            };

            await _dbNet6Context.TbParents.AddAsync(tbParent);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Parent Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Parent !", Status = "Error" });
            }
        }

        [HttpPut]
        [Route("UpdateParent")]
        public async Task<IActionResult> UpdateParent([FromBody] ParentsListResponse parent)
        {
            var oldParent = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId.ToString() == parent.ParentID);

            oldParent.ParentName = parent.ParentName;
            oldParent.ParentDelete = parent.ParentDelete;
            oldParent.ParentActive = parent.ParentActive;

            _dbNet6Context.TbParents.Update(oldParent);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Update Parent Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Update Parent !", Status = "Error" });
            }
        }

        [HttpDelete]
        [Route("DeleteParent/{id}")]
        public async Task<IActionResult> DeleteParent(string? id)
        {
            var parent = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId.ToString() == id);
            var rs = _dbNet6Context.TbParents.Remove(parent);
            var rs2 = await _dbNet6Context.SaveChangesAsync();
            if (rs2 > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Delete Parent Successfully", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Delete Parent !", Status = "Error" });
            }

        }

        #endregion

        #region Level

        [HttpGet]
        [Route("LevelsList")]
        public async Task<IActionResult> LevelsList()
        {
            var levels = await _dbNet6Context.TbLevels.AsNoTracking().Include(x=>x.TbCategories).Include(x=>x.TbProducts).ToListAsync();
            List<LevelsListResponse> levelList = new List<LevelsListResponse>();
            if (levels != null)
            {
                foreach (var item in levels)
                {
                    LevelsListResponse level = new LevelsListResponse
                    {
                        LevelActive = (bool)item.LevelActive,
                        LevelDelete = item.LevelDelete,
                        LevelName = item.LevelName,
                        LevelCode = item.LevelCode.ToString(),
                        ParentID = item.ParentId.ToString(),
                        AmountCategory = item.TbCategories.Count > 0 ? item.TbCategories.Count : 0,
                        AmountProduct = item.TbProducts.Count > 0 ? item.TbProducts.Count : 0,
                    };

                    levelList.Add(level);
                }
            }
            return Ok(levelList);
        }

        [HttpPost]
        [Route("AddNewLevel")]
        public async Task<IActionResult> AddNewLevel([FromBody] LevelsListResponse level)
        {
            TbLevel tbLevel = new TbLevel
            {
                LevelActive = (bool)level.LevelActive,
                LevelDelete = level.LevelDelete,
                LevelName = level.LevelName,
                ParentId = int.Parse(level.ParentID)
            };

            await _dbNet6Context.TbLevels.AddAsync(tbLevel);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Level Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Level !", Status = "Error" });
            }
        }

        #endregion

        #region Unit Payment

        [HttpGet]
        [Route("UnitPaymentsList")]
        public async Task<IActionResult> UnitPaymentsList()
        {
            var units = await _dbNet6Context.TbUnitsPayments.AsNoTracking().ToListAsync();
            List<UnitPaymentsListResponse> unitList = new List<UnitPaymentsListResponse>();
            if (units != null)
            {
                foreach (var item in units)
                {
                    UnitPaymentsListResponse unit = new UnitPaymentsListResponse
                    {
                        UpayId = item.UpayId.ToString(),
                        IsActive = (bool)item.IsActive,
                        IsDelete = item.IsDelete,
                        UpayName = item.UpayName
                    };

                    unitList.Add(unit);
                }
            }
            return Ok(unitList);
        }

        [HttpPost]
        [Route("AddNewUnitpay")]
        public async Task<IActionResult> AddNewUnitpay([FromBody] UnitPaymentsListResponse unit)
        {
            TbUnitsPayment tbUnit = new TbUnitsPayment
            {
                IsActive = (bool)unit.IsActive,
                UpayName = unit.UpayName,
                IsDelete = unit.IsDelete
            };

            await _dbNet6Context.TbUnitsPayments.AddAsync(tbUnit);
            var result2 = await _dbNet6Context.SaveChangesAsync();

            if (result2 > 0)
            {
                return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = $"Add New Unitpay Successfully !", Status = "Success" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = "Cannot Add New Unitpay !", Status = "Error" });
            }
        }

        #endregion

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
            var categories = await _dbNet6Context.TbCategories.AsNoTracking().Include(x=>x.TbProducts).ToListAsync();
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
                        Title = item.Title,
                        AmountProduct = item.TbProducts.Count > 0 ? item.TbProducts.Count : 0
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
            var level = await _dbNet6Context.TbLevels.SingleOrDefaultAsync(x=>x.LevelCode == category.Levels);
            TbCategory tbCategory = new TbCategory {
                CatName = category.CatName,
                Alias = category.Alias,
                Description = category.Description,
                Levels = category.Levels,
                MetaDesc = category.MetaDesc,
                MetaKey = category.MetaKey,
                Ordering = category.Ordering,
                IsDeleted = false,
                ParentId = level.ParentId,
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
                        Video = item.Video,
                        UnitID = item.UnitId,
                        Height = (int)item.Height,
                        LevelCode = item.LevelCode != null ? item.LevelCode.ToString() : null,
                        UpayId = item.UpayId
                    };

                    prosList.Add(product);
                }
            }

            return Ok(prosList);
        }

        [HttpGet]
        [Route("ProductsList2")]
        public async Task<IActionResult> ProductsList2()
        {
            var products = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Cat).Include(x => x.LevelCodeNavigation).Include(x => x.Unit).Include(x => x.Upay).ToListAsync();
            List<ProductsListResponse2> prosList = new List<ProductsListResponse2>();
            if (products != null)
            {
                foreach (var item in products)
                {
                    ProductsListResponse2 product = new ProductsListResponse2
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
                        Video = item.Video,
                        UnitID = item.UnitId,
                        Height = (int)item.Height,
                        LevelCode = item.LevelCode != null ? item.LevelCode.ToString() : null,
                        UpayId = item.UpayId,
                        Unit = item.Unit,
                        Category = item.Cat,
                        UnitsPayment = item.Upay,
                        Level = item.LevelCodeNavigation
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
            var cat = new TbCategory();
            if (product.CatID != null) {
                cat = await _dbNet6Context.TbCategories.SingleOrDefaultAsync(x => x.CatId == int.Parse(product.CatID));
            }

            var unit = new TbUnit();
            if (product.UnitID != null)
            {
                unit = await _dbNet6Context.TbUnits.SingleOrDefaultAsync(x => x.UnitId == product.UnitID);
            }

            var unitpayment = new TbUnitsPayment();
            if (product.UpayId != null)
            {
                unitpayment = await _dbNet6Context.TbUnitsPayments.SingleOrDefaultAsync(x => x.UpayId == product.UpayId);
            }

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
                CatId = product.CatID != null ? int.Parse(product.CatID) : null,
                LevelCode = int.Parse(product.LevelCode),
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
                UpayId = product.UpayId,
                Height = product.Height
            };

            if (product.CatID != null) {
                tp.Cat = cat;
            }
            else
            {
                tp.Cat = null;
            }

            if (product.UnitID != null)
            {
                tp.Unit = unit;
            }
            else
            {
                tp.Cat = null;
            }

            if (product.CatID != null)
            {
                tp.Upay = unitpayment;
            }
            else
            {
                tp.Cat = null;
            }

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
