using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankuKudeaketa.Modeloak
{
    public class del_Mailegua
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public long Kantitatea { get; set; } = 0;
        public long EpeaHilabeteak { get; set; } = 0;
        public DateTime HasieraData { get; set; } = DateTime.MinValue;
        public string Deskripzioa { get; set; } = String.Empty;

        public DateTime Ezabatua { get; set; } = DateTime.UtcNow;

        public del_Mailegua(Mailegua mailegua)
        {
            Nan = mailegua.Nan;
            Kantitatea = mailegua.Kantitatea;
            EpeaHilabeteak = mailegua.EpeaHilabeteak;
            HasieraData = mailegua.HasieraData;
            Deskripzioa = mailegua.Deskripzioa;
        }

        public del_Mailegua() { }
    }

    public class del_Gordailua
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public string Deskripzioa { get; set; } = String.Empty;
        public double Saldo { get; set; } = 0;
        public DateTime Ezabatua { get; set; } = DateTime.UtcNow;


        public del_Gordailua(Gordailua gordailua)
        {
            Nan = gordailua.Nan;
            Deskripzioa = gordailua.Deskripzioa;
            Saldo = gordailua.Saldo;
        }

        public del_Gordailua() { }
    }
    public class del_Bezeroa
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Nan { get; set; } = String.Empty;
        public string Izena { get; set; } = String.Empty;
        public DateTime Ezabatua { get; set; } = DateTime.UtcNow;

        public del_Bezeroa(Bezeroa bezeroa)
        {
            Nan = bezeroa.Nan;
            Izena = bezeroa.Izena;
        }

        public del_Bezeroa() { }
    }

    }
