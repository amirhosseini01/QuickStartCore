using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Server.Core.Commons;

public class IdDto
{
    [FromQuery]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int Id { get; set; }
}
