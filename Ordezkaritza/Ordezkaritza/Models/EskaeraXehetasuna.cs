using SQLite;

public class Eskaera_Xehetasuna
{
    [PrimaryKey]
    public int Eskaera_kod { get; set; }
    [PrimaryKey]
    public string Produktu_kod { get; set; }
    public string Deskribapena { get; set; }
    public decimal Prezioa { get; set; }
    public decimal Guztira { get; set; }
    public int Kantitatea { get; set; }
}
