﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Cms;
using Server.Core.Commons;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Implementations;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Cms;

public class NewsTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new NewsRepo(context: context);
        var pageModel = new NewsModel(repo: repo, fileUploader: null);
        
        await context.Database.BeginTransactionAsync();
        var entity = new News
        {
            Title = "1324",
            Slug = "1234",
            ShortDescription = "1234",
            Description = "1234",
            Image = "1234",
            Thumbnail = "1234",
            NewsCategory = new NewsCategory()
            {
                Title = "1234"
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
        var resultVal = (ResponseDto<News>)jsonResult.Value;
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
        var repo = new NewsRepo(context: context);
        var pageModel = new NewsModel(repo: repo, fileUploader: null);
        pageModel.ModelState.AddModelError("Id", "Id is required");
        var routeVal = new IdDto { Id = id };
        
        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<News>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task OnGetByIdAsync_NotFoundId_ReturnsNotFound()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new NewsRepo(context: context);
        var pageModel = new NewsModel(repo: repo, fileUploader: null);
        
        await context.Database.BeginTransactionAsync();
        var entity = new News
        {
            Title = "1324",
            Slug = "1234",
            ShortDescription = "1234",
            Description = "1234",
            Image = "1234",
            Thumbnail = "1234",
            NewsCategory = new NewsCategory()
            {
                Title = "1234"
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
        var resultVal = (ResponseDto<News>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
}