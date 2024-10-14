using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Repositories.Contracts;

public interface INewsCategoryRepo : IGenericRepository<NewsCategory>
{
    IQueryable<NewsCategory> FilterQuery(NewsCategoryFilter filter, DataTableFilter? dataTableFilter = null);
}
