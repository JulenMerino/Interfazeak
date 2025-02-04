using SQLite;

public class Katalogoa
{
    [PrimaryKey]
    public int Produktu_kod { get; set; }
    public string Izena { get; set; }
    public decimal Prezioa { get; set; }
    public int Stock { get; set; }

    [Ignore] 
    public string Irudia { get; set; }

}
