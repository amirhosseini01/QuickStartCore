using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;

namespace Server.Core.Modules.Cms.Repositories.Implementations;

public class SliderRepo(BaseDbContext context) : GenericRepository<Slider>(context), ISliderRepo
{
    private readonly DbSet<Slider> _entities = context.Sliders;

    public IQueryable<Slider> FilterQuery(SliderFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.Visible is not null)
        {
            query = query.Where(x => x.Visible == filter.Visible.Value);
        }
        
        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }

        return query;
    }
    
    private static IQueryable<Slider> FilterQuery(IQueryable<Slider> query, DataTableFilter dataTableFilter)
    {
        if (!string.IsNullOrEmpty(dataTableFilter.Search?.Value))
        {
            query = query.Where(x => x.Title.Contains(dataTableFilter.Search.Value));
        }

        return query;
    }
}
