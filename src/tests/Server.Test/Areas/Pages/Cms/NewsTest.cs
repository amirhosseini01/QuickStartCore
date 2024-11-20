using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Cms;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Cms.Dto;
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
    
    [Fact]
    public async Task OnPostListAsync_DefaultFilters_ReturnsVisibleAndMoreThan1()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new NewsRepo(context: context);
        var pageModel = new NewsModel(repo: repo, fileUploader: null);
        var filter = new NewsFilter();
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<News>()
        {
            new()
            {
                Title = "visible",
                Visible = true,
                Slug = "1234",
                ShortDescription = "1234",
                Description = "1234",
                Image = "1234",
                Thumbnail = "1234",
                NewsCategory = new NewsCategory()
                {
                    Title = "1234"
                }
            },
            new()
            {
                Title = "invisible",
                Visible = false,
                Slug = "1234",
                ShortDescription = "1234",
                Description = "1234",
                Image = "1234",
                Thumbnail = "1234",
                NewsCategory = new NewsCategory()
                {
                    Title = "1234"
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
        var resultVal = (DataTableResult<News>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
        Assert.DoesNotContain(resultVal.Data, v=> v.Visible == false);
    }
}