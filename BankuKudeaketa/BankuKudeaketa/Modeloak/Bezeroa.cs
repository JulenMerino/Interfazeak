using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BankuKudeaketa.Modeloak
{
    /// <summary>
    /// Datubaseko Bezeroaren
    /// </summary>
    public class Bezeroa
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public string Izena { get; set; } = String.Empty;

        public Bezeroa(string izena, string nan)
        {
            Nan = nan;
            Izena = izena;
        }

        public Bezeroa() { }

        public override string ToString()
        {
            return $"{Izena} ({Nan})";
        }

    }
}
