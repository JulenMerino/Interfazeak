using SQLite;

public class Eskaera_Xehetasuna
{
    [PrimaryKey, AutoIncrement]
    public int Eskaera_kod { get; set; }
    public string Produktu_kod { get; set; }
    public string Deskribapena { get; set; }
    public decimal Prezioa { get; set; }
    public decimal Guztira { get; set; }
}
