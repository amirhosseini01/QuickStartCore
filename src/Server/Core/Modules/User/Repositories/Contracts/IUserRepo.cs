using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.User.Repositories.Contracts;

public interface IUserRepo : IGenericRepository<AppUser>
{
    IQueryable<AppUser> FilterQuery(UserFilter filter, DataTableFilter? dataTableFilter = null);
}
