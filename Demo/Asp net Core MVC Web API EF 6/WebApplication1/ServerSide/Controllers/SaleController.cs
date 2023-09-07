using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Entities;
using ServerSide.Models.ResponseModels;
using ServerSide.Models.ViewModels.Harmichome;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly DbNet6Context _dbNet6Context;
        public SaleController(DbNet6Context dbNet6Context)
        {
            _dbNet6Context = dbNet6Context;
        }

        [HttpGet]
        [Route("GetProductsByCateArea")]
        public async Task<IActionResult> GetProductsByCateArea()
        {
            var lsProducts = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x=>x.Upay).Include(x=>x.LevelCodeNavigation).Where(x => x.Active == true && x.HomeFlag == true).ToListAsync();
            var lsParents = await _dbNet6Context.TbParents.AsNoTracking().Where(x => x.ParentActive == true).ToListAsync();
            List<ProductHomeVM> lsProductHomeVM = new List<ProductHomeVM>();
            if (lsParents != null)
            {
                foreach (var item in lsParents)
                {
                    ProductHomeVM productHomeVM = new ProductHomeVM
                    {
                        category = item,
                        lsproducts = lsProducts.Where(x=>x.LevelCodeNavigation.ParentId == item.ParentId).Take(8).ToList()
                    };

                    lsProductHomeVM.Add(productHomeVM);
                }
            }
            return Ok(lsProductHomeVM);
        }

        [HttpGet]
        [Route("GetProductDetailById/{Id}")]
        public async Task<IActionResult> GetProductDetailById(int Id)
        {
            var product = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).SingleOrDefaultAsync(x=>x.ProductId == Id && x.Active == true);
            var parent = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId == product.LevelCodeNavigation.ParentId);
            var lsRelatedProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true && x.ProductId != Id && x.LevelCodeNavigation.ParentId == parent.ParentId).Take(8).ToListAsync();

            List<ProductDetail> lsProduct = new List<ProductDetail>();

            ProductDetail productDetail = new ProductDetail { 
                ProductId = Id,
                Active = (bool)product.Active,
                Alias = product.Alias,
                BestSellers = (bool)product.BestSellers,
                HomeFlag = (bool)product.HomeFlag,
                CatId = product.CatId,
                CatName = product.Cat != null ? product.Cat.CatName : null,
                Description = product.Description,
                Discount = (int)product.Discount,
                Image1 = product.Image1,
                Image2 = product.Image2,
                Image3 = product.Image3,
                Image4 = product.Image4,
                Image5 = product.Image5,
                Thumb = product.Thumb,
                Image6 = product.Image6,
                LevelCode = (int)product.LevelCode,
                LevelName  = product.LevelCodeNavigation != null ? product.LevelCodeNavigation.LevelName : null,
                MetaDesc = product.MetaDesc,
                MetaKey = product.MetaKey,
                ParentId = (int)product.LevelCodeNavigation.ParentId,
                ParentName = parent.ParentName,
                Price = (int)product.Price,
                ProductName = product.ProductName,
                ShortDesc = product.ShortDesc,
                SoLuongConLai = (int)product.UnitsInStock
            };

            ProductDetailVM productDetailVM = new ProductDetailVM();

            if (lsRelatedProduct != null)
            {
                if (lsRelatedProduct.Count > 0)
                {
                    foreach (var item in lsRelatedProduct)
                    {
                        var parents = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId == item.LevelCodeNavigation.ParentId);
                        ProductDetail productDT = new ProductDetail
                        {
                            ProductId = item.ProductId,
                            Active = (bool)item.Active,
                            Alias = item.Alias,
                            BestSellers = (bool)item.BestSellers,
                            HomeFlag = (bool)item.HomeFlag,
                            CatId = item.CatId,
                            CatName = item.Cat != null ? item.Cat.CatName : null,
                            Description = item.Description,
                            Discount = (int)item.Discount,
                            Image1 = item.Image1,
                            Image2 = item.Image2,
                            Image3 = item.Image3,
                            Image4 = item.Image4,
                            Image5 = item.Image5,
                            Image6 = item.Image6,
                            Thumb = item.Thumb,
                            LevelCode = (int)item.LevelCode,
                            LevelName = item.LevelCodeNavigation != null ? item.LevelCodeNavigation.LevelName : null,
                            MetaDesc = item.MetaDesc,
                            MetaKey = item.MetaKey,
                            ParentId = (int)item.LevelCodeNavigation.ParentId,
                            ParentName = parents.ParentName,
                            Price = (int)item.Price,
                            ProductName = item.ProductName,
                            ShortDesc = item.ShortDesc,
                            SoLuongConLai = (int)item.UnitsInStock
                        };

                        lsProduct.Add(productDT);
                    }
                }
            }

            productDetailVM.ProductDetail = productDetail;
            productDetailVM.lsRelatedProduct = lsProduct;

            return Ok(productDetailVM);
        }

        [HttpGet]
        [Route("GetDataForShopProductByLevelCode/{LevelCode}")]
        public async Task<IActionResult> GetDataForShopProductByLevelCode(int LevelCode)
        {
            var levell = await _dbNet6Context.TbLevels.SingleOrDefaultAsync(x => x.LevelCode == LevelCode);
            var parent = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId == levell.ParentId);
            var lsRelatedProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true && x.LevelCode != LevelCode && x.LevelCodeNavigation.ParentId == parent.ParentId).Take(5).ToListAsync();

            var lsProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true && x.LevelCode == LevelCode).ToListAsync();

            List<ShopProduct> lsShopProduct = new List<ShopProduct>();
            List<ShopProduct> lsRelatedShopProduct = new List<ShopProduct>();

            if (lsProduct != null)
            {
                if (lsProduct.Count > 0)
                {
                    foreach (var item in lsProduct)
                    {
                        ShopProduct shopProduct = new ShopProduct
                        {
                            Active = (bool)item.Active,
                            Alias = item.Alias,
                            ProductId = item.ProductId,
                            BestSellers = (bool)item.BestSellers,
                            CatId = item.CatId,
                            Description = item.Description,
                            Discount = (int)item.Discount,
                            HomeFlag = (bool)item.HomeFlag,
                            LevelCode = (int)item.LevelCode,
                            ParentId = (int)item.LevelCodeNavigation.ParentId,
                            Price = (int)item.Price,
                            ProductName = item.ProductName,
                            ShortDesc = item.ShortDesc,
                            SoLuongConLai = (int)item.UnitsInStock,
                            Thumb = item.Thumb
                        };
                        lsShopProduct.Add(shopProduct);
                    }
                }
            }

            if (lsRelatedProduct != null)
            {
                if (lsRelatedProduct.Count > 0)
                {
                    foreach (var item in lsRelatedProduct)
                    {
                        ShopProduct shopProduct = new ShopProduct
                        {
                            Active = (bool)item.Active,
                            Alias = item.Alias,
                            ProductId = item.ProductId,
                            BestSellers = (bool)item.BestSellers,
                            CatId = item.CatId,
                            Description = item.Description,
                            Discount = (int)item.Discount,
                            HomeFlag = (bool)item.HomeFlag,
                            LevelCode = (int)item.LevelCode,
                            ParentId = (int)item.LevelCodeNavigation.ParentId,
                            Price = (int)item.Price,
                            ProductName = item.ProductName,
                            ShortDesc = item.ShortDesc,
                            SoLuongConLai = (int)item.UnitsInStock,
                            Thumb = item.Thumb
                        };
                        lsRelatedShopProduct.Add(shopProduct);
                    }
                }
            }

            var parents = await _dbNet6Context.TbParents.AsNoTracking().Include(x => x.TbLevels).Include(x => x.TbCategories).ToListAsync();
            List<ParentsListResponse> parentList = new List<ParentsListResponse>();
            if (parents != null)
            {
                foreach (var item in parents)
                {
                    ParentsListResponse dtParent = new ParentsListResponse
                    {
                        ParentID = item.ParentId.ToString(),
                        ParentActive = (bool)item.ParentActive,
                        ParentDelete = item.ParentDelete,
                        ParentName = item.ParentName,
                        AmountLevel = item.TbLevels.Count > 0 ? item.TbLevels.Count : 0,
                        AmountCategory = item.TbCategories.Count > 0 ? item.TbCategories.Count : 0
                    };

                    parentList.Add(dtParent);
                }
            }

            var levels = await _dbNet6Context.TbLevels.AsNoTracking().Include(x => x.TbCategories).Include(x => x.TbProducts).ToListAsync();
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

            var categories = await _dbNet6Context.TbCategories.AsNoTracking().Include(x => x.TbProducts).ToListAsync();
            List<CategoriesListResponse> cateList = new List<CategoriesListResponse>();
            if (categories != null)
            {
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

            ShopProductVM shopProductVM = new ShopProductVM();
            shopProductVM.ShopProducts = lsShopProduct;
            shopProductVM.RelatedShopProducts = lsRelatedShopProduct;
            shopProductVM.OrderBy = 0;
            shopProductVM.Parents = parentList;
            shopProductVM.Levels = levelList;
            shopProductVM.Categories = cateList;

            return Ok(shopProductVM);
        }

        [HttpGet]
        [Route("GetDataForShopProductByParentId/{ParentId}")]
        public async Task<IActionResult> GetDataForShopProductByParentId(int ParentId)
        {
            var parent = await _dbNet6Context.TbParents.SingleOrDefaultAsync(x => x.ParentId == ParentId);
            var lsRelatedProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true).Take(5).ToListAsync();

            if (ParentId != 0)
            {
                lsRelatedProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true && x.LevelCodeNavigation.ParentId != parent.ParentId).Take(5).ToListAsync();
            }

            var lsProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true).ToListAsync();

            if (ParentId != 0)
            {
                lsProduct = await _dbNet6Context.TbProducts.AsNoTracking().Include(x => x.Unit).Include(x => x.Cat).Include(x => x.Upay).Include(x => x.LevelCodeNavigation).Where(x => x.Active == true && x.LevelCodeNavigation.ParentId == parent.ParentId).ToListAsync();
            }

            List<ShopProduct> lsShopProduct = new List<ShopProduct>();
            List<ShopProduct> lsRelatedShopProduct = new List<ShopProduct>();

            if (lsProduct != null)
            {
                if (lsProduct.Count > 0)
                {
                    foreach (var item in lsProduct)
                    {
                        ShopProduct shopProduct = new ShopProduct
                        {
                            Active = (bool)item.Active,
                            Alias = item.Alias,
                            ProductId = item.ProductId,
                            BestSellers = (bool)item.BestSellers,
                            CatId = item.CatId,
                            Description = item.Description,
                            Discount = (int)item.Discount,
                            HomeFlag = (bool)item.HomeFlag,
                            LevelCode = (int)item.LevelCode,
                            ParentId = (int)item.LevelCodeNavigation.ParentId,
                            Price = (int)item.Price,
                            ProductName = item.ProductName,
                            ShortDesc = item.ShortDesc,
                            SoLuongConLai = (int)item.UnitsInStock,
                            Thumb = item.Thumb
                        };
                        lsShopProduct.Add(shopProduct);
                    }
                }
            }

            if (lsRelatedProduct != null)
            {
                if (lsRelatedProduct.Count > 0)
                {
                    foreach (var item in lsRelatedProduct)
                    {
                        ShopProduct shopProduct = new ShopProduct
                        {
                            Active = (bool)item.Active,
                            Alias = item.Alias,
                            ProductId = item.ProductId,
                            BestSellers = (bool)item.BestSellers,
                            CatId = item.CatId,
                            Description = item.Description,
                            Discount = (int)item.Discount,
                            HomeFlag = (bool)item.HomeFlag,
                            LevelCode = (int)item.LevelCode,
                            ParentId = (int)item.LevelCodeNavigation.ParentId,
                            Price = (int)item.Price,
                            ProductName = item.ProductName,
                            ShortDesc = item.ShortDesc,
                            SoLuongConLai = (int)item.UnitsInStock,
                            Thumb = item.Thumb
                        };
                        lsRelatedShopProduct.Add(shopProduct);
                    }
                }
            }

            var parents = await _dbNet6Context.TbParents.AsNoTracking().Include(x => x.TbLevels).Include(x => x.TbCategories).ToListAsync();
            List<ParentsListResponse> parentList = new List<ParentsListResponse>();
            if (parents != null)
            {
                foreach (var item in parents)
                {
                    ParentsListResponse dtParent = new ParentsListResponse
                    {
                        ParentID = item.ParentId.ToString(),
                        ParentActive = (bool)item.ParentActive,
                        ParentDelete = item.ParentDelete,
                        ParentName = item.ParentName,
                        AmountLevel = item.TbLevels.Count > 0 ? item.TbLevels.Count : 0,
                        AmountCategory = item.TbCategories.Count > 0 ? item.TbCategories.Count : 0
                    };

                    parentList.Add(dtParent);
                }
            }

            var levels = await _dbNet6Context.TbLevels.AsNoTracking().Include(x => x.TbCategories).Include(x => x.TbProducts).ToListAsync();
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

            var categories = await _dbNet6Context.TbCategories.AsNoTracking().Include(x => x.TbProducts).ToListAsync();
            List<CategoriesListResponse> cateList = new List<CategoriesListResponse>();
            if (categories != null)
            {
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

            ShopProductVM shopProductVM = new ShopProductVM();
            shopProductVM.ShopProducts = lsShopProduct;
            shopProductVM.RelatedShopProducts = lsRelatedShopProduct;
            shopProductVM.OrderBy = 0;
            shopProductVM.Parents = parentList;
            shopProductVM.Levels = levelList;
            shopProductVM.Categories = cateList;

            return Ok(shopProductVM);
        }
    }
}
