using System.ComponentModel.DataAnnotations;

namespace Server.Core.Data;

public class BaseEntity<T>
{
    [Key]
    public T Id { get; set; }
}

public class BaseEntity : BaseEntity<int>
{
}