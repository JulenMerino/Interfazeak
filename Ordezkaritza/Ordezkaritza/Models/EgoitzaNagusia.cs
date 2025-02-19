

using SQLite;

namespace Ordezkaritza.Models
{
    public class EgoitzaNagusia
    {
        [PrimaryKey]
        public int Eskaera_kod { get; set; }
        public int Produktu_kod { get; set; }
        public string Izena { get; set; }
        public int Kantitatea { get; set; }
        
    }
}
