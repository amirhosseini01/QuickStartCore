using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Repositories.Contracts;

public interface ISliderRepo : IGenericRepository<Slider>
{
    IQueryable<Slider> FilterQuery(SliderFilter filter, DataTableFilter? dataTableFilter = null);
}
