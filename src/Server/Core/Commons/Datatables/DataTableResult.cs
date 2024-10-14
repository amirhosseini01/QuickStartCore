namespace Server.Core.Commons.Datatables;

public class DataTableResult<T>
{
    public int Draw { get; set; }

    public int RecordsFiltered { get; set; }

    public int RecordsTotal { get; set; }

    public List<T> Data { get; set; }
}