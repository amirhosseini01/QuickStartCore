﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Server.Areas.Admin.Pages.Products;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Implementations;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Products;

public class BrandsTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductBrandRepo(context: context);
        var pageModel = new BrandsModel(repo: repo, fileUploader: null);

        await context.Database.BeginTransactionAsync();
        var entity = new ProductBrand
        {
            Title = "1324"
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
        var resultVal = (ResponseDto<ProductBrand>)jsonResult.Value;
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
        var repo = new ProductBrandRepo(context: context);
        var pageModel = new BrandsModel(repo: repo, fileUploader: null);

        await context.Database.BeginTransactionAsync();
        var entity = new ProductBrand
        {
            Title = "1324"
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
        var resultVal = (ResponseDto<ProductBrand>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Fact]
    public async Task OnPostListAsync_VisibleFilter_ReturnsVisibleAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductBrandRepo(context: context);
        var pageModel = new BrandsModel(repo: repo, fileUploader: null);
        var filter = new ProductBrandFilter();

        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductBrand>()
        {
            new()
            {
                Title = "visible",
                Visible = true
            },
            new()
            {
                Title = "invisible",
                Visible = false
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
        var resultVal = (DataTableResult<ProductBrand>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.Visible == false);
    }

    [Fact]
    public async Task OnPostListAsync_InVisibleFilter_ReturnsInVisibleAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductBrandRepo(context: context);
        var pageModel = new BrandsModel(repo: repo, fileUploader: null);
        var filter = new ProductBrandFilter()
        {
            Visible = false
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductBrand>()
        {
            new()
            {
                Title = "visible",
                Visible = true
            },
            new()
            {
                Title = "invisible",
                Visible = false
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
        var resultVal = (DataTableResult<ProductBrand>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v => v.Visible);
    }

    [Fact]
    public async Task OnPostListAsync_NullVisibleFilters_ReturnsAllAndNotEmpty()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductBrandRepo(context: context);
        var pageModel = new BrandsModel(repo: repo, fileUploader: null);
        var filter = new ProductBrandFilter()
        {
            Visible = null
        };

        await context.Database.BeginTransactionAsync();
        var entities = new List<ProductBrand>()
        {
            new()
            {
                Title = "visible",
                Visible = true
            },
            new()
            {
                Title = "invisible",
                Visible = false
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
        var resultVal = (DataTableResult<ProductBrand>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.Contains(resultVal.Data, v => v.Visible == false);
        Assert.Contains(resultVal.Data, v => v.Visible);
    }
    
    [Fact]
    public async Task OnPostAddAsync_ValidInputWithFile_ReturnsSuccess()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductBrandRepo(context: context);
        var file = MockIFormFile.CreateMockFormFile("test.txt", "text/plain", "Mock content");
        var fileUploader = Substitute.For<IFileUploader>();
        fileUploader.UploadFile(file).Returns(ResponseBase.Success<string>());
        var pageModel = new BrandsModel(repo: repo, fileUploader: fileUploader);
        var input = new ProductBrandInput()
        {
            Visible = true,
            Title = "visible",
            LogoFile = file,
        };
        await context.Database.BeginTransactionAsync();
        
        
        // act
        var result = await pageModel.OnPostAddAsync(input: input);
        context.ChangeTracker.Clear();
        
        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<string>)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.True(resultVal.IsSuccess);
        Assert.False(resultVal.IsFailed);
        Assert.Null(resultVal.Messages);
    }
}