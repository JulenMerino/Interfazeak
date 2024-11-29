using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace BankuBatenKudeaketa
{
    class DatuBaseaMetodoak
    {

        private readonly string connectionString = "Server=localhost;Port=3306;Database=bankubatenkudeaketa;User Id=root;Password=mysql;";

        /// <summary>
        /// Datu-base lokalarekin konexioa egiteko metodoa.
        /// </summary>
        /// <returns>
        /// Konexioa lortu bada, datu-basearen eta zerbitzariaren izena adierazten duen mezua.
        /// Errorea gertatuz gero, errorearen mezua itzultzen du.
        /// </returns>
        public async Task<string> KonexioaEginDatuBasearekinAsync()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string database = connection.Database;
                    string server = connection.DataSource;

                    return $"Konektatuta ondorengo datubasera: '{database}' ondorengo zerbitzarian '{server}'";
                }
                catch (Exception ex)
                {
                    return $"Errorea konektatzean: {ex.Message}";
                }
            }
        }



        // Bezero zatia



        /// <summary>
        /// Datu-basean dauden bezeroen kopurua lortzen duen metodoa.
        /// </summary>
        /// <returns>
        /// Bezeroen kopuru osoa zenbakitan itzultzen du. 
        /// Errorea gertatuz gero, 0 itzuliko du.
        /// </returns>
        public int LortuBezeroKopurua()
        {
            int bezeroKopurua = 0;
            string query = "SELECT COUNT(*) FROM bezeroak;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    bezeroKopurua = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea: " + ex.Message);
                }
            }

            return bezeroKopurua;
        }


        /// <summary>
        /// Datu-baseko lerro jakin batean dagoen bezeroaren NAN eta izena lortzen duen metodoa.
        /// </summary>
        /// <param name="lerroa">
        /// Lerro zenbakia (1-indizekoa) bezeroa lortzeko.
        /// </param>
        /// <returns>
        /// NAN eta izena duen tupla bat itzultzen du. 
        /// Lerro horretan bezeroa aurkitzen ez bada edo errorea gertatzen bada, (null, null) itzultzen du.
        /// </returns>
        public async Task<(string NAN, string Izena)> LortuBezeroaLerroarenAraberaAsync(int lerroa)
        {
            string query = "SELECT NAN, Izena FROM bezeroak LIMIT 1 OFFSET @lerroa";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@lerroa", lerroa - 1); 

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string nan = reader["NAN"].ToString();
                            string izena = reader["Izena"].ToString();
                            return (nan, izena);
                        }
                        else
                        {
                            return (null, null);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea: " + ex.Message);
                    return (null, null);
                }
            }
        }


        /// <summary>
        /// Bezero berri bat datu-basean txertatzen duen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Bezeroaren NAN-a (Nortasun Agiri Nazionala).
        /// </param>
        /// <param name="izena">
        /// Bezeroaren izena.
        /// </param>
        /// <returns>
        /// Txertaketa arrakastatsua izan bada, true itzultzen du.
        /// Errorea gertatu bada, false itzultzen du.
        /// </returns>
        public async Task<bool> TxertatuBezeroaAsync(string nan, string izena)
        {
            string query = "INSERT INTO bezeroak (NAN, Izena) VALUES (@nan, @izena);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@izena", izena);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();  
                    return true;  
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea bezeroa txertatzean: " + ex.Message);
                    return false;  
                }
            }
        }


        /// <summary>
        /// Zehaztutako NAN eta izena duen bezeroa datu-basetik ezabatzen duen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Ezabatu nahi den bezeroaren NAN-a.
        /// </param>
        /// <param name="izena">
        /// Ezabatu nahi den bezeroaren izena.
        /// </param>
        /// <returns>
        /// Bezeroa arrakastaz ezabatu bada, true itzultzen du.
        /// Errorea gertatu bada edo bezeroa ez bada aurkitu, false itzultzen du.
        /// </returns>
        public async Task<bool> EzabatuBezeroaAsync(string nan, string izena)
        {
            string query = "DELETE FROM bezeroak WHERE NAN = @nan AND Izena = @izena;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@izena", izena);

                try
                {
                    await connection.OpenAsync();
                    int lerroakKaltetuta = await command.ExecuteNonQueryAsync();  

                    
                    return lerroakKaltetuta > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea bezeroa ezabatzean: " + ex.Message);
                    return false;  
                }
            }
        }


        /// <summary>
        /// Zehaztutako NAN bateko bezeroaren izena eguneratzen duen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Eguneratu nahi den bezeroaren NAN-a.
        /// </param>
        /// <param name="izena">
        /// Bezeroaren eguneratutako izena.
        /// </param>
        /// <returns>
        /// Bezeroa arrakastaz eguneratu bada, true itzultzen du.
        /// Errorea gertatu bada edo bezeroaren NAN-a ez bada aurkitu, false itzultzen du.
        /// </returns>
        public async Task<bool> EguneratuBezeroaAsync(string nan, string izena)
        {
            string query = "UPDATE bezeroak SET Izena = @izena WHERE NAN = @nan;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@izena", izena);

                try
                {
                    await connection.OpenAsync();
                    int lerroakKaltetuta = await command.ExecuteNonQueryAsync();  

                    
                    return lerroakKaltetuta > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea bezeroa eguneratzean: " + ex.Message);
                    return false; 
                }
            }
        }



        // Kontuen kudeaketa zatia



        /// <summary>
        /// Datu-baseko bezero guztiak eta haien NANak jasotzen dituen metodoa.
        /// </summary>
        /// <returns>
        /// Bezero guztien NANak dituen zerrenda bat itzultzen du.
        /// Errorea gertatzen bada, zerrenda hutsa itzultzen du.
        /// </returns>
        public async Task<List<string>> LortuNanGuztiakAsync()
        {
            string query = "SELECT NAN FROM bezeroak;";  

            List<string> nanZerrenda = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    await connection.OpenAsync();  
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  
                        {
                            string nan = reader["NAN"].ToString();  
                            nanZerrenda.Add(nan);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea NANak lortzean: " + ex.Message);
                }
            }

            return nanZerrenda;  
        }


        /// <summary>
        /// Zehaztutako NAN-aren arabera bezeroaren NAN eta izena lortzen dituen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Lortu nahi den bezeroaren NAN-a.
        /// </param>
        /// <returns>
        /// Bezeroaren NAN eta izena dituen tupla bat itzultzen du.
        /// Bezerorik aurkitzen ez bada edo errore bat gertatzen bada, (null, null) itzultzen du.
        /// </returns>
        public async Task<(string NAN, string Izena)> LortuBezeroaNanarenAraberaAsync(string nan)
        {
            string query = "SELECT NAN, Izena FROM bezeroak WHERE NAN = @nan LIMIT 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string izena = reader["Izena"].ToString();
                            return (nan, izena);  
                        }
                        else
                        {
                            return (null, null);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea NANaren arabera bezeroa lortzean: " + ex.Message);
                    return (null, null);  
                }
            }
        }


        /// <summary>
        /// Zehaztutako NAN-aren arabera bezeroari dagozkion kontuen deskripzioak jasotzen dituen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Lortu nahi den bezeroaren NAN-a.
        /// </param>
        /// <returns>
        /// Bezeroaren kontu guztiak dituen deskripzioen zerrenda bat itzultzen du.
        /// Errorea gertatzen bada, zerrenda hutsa itzultzen du.
        /// </returns>
        public async Task<List<string>> LortuKontuakNanarenAraberaAsync(string nan)
        {
            string query = "SELECT Deskripzioa FROM gordailuak WHERE Bezeroak_NAN = @nan;";  

            List<string> kontuZerrenda = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();  
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  
                        {
                            string deskripzioa = reader["Deskripzioa"].ToString();  
                            kontuZerrenda.Add(deskripzioa);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea kontuak lortzean: " + ex.Message);
                }
            }

            return kontuZerrenda;  
        }


        /// <summary>
        /// Bezeroaren NAN-a erabiliz, haren maileguen deskripzioak lortzen dituen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Bezeroaren NAN, maileguak bilatzeko erabiltzen den identifikazioa.
        /// </param>
        /// <returns>
        /// Bezeroaren maileguen deskripzioak dituen zerrenda bat itzultzen du. 
        /// Maileguak aurkitzen ez badira, zerrenda hutsa itzultzen du.
        /// </returns>
        public async Task<List<string>> LortuMaileguakNanarenAraberaAsync(string nan)
        {
            string query = "SELECT Deskripzioa FROM maileguak WHERE Bezeroak_NAN = @nan;";  
            List<string> maileguZerrenda = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();  
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  
                        {
                            string deskripzioa = reader["Deskripzioa"].ToString();  
                            maileguZerrenda.Add(deskripzioa);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea maileguak lortzean: " + ex.Message);
                }
            }

            return maileguZerrenda;  
        }


        /// <summary>
        /// Zehaztutako NAN-aren arabera bezeroari dagozkion maileguen deskripzioak jasotzen dituen metodoa.
        /// </summary>
        /// <param name="nan">
        /// Lortu nahi den bezeroaren NAN-a.
        /// </param>
        /// <returns>
        /// Bezeroaren mailegu guztiak dituen deskripzioen zerrenda bat itzultzen du.
        /// Errorea gertatzen bada, zerrenda hutsa itzultzen du.
        /// </returns>
        public async Task<decimal?> LortuSaldoaDeskripzioarenAraberaAsync(string nan, string deskripzioa)
        {
            string query = "SELECT Saldo FROM gordailuak WHERE Bezeroak_NAN = @PkNan AND Deskripzioa = @deskripzioa;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PkNan", nan);
                command.Parameters.AddWithValue("@deskripzioa", deskripzioa);

                try
                {
                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToDecimal(result) : (decimal?)null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea saldoa lortzean: " + ex.Message);
                    return null;
                }
            }
        }


        /// <summary>
        /// Maileguaren deskripzioaren arabera mailegu bat ezabatzen duen metodoa.
        /// </summary>
        /// <param name="deskripzioa">
        /// Ezabatu nahi den maileguaren deskripzioa.
        /// </param>
        /// <returns>
        /// Deskripzioa duen mailegua ezabatzen den edo ez adierazten duen balio boolearra itzultzen du.
        /// </returns>
        public async Task<bool> EzabatuMaileguaDeskripzioarenAraberaAsync(string deskripzioa)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM maileguak WHERE Deskripzioa = @deskripzioa";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@deskripzioa", deskripzioa);

                        int eragindakoErrenkadak = await command.ExecuteNonQueryAsync();
                        return eragindakoErrenkadak > 0; 
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL errorea: " + ex.Message);
                return false;
            }
        }



        // Gordailu zatia



        /// <summary>
        /// Deskribapenaren arabera gordailu baten zenbatekoa aldatzen duen metodoa.
        /// </summary>
        /// <param name="deskribapena">
        /// Aldatu nahi den gordailuaren deskripzioa.
        /// </param>
        /// <param name="zenbatekoBerria">
        /// Gordailuaren zenbateko berriaren balioa.
        /// </param>
        /// <returns>
        /// Gordailua egoki eguneratzen bada, `true` itzultzen du. 
        /// Akatsen bat gertatzen bada, `false` itzultzen du.
        /// </returns>
        public async Task<bool> AldatuZenbatekoaDeskribapenarenAraberaAsync(string deskribapena, decimal zenbatekoBerria)
        {
            string query = "UPDATE gordailuak SET Saldo = @importe WHERE Deskripzioa = @deskribapena;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@importe", zenbatekoBerria);
                command.Parameters.AddWithValue("@deskribapena", deskribapena);

                try
                {
                    await connection.OpenAsync();
                    int eragindakoErrenkadak = await command.ExecuteNonQueryAsync();
                    return eragindakoErrenkadak > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea zenbatekoa aldatzean: " + ex.Message);
                    return false;
                }
            }
        }


        /// <summary>
        /// Deskribapenaren arabera gordailu bat ezabatzen duen metodoa.
        /// </summary>
        /// <param name="deskribapena">
        /// Ezabatu nahi den gordailuaren deskripzioa.
        /// </param>
        /// <returns>
        /// Gordailua egoki ezabatzen bada, `true` itzultzen du. 
        /// Akatsen bat gertatzen bada, `false` itzultzen du.
        /// </returns>
        public async Task<bool> EzabatuGordailuaDeskribapenarenAraberaAsync(string deskribapena)
        {
            string query = "DELETE FROM gordailuak WHERE Deskribapena = @deskribapena;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@deskribapena", deskribapena);

                try
                {
                    await connection.OpenAsync();
                    int eragindakoErrenkadak = await command.ExecuteNonQueryAsync();
                    return eragindakoErrenkadak > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea gordailua ezabatzean: " + ex.Message);
                    return false;
                }
            }
        }



        // Imprimatu zatia



        /// <summary>
        /// Bezeroaren NAN eta deskribapenaren arabera maileguaren datuak lortzen dituen metodoa.
        /// </summary>
        /// <param name="pkNan">
        /// Mailegua duen bezeroaren NAN.
        /// </param>
        /// <param name="deskribapena">
        /// Lortu nahi den maileguaren deskripzioa.
        /// </param>
        /// <returns>
        /// Maileguaren deskribapena, zenbatekoa, epea (hilabeteetan), eta hasiera data itzultzen ditu.
        /// Mailegua ez badago, `null`, `0`, `0` eta `DateTime.MinValue` itzultzen ditu.
        /// </returns>
        public async Task<(string Deskribapena, decimal Zenbatekoa, int Epea, DateTime Data)> LortuMaileguaNanEtaDeskribapenarenAraberaAsync(string pkNan, string deskribapena)
        {
            string query = @"SELECT Deskripzioa, Kantitatea, EpeHilabete, HasieraData 
                     FROM Maileguak 
                     WHERE Bezeroak_NAN = @PkNan AND Deskripzioa = @Deskribapena";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PkNan", pkNan);
                command.Parameters.AddWithValue("@Deskribapena", deskribapena);

                try
                {
                    await connection.OpenAsync();
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            
                            string deskribapenaEmaitza = reader["Deskripzioa"] != DBNull.Value ? reader["Deskripzioa"].ToString() : null;
                            decimal zenbatekoaEmaitza = reader["Kantitatea"] != DBNull.Value ? reader.GetDecimal("Kantitatea") : 0.0m;
                            int epeaEmaitza = reader["EpeHilabete"] != DBNull.Value ? reader.GetInt32("EpeHilabete") : 0;
                            DateTime dataEmaitza = reader["HasieraData"] != DBNull.Value ? reader.GetDateTime("HasieraData") : DateTime.MinValue;

                            return (deskribapenaEmaitza, zenbatekoaEmaitza, epeaEmaitza, dataEmaitza);
                        }
                        else
                        {
                            return (null, 0, 0, DateTime.MinValue); 
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea mailegua lortzean: " + ex.Message);
                    return (null, 0, 0, DateTime.MinValue);
                }
            }
        }
    }
}
