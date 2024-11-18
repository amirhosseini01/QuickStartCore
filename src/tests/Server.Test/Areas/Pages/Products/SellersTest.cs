﻿using System.Collections.Specialized;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Server.Areas.Admin.Pages.Products;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Implementations;
using Server.Core.Modules.User.Models;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Products;

public class SellersTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductSellerRepo(context: context);
        var pageModel = new SellersModel(repo: repo, fileUploader: null);
        
        await context.Database.BeginTransactionAsync();
        
        var entity = new ProductSeller
        {
            Title = "1324",
            PhoneNumber = "09191234567",
            User = new AppUser()
            {
                UserName = "1234"
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
        var resultVal = (ResponseDto<ProductSeller>)jsonResult.Value;
        Assert.True(resultVal.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task OnGetByIdAsync_InvalidId_ReturnsInValid(int id)
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductSellerRepo(context: context);
        var pageModel = new SellersModel(repo: repo, fileUploader: null);
        pageModel.ModelState.AddModelError("Id", "Id is required");
        var routeVal = new IdDto { Id = id };
        
        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<ProductSeller>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task OnGetByIdAsync_NotFoundId_ReturnsNotFound()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new ProductSellerRepo(context: context);
        var pageModel = new SellersModel(repo: repo, fileUploader: null);
        
        await context.Database.BeginTransactionAsync();
        
        var entity = new ProductSeller
        {
            Title = "1324",
            PhoneNumber = "09191234567",
            User = new AppUser()
            {
                UserName = "1234"
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
        var resultVal = (ResponseDto<ProductSeller>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
}