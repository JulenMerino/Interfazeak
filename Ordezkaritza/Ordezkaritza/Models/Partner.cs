using SQLite;

public class Partner
{
    [PrimaryKey, AutoIncrement]
    public int Partner_ID { get; set; }
    public string Izena { get; set; }
    public string Helbidea { get; set; }
    public string Telefonoa { get; set; }
    public string Egoera { get; set; }
    public int ID_komertzial { get; set; }
}
