using SQLite;

public class Komertziala
{
    [PrimaryKey, AutoIncrement]
    public int ID_komertzial { get; set; }
    public string Izena { get; set; }
    public string Eremua { get; set; }
    public string Telefonoa { get; set; }
    public string Posta_helbidea { get; set; }
}
