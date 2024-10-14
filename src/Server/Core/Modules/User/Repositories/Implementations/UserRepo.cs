using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;
using Server.Core.Modules.User.Repositories.Contracts;

namespace Server.Core.Modules.User.Repositories.Implementations;

public class UserRepo(BaseDbContext context) : GenericRepository<AppUser>(context), IUserRepo
{
    private readonly DbSet<AppUser> _entities = context.Users;

    public IQueryable<AppUser> FilterQuery(UserFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.EmailConfirmed is not null)
        {
            query = query.Where(x => x.EmailConfirmed == filter.EmailConfirmed.Value);
        }
        
        if (filter.PhoneNumberConfirmed is not null)
        {
            query = query.Where(x => x.PhoneNumberConfirmed == filter.PhoneNumberConfirmed.Value);
        }
        
        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }

        return query;
    }
    
    private static IQueryable<AppUser> FilterQuery(IQueryable<AppUser> query, DataTableFilter dataTableFilter)
    {
        if (!string.IsNullOrEmpty(dataTableFilter.Search?.Value))
        {
            query = query.Where(x => x.UserName.Contains(dataTableFilter.Search.Value) ||
                                     x.Email.Contains(dataTableFilter.Search.Value) ||
                                     x.PhoneNumber.Contains(dataTableFilter.Search.Value));
        }

        return query;
    }
}
