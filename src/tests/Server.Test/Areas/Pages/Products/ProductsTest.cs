using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Products;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Enums;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Implementations;
using Server.Core.Modules.User.Models;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Products;

public class ProductsTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);

        await context.Database.BeginTransactionAsync();


        var entity = new Product
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
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        var routeVal = new IdDto { Id = entity.Id };

        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<Product>)jsonResult.Value;
        Assert.True(resultVal.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public void Id_ShouldFailValidation_WhenIdIsMissing()
    {
        // Arrange
        var dto = new IdDto();

        // Act
        var validationResults = ModelStateValidationHelper.ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Id_ShouldFailValidation_WhenIdIsOutOfRange(int invalidId)
    {
        // Arrange
        var dto = new IdDto { Id = invalidId };

        // Act
        var validationResults = ModelStateValidationHelper.ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
    }

    [Fact]
    public void Id_ShouldPassValidation_WhenIdIsValid()
    {
        // Arrange
        var validId = 1; // A valid value within range
        var dto = new IdDto { Id = validId };

        // Act
        var validationResults = ModelStateValidationHelper.ValidateModel(dto);

        // Assert
        Assert.Empty(validationResults); // No validation errors
    }

    [Fact]
    public async Task OnGetByIdAsync_NotFoundId_ReturnsNotFound()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);

        await context.Database.BeginTransactionAsync();

        var brand = new ProductBrand()
        {
            Title = "1234"
        };
        await context.AddAsync(brand);
        await context.SaveChangesAsync();

        var entity = new Product
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
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        context.Remove(entity);
        await context.SaveChangesAsync();
        var routeVal = new IdDto { Id = entity.Id };

        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<Product>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Fact]
    public async Task OnPostListAsync_VisibleFilter_ReturnsVisibleAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter();

        await context.Database.BeginTransactionAsync();
        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "invisible",
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
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.Visible == false);
    }

    [Fact]
    public async Task OnPostListAsync_InVisibleFilter_ReturnsInVisibleAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = false
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "invisible",
                Visible = false,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = false
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = false
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
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.Visible);
    }

    [Fact]
    public async Task OnPostListAsync_NullVisibleFilter_ReturnsAllAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "invisible",
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
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // act
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
    }

    [Fact]
    public async Task OnPostListAsync_BrandFilter_ReturnsSpecificBrandAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null
        };

        await context.Database.BeginTransactionAsync();

        var brand = new ProductBrand()
        {
            Title = "1234",
            Visible = true
        };

        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = brand,
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "invisible",
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
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // act
        filter.ProductBrandId = brand.Id;
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.ProductBrandId != brand.Id);
    }

    [Fact]
    public async Task OnPostListAsync_CategoryFilter_ReturnsSpecificCategoryAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null
        };

        await context.Database.BeginTransactionAsync();

        var category = new ProductCategory()
        {
            Title = "1234",
            Visible = true
        };

        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = category,
                ProductSeller = new ProductSeller
                {
                    Title = "1324",
                    PhoneNumber = "09191234567",
                    User = new AppUser()
                    {
                        UserName = "1234"
                    }
                }
            },
            new()
            {
                Title = "invisible",
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
        };
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // act
        filter.ProductCategoryId = category.Id;
        var result = await pageModel.OnPostListAsync(filter: filter, dataTableFilter: new DataTableFilter());
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.ProductCategoryId != category.Id);
    }

    [Fact]
    public async Task OnPostListAsync_SpecialOfferFilter_ReturnsSpecialOffersAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null,
            IsSpecialOffer = true
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                IsSpecialOffer = true,
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.IsSpecialOffer == false);
    }

    [Fact]
    public async Task OnPostListAsync_SaleableFilter_ReturnsSaleableAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null,
            Saleable = true
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<Product>()
        {
            new()
            {
                Title = "visible",
                Saleable = true,
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
            },
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.Saleable == false);
    }

    [Fact]
    public async Task OnPostListAsync_DiscountFilter_ReturnsHasDiscountAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null,
            HasDiscount = true
        };

        await context.Database.BeginTransactionAsync();

        var discountItem = new Product()
        {
            Title = "visible",
            IsSpecialOffer = true,
            Visible = true,
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = new List<ProductModel>()
            {
                new()
                {
                    Title = "1324",
                    Value = "1324",
                    Price = 2_000,
                    Type = ProductModelType.Color,
                    Discount = 1_000 // this will cause discount
                }
            },
            ProductStocks = null,
            OrderDetails = null,
            ProductBrand = new ProductBrand()
            {
                Title = "1234",
                Visible = true
            },
            ProductCategory = new ProductCategory()
            {
                Title = "1234",
                Visible = true
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
        
        var entities = new List<Product>()
        {
            discountItem,
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.Contains(resultVal.Data, v => v.Id == discountItem.Id);
    }

    [Fact]
    public async Task OnPostListAsync_AvaiableFilter_ReturnsAvailableAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductRepo(context: context);
        var pageModel = new IndexModel(repo: repo, fileUploader: null);
        var filter = new ProductFilter()
        {
            Visible = null,
            Available = true
        };

        await context.Database.BeginTransactionAsync();

        var productModel = new ProductModel()
        {
            Title = "1324",
            Value = "1324",
            Price = 1,
            Type = ProductModelType.Color
        };

        var availableItem = new Product()
        {
            Title = "visible",
            IsSpecialOffer = true,
            Visible = true,
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = new List<ProductModel>()
            {
                productModel
            },
            ProductStocks = new List<ProductStock>()
            {
                new ProductStock()
                {
                    CreateDate = DateTime.Now,
                    Value = 1,
                    ProductModel = productModel
                },
                new ProductStock()
                {
                    CreateDate = DateTime.Now,
                    Value = 1,
                    ProductModel = productModel
                }
            },
            OrderDetails = null,
            ProductBrand = new ProductBrand()
            {
                Title = "1234",
                Visible = true
            },
            ProductCategory = new ProductCategory()
            {
                Title = "1234",
                Visible = true
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
        
        
        var entities = new List<Product>()
        {
            availableItem,
            new()
            {
                Title = "visible",
                Visible = true,
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand = new ProductBrand()
                {
                    Title = "1234",
                    Visible = true
                },
                ProductCategory = new ProductCategory()
                {
                    Title = "1234",
                    Visible = true
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
        var resultVal = (DataTableResult<Product>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.Contains(resultVal.Data, v => v.Id == availableItem.Id);
    }
}