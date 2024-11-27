using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Products;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Enums;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Implementations;
using Server.Core.Modules.User.Models;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Products;

public class StocksTest
{
    [Fact]
    public async Task OnPostListAsync_DefaultFilters_ReturnsAllAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductStockRepo(context: context);
        var pageModel = new StocksModel(repo: repo);
        var filter = new ProductStockFilter();

        var product = new Product
        {
            Title = "1324",
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = null,
            ProductStocks = null,
            OrderDetails = null,
            ProductBrand = new ProductBrand()
            {
                Title = "1234"
            },
            ProductCategory = new ProductCategory()
            {
                Title = "1234"
            },
            ProductSeller = new ProductSeller
            {
                Title = "1324",
                PhoneNumber = "09191234567",
                User = new AppUser()
                {
                    UserName = "1234"
                }
            }
        };
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductStock>()
        {
            new ProductStock()
            {
                CreateDate = DateTime.Now,
                User = new AppUser()
                {
                    UserName = "1234"
                },
                ProductModel = new()
                {
                    Title = "1324",
                    Value = "1324",
                    Price = 1,
                    Type = ProductModelType.Color,
                    Product = product
                },
                Product = product
            },
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
        
        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();
        
        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<ProductStock>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
    }
    
    [Fact]
    public async Task OnPostListAsync_ProductFilter_ReturnsSpecificProductAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductStockRepo(context: context);
        var pageModel = new StocksModel(repo: repo);
        var filter = new ProductStockFilter();

        var product = new Product
        {
            Title = "1324",
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = null,
            ProductStocks = null,
            OrderDetails = null,
            ProductBrand = new ProductBrand()
            {
                Title = "1234"
            },
            ProductCategory = new ProductCategory()
            {
                Title = "1234"
            },
            ProductSeller = new ProductSeller
            {
                Title = "1324",
                PhoneNumber = "09191234567",
                User = new AppUser()
                {
                    UserName = "1234"
                }
            }
        };
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductStock>()
        {
            new ProductStock()
            {
                CreateDate = DateTime.Now,
                User = new AppUser()
                {
                    UserName = "1234"
                },
                ProductModel = new()
                {
                    Title = "1324",
                    Value = "1324",
                    Price = 1,
                    Type = ProductModelType.Color,
                    Product = product
                },
                Product = product
            },
            new ProductStock()
            {
                CreateDate = DateTime.Now,
                User = new AppUser()
                {
                    UserName = "1234"
                },
                ProductModel = new()
                {
                    Title = "1324",
                    Value = "1324",
                    Price = 1,
                    Type = ProductModelType.Color,
                    Product = product
                },
                Product = new Product
                {
                    Title = "1324",
                    ShortDescription = "1234",
                    Description = "1234",
                    ProductComments = null,
                    ProductModels = null,
                    ProductStocks = null,
                    OrderDetails = null,
                    ProductBrand = new ProductBrand()
                    {
                        Title = "1234"
                    },
                    ProductCategory = new ProductCategory()
                    {
                        Title = "1234"
                    },
                    ProductSeller = new ProductSeller
                    {
                        Title = "1324",
                        PhoneNumber = "09191234567",
                        User = new AppUser()
                        {
                            UserName = "1234"
                        }
                    }
                }
            },
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        filter.ProductId = product.Id;
        
        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();
        
        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<ProductStock>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v=> v.ProductId != product.Id);
    }
    
    [Fact]
    public async Task OnPostListAsync_ModelFilter_ReturnsSpecificModelAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductStockRepo(context: context);
        var pageModel = new StocksModel(repo: repo);
        var filter = new ProductStockFilter();

        var product = new Product
        {
            Title = "1324",
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = null,
            ProductStocks = null,
            OrderDetails = null,
            ProductBrand = new ProductBrand()
            {
                Title = "1234"
            },
            ProductCategory = new ProductCategory()
            {
                Title = "1234"
            },
            ProductSeller = new ProductSeller
            {
                Title = "1324",
                PhoneNumber = "09191234567",
                User = new AppUser()
                {
                    UserName = "1234"
                }
            }
        };

        var productModel = new ProductModel()
        {
            Title = "1324",
            Value = "1324",
            Price = 1,
            Type = ProductModelType.Color,
            Product = product
        };
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductStock>()
        {
            new ProductStock()
            {
                CreateDate = DateTime.Now,
                User = new AppUser()
                {
                    UserName = "1234"
                },
                ProductModel = productModel,
                Product = product
            },
            new ProductStock()
            {
                CreateDate = DateTime.Now,
                User = new AppUser()
                {
                    UserName = "1234"
                },
                ProductModel = new()
                {
                    Title = "1324",
                    Value = "1324",
                    Price = 1,
                    Type = ProductModelType.Color,
                    Product = product
                },
                Product = new Product
                {
                    Title = "1324",
                    ShortDescription = "1234",
                    Description = "1234",
                    ProductComments = null,
                    ProductModels = null,
                    ProductStocks = null,
                    OrderDetails = null,
                    ProductBrand = new ProductBrand()
                    {
                        Title = "1234"
                    },
                    ProductCategory = new ProductCategory()
                    {
                        Title = "1234"
                    },
                    ProductSeller = new ProductSeller
                    {
                        Title = "1324",
                        PhoneNumber = "09191234567",
                        User = new AppUser()
                        {
                            UserName = "1234"
                        }
                    }
                }
            },
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        filter.ProductModelId = productModel.Id;
        
        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();
        
        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<ProductStock>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v=> v.ProductModelId != productModel.Id);
    }
}