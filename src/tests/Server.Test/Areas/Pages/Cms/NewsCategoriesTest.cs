using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Cms;
using Server.Core.Commons;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Implementations;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Cms;

public class NewsCategoriesTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new NewsCategoryRepo(context: context);
        var pageModel = new NewsCategoriesModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        var entity = new NewsCategory
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
        var resultVal = (ResponseDto<NewsCategory>)jsonResult.Value;
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
        var repo = new NewsCategoryRepo(context: context);
        var pageModel = new NewsCategoriesModel(repo: repo);
        pageModel.ModelState.AddModelError("Id", "Id is required");
        var routeVal = new IdDto { Id = id };
        
        // act
        var result = await pageModel.OnGetByIdAsync(routeVal: routeVal);

        // assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        var resultVal = (ResponseDto<NewsCategory>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task OnGetByIdAsync_NotFoundId_ReturnsNotFound()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new NewsCategoryRepo(context: context);
        var pageModel = new NewsCategoriesModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        var entity = new NewsCategory
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
        var resultVal = (ResponseDto<NewsCategory>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
}