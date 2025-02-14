
using SQLite;

namespace Ordezkaritza.Models
{
    public class Bidalketa
    {
        [PrimaryKey, AutoIncrement]
        public int BidalketaID { get; set; }
        public string Enpresa_izena { get; set; }
        public string Data { get; set; }
        public int Eskaera_kod { get; set; }
    }
}
