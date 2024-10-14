namespace Server.Core.Commons.Datatables;

public class DataTableFilter
{
    public int Draw { get; set; }
    public List<DataTableColumns> Columns { get; set; }
    public List<DataTableColumnOrders> Order { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public DataTableColumnSearch Search { get; set; }
}

public class DataTableColumns
{
    public string Data { get; set; }
    public string Name { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
    public DataTableColumnSearch Search { get; set; }
}
public class Test
{

}

public class DataTableColumnSearch
{
    public string Value { get; set; }
    public bool Regex { get; set; }
}

public class DataTableColumnOrders
{
    public int Column { get; set; }
    public string Dir { get; set; }
}