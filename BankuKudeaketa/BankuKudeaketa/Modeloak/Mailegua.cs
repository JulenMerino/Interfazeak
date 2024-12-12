using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BankuKudeaketa.Modeloak
{
    /// <summary>
    /// Datubaseko Maileguaren Modeloa
    /// </summary>
    public class Mailegua
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public long Kantitatea { get; set; } = 0;
        public long EpeaHilabeteak { get; set; } = 0;
        public DateTime HasieraData { get; set; } = DateTime.MinValue;
        public string Deskripzioa { get; set; } = String.Empty;

        public Mailegua(string nan, long kantitatea, long epeaHilabeteak, DateTime hasieraData, string deskripzioa)
        {
            Nan = nan;
            Kantitatea = kantitatea;
            EpeaHilabeteak = epeaHilabeteak;
            HasieraData = hasieraData;
            Deskripzioa = deskripzioa;
        }

        public Mailegua() { }

        public override string ToString()
        {
            return $"{Deskripzioa} ({Kantitatea}) {HasieraData} {EpeaHilabeteak}";
        }
    }
}
