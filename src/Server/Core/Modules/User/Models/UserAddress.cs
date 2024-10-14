using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Sale.Models;

namespace Server.Core.Modules.User.Models;

public class UserAddress : BaseEntity
{
    public int UserId { get; set; }

    [StringLength(ModelStatics.TitleRequiredLength)]
    public string Title { get; set; }

    [StringLength(ModelStatics.PhoneNumberRequiredLength)]
    public string PhoneNumber { get; set; }

    [StringLength(ModelStatics.PhoneNumberRequiredLength)]
    public string Tel { get; set; }

    [StringLength(ModelStatics.DescriptionRequiredLength)]
    public string? PostalAddress { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public AppUser User { get; set; }

    public ICollection<Order> Orders { get; }
}
