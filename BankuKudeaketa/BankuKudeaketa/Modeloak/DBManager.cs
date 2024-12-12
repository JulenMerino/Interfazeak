using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BankuKudeaketa.Modeloak
{
    public class DBManager
    {
        public SQLiteConnection Db { get; private set; }
        public static DBManager Instantzia { get; } = new DBManager();

        private string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BankuaApp\\BankuKudeaketa.db");

        private DBManager()
        {
            
            var directoryPath = Path.GetDirectoryName(databasePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.WriteLine("Directory created: " + directoryPath);
            }

            
            if (!File.Exists(databasePath))
            {
                File.Create(databasePath).Dispose();
                Debug.WriteLine("Database file created: " + databasePath);
            }

            Db = new SQLiteConnection(databasePath);
            Debug.WriteLine("Database connection established: " + Db.DatabasePath);

            Db.CreateTable<Bezeroa>();
            Db.CreateTable<Gordailua>();
            Db.CreateTable<Mailegua>();
            Db.CreateTable<del_Bezeroa>();
            Db.CreateTable<del_Gordailua>();
            Db.CreateTable<del_Mailegua>();

            if (Db.Table<Bezeroa>().Count() == 0)
            {
                Debug.WriteLine("Inserting initial data into Bezeroa table");
                SortuBezeroak();
            }
            if (Db.Table<Gordailua>().Count() == 0)
            {
                Debug.WriteLine("Inserting initial data into Gordailua table");
                SortuGordailuak();
            }
            if (Db.Table<Mailegua>().Count() == 0)
            {
                Debug.WriteLine("Inserting initial data into Mailegua table");
                SortuMaileguak();
            }
        }

        /// <summary>
        /// Bezeroen datuak hasieratzeko metodoa
        /// </summary>
        private void SortuBezeroak()
        {
            Db.Insert(new Bezeroa("Pep Larruquert", "15957912Y"));
            Db.Insert(new Bezeroa("Ana González", "15957913F"));
            Db.Insert(new Bezeroa("Iker Aristi", "15957925L"));
            Db.Insert(new Bezeroa("Juan Ignacio Sarasola", "15958461A"));
            Db.Insert(new Bezeroa("Unai Granado", "72523835M"));
            Debug.WriteLine("Sartu hasierako data bezeroa");
        }

        /// <summary>
        /// Gordailuen datuak hasieratzeko metodoa
        /// </summary>
        private void SortuGordailuak()
        {
            Db.Insert(new Gordailua("15957913F", "Kontu Korrontea", 270.5));
            Db.Insert(new Gordailua("15957913F", "Arriskua Inbertitzeko Funtsa", 35000));
            Db.Insert(new Gordailua("15957913F", "Aurreikuspen-funtsa", 25679.2));
            Db.Insert(new Gordailua("15957913F", "Eperako inbertsioa", 50000));
            Db.Insert(new Gordailua("15957912Y", "Kontu Korrontea", 540.1));
            Db.Insert(new Gordailua("15957925L", "Pentsio Aseguratuen Funtsa", 43000));
            Db.Insert(new Gordailua("15957912Y", "Eperako inbertsioa",+ 30000));
            Db.Insert(new Gordailua("72523835M", "Kontu Korrontea", 99999));
            Debug.WriteLine("Satu hasierako data gordailua");
        }

        /// <summary>
        /// maileguen datuak hasieratzeko metodoa
        /// </summary>
        private void SortuMaileguak()
        {
            Db.Insert(new Mailegua("15957925L", 200000, 240, new DateTime(2014, 10, 08), "Hipoteka"));
            Db.Insert(new Mailegua("15957925L", 10000, 60, new DateTime(2015, 02, 11), "Kotxea"));
            Db.Insert(new Mailegua("15958461A", 5000, 48, new DateTime(2015, 02, 09), "Reformak"));
            Debug.WriteLine("Sartu hasierako data maileguak tablan");
        }
    }
}
