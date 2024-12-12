using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BankuKudeaketa.Modeloak
{
    /// <summary>
    /// Datubaseko Gordailuaren Modeloa
    /// </summary>
    public class Gordailua
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public string Deskripzioa { get; set; } = String.Empty;
        public double Saldo { get; set; } = 0;

        public Gordailua(string nan, string deskripzioa, double saldo)
        {
            Nan = nan;
            Deskripzioa = deskripzioa;
            Saldo = saldo;
        }

        public Gordailua() { }

        public override string ToString()
        {
            return $"{Deskripzioa} ({Saldo})";
        }
    }
}
