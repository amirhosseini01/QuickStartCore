using System.Collections.Specialized;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Server.Areas.Admin.Pages.Products;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Enums;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Implementations;
using Server.Core.Modules.User.Models;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Products;

public class ModelsTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductModelRepo(context: context);
        var pageModel = new ModelsModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        
        var entity = new ProductModel
        {
            Title = "1324",
            Value = "1324",
            Price = 1,
            Type = ProductModelType.Color,
            Product = new Product
            {
                Title = "1324",
                ShortDescription = "1234",
                Description = "1234",
                ProductComments = null,
                ProductModels = null,
                ProductStocks = null,
                OrderDetails = null,
                ProductBrand  = new ProductBrand()
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
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        var routeVal = new IdDto { Id = entity.Id };
        
        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);
        context.ChangeTracker.Clear();

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<ProductModel>)jsonResult.Value;
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
    [InlineData(- 1)]
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
        var repo = new ProductModelRepo(context: context);
        var pageModel = new ModelsModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        
        var brand = new ProductBrand()
        {
            Title = "1234"
        };
        await context.AddAsync(brand);
        await context.SaveChangesAsync();
        
        var product = new Product
        {
            ProductBrandId = brand.Id,
            Title = "1324",
            ShortDescription = "1234",
            Description = "1234",
            ProductComments = null,
            ProductModels = null,
            ProductStocks = null,
            OrderDetails = null,
            ProductBrand  = new ProductBrand()
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
        await context.AddAsync(product);
        await context.SaveChangesAsync();
        
        var entity = new ProductModel
        {
            Title = "1324",
            Value = "1324",
            Price = 1,
            Type = ProductModelType.Color,
            ProductId = product.Id
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
        var resultVal = (ResponseDto<ProductModel>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
    
    [Fact]
    public async Task OnPostListAsync_DefaultFilters_ReturnsVisibleAndMoreThan1()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductModelRepo(context: context);
        var pageModel = new ModelsModel(repo: repo);
        var filter = new ProductModelFilter();
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductModel>()
        {
            new()
            {
                Title = "1324",
                Value = "1324",
                Price = 1,
                Type = ProductModelType.Color,
                Product = new Product
                {
                    Title = "1324",
                    ShortDescription = "1234",
                    Description = "1234",
                    ProductComments = null,
                    ProductModels = null,
                    ProductStocks = null,
                    OrderDetails = null,
                    ProductBrand  = new ProductBrand()
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
        var resultVal = (DataTableResult<ProductModel>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
    }
}