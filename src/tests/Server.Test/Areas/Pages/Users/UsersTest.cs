using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Areas.Admin.Pages.Users;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;
using Server.Core.Modules.User.Repositories.Implementations;
using Server.Test.Fixtures;

namespace Server.Test.Areas.Pages.Users;

public class UsersTest
{
    [Fact]
    public async Task OnGetByIdAsync_ValidId_ReturnsObject()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new UserRepo(context: context);
        var pageModel = new IndexModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        var entity = new AppUser
        {
            UserName = "1324"
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
        var resultVal = (ResponseDto<AppUser>)jsonResult.Value;
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
        var repo = new UserRepo(context: context);
        var pageModel = new IndexModel(repo: repo);
        
        await context.Database.BeginTransactionAsync();
        var entity = new AppUser
        {
            UserName = "1234"
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
        var resultVal = (ResponseDto<AppUser>)jsonResult.Value;
        Assert.True(resultVal.IsFailed);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
    
    [Fact]
    public async Task OnPostListAsync_DefaultFilters_ReturnsVisibleAndMoreThan1()
    {
        // arrange
        await using var context = TestDatabaseFixture.CreateContext();
        var repo = new UserRepo(context: context);
        var pageModel = new IndexModel(repo: repo);
        var filter = new UserFilter();
        
        await context.Database.BeginTransactionAsync();
        var entities = new List<AppUser>()
        {
            new()
            {
                UserName = "1234"
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
        var resultVal = (DataTableResult<AppUser>?)jsonResult.Value;
        Assert.NotNull(resultVal);
        Assert.NotEmpty(resultVal.Data);
    }
}