using SQLite;

public class Eskaera_Goiburua
{
    [PrimaryKey, AutoIncrement]
    public int Eskaera_kod { get; set; }
    public DateTime Data { get; set; }
    public string Egoera { get; set; }
    public string Bidalketa_Helb { get; set; }
    public int Komertzial_ID { get; set; }
    public int Partner_ID { get; set; }
}
