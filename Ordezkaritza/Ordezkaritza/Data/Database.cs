
using Ordezkaritza.Models;
using SQLite;
using System.Diagnostics;
using System.Xml.Linq;


namespace Ordezkaritza.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;
        private static string _dbPath;

        public Database(string dbPath)
        {
            _dbPath = dbPath ?? Path.Combine(FileSystem.AppDataDirectory, "Komertzialak.db");

            // Debug.WriteLine($" Ruta de la base de datos: {Path.GetFullPath(_dbPath)}");


            if (!File.Exists(_dbPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("Komertzialak.db").Result;
                using var fileStream = File.Create(_dbPath);
                stream.CopyTo(fileStream);
            }

            _database = new SQLiteAsyncConnection(_dbPath);


            // Datu basearen taulak sortzeko 
            _database.CreateTableAsync<Katalogoa>().Wait();
            _database.CreateTableAsync<Komertziala>().Wait();
            _database.CreateTableAsync<Partner>().Wait();
            _database.CreateTableAsync<Eskaera_Goiburua>().Wait();
            _database.CreateTableAsync<Eskaera_Xehetasuna>().Wait();
            _database.CreateTableAsync<Bidalketa>().Wait();
            _database.CreateTableAsync<EgoitzaNagusia>().Wait();


        }

        // Erregistroak lortzeko metodoak
        public Task<List<Katalogoa>> GetAllKatalogoasAsync() => _database.Table<Katalogoa>().ToListAsync();
        public Task<List<Partner>> GetAllPartnersAsync() => _database.Table<Partner>().ToListAsync();
        public Task<List<EgoitzaNagusia>> GetAllEgoitzaNagusiaAsync() => _database.Table<EgoitzaNagusia>().ToListAsync();

        // Erregistroak sartzeko metodoak
        public Task<int> InsertEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.InsertAsync(eskaeraGoiburua);
        public Task<int> InsertEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.InsertAsync(eskaeraXehetasuna);
        public Task<int> InsertBidalketaAsync(Bidalketa bidalketa) => _database.InsertAsync(bidalketa);
        public Task<int> InsertEgoitzaNagusiaAsync(EgoitzaNagusia egoitzaNagusia) => _database.InsertAsync(egoitzaNagusia);

        // Erregistroak eguneratzeko metodoak
        public Task<int> UpdateKatalogoaAsync(Katalogoa katalogoa) => _database.UpdateAsync(katalogoa);



        //XML zatia

        /// <summary>
        /// XML fitxategia aukeratzeko metodoa
        /// </summary>
        /// <returns>XML fitxategiaren helbidea</returns>
        public async Task<string> AuketatuXmlFitxategia()
        {

            var xmlFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".xml" } }
            });
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = xmlFileType
            });

            return result?.FullPath;
        }

        /// <summary>
        /// katalogoko informazioa XML fitxategitik lortzeko metodoa
        /// </summary>
        /// <param name="fitxategiHelbidea"></param>
        /// <returns>Produktu bakoitzaren informazioa</returns>
        public async Task<List<Katalogoa>> KatalogoaEguneratuXML(string fitxategiHelbidea)
        {
            if (string.IsNullOrWhiteSpace(fitxategiHelbidea)) return null;

            var edikiXml = await File.ReadAllTextAsync(fitxategiHelbidea);
            var xdoc = XDocument.Parse(edikiXml);

            var infromazioLista = xdoc.Descendants("Producto")
                .Select(x => new Katalogoa
                {
                    Produktu_kod = (int)x.Element("Codigo"),
                    Izena = (string)x.Element("Nombre"),
                    Prezioa = (decimal)x.Element("Precio"),
                    Stock = (int)x.Element("Stock")
                }).ToList();

            return infromazioLista;
        }

        /// <summary>
        /// partnerren informazioa XML fitxategitik lortzeko metodoa
        /// </summary>
        /// <param name="fitxategiHelbidea"></param>
        /// <returns>Partner bakoitzaren informazioa</returns>
        public async Task<List<Partner>> PartneraEguneratuXML(string fitxategiHelbidea)
        {
            if (string.IsNullOrWhiteSpace(fitxategiHelbidea)) return null;

            var edikiXml = await File.ReadAllTextAsync(fitxategiHelbidea);
            var xdoc = XDocument.Parse(edikiXml);

            var infromazioLista = xdoc.Descendants("partner")
                .Select(x => new Partner
                {
                    Izena = (string)x.Element("nombre"),
                    Helbidea = (string)x.Element("direccion"),
                    Telefonoa = (string)x.Element("telefono"),
                    Egoera =(string)x.Element("estado"),
                    ID_komertzial = (int)x.Element("idComercial")
                }).ToList();

            return infromazioLista;
        }


        /// <summary>
        /// Informnazioa XML fitxategitik datu basean gordetzeko metodoa
        /// </summary>
        /// <param name="fitxategiHelbidea"></param>
        public async Task SaveDataFromXmlAsync(string fitxategiHelbidea)
        {
            var fitxategiIzena = Path.GetFileName(fitxategiHelbidea);

            //Debug.WriteLine($"XML fitxategia: {Path.GetFileName(filePath)}");

            if (fitxategiIzena == "EgoitzaNagusia.xml")
            {
                var data = await KatalogoaEguneratuXML(fitxategiHelbidea);
                if (data == null || data.Count == 0)
                {
                    // Debug.WriteLine("No hay datos para guardar en la base de datos.");
                    return;
                }

                foreach (var item in data)
                {
                    try
                    {
                        // Verificar si el producto ya existe en la base de datos
                        var existingItem = await _database.Table<Katalogoa>()
                            .FirstOrDefaultAsync(x => x.Produktu_kod == item.Produktu_kod);

                        if (existingItem != null)
                        {
                            // Si existe, sumamos la cantidad
                            existingItem.Stock += item.Stock;
                            await _database.UpdateAsync(existingItem);
                            // Debug.WriteLine($"Actualizado en BD: {item.Produktu_kod}, Stock actualizado: {existingItem.Stock}");
                        }
                        else
                        {
                            // Si no existe, insertamos el nuevo producto
                            await _database.InsertAsync(item);
                            // Debug.WriteLine($"Guardado en BD: {item.Produktu_kod}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Debug.WriteLine($"Error al guardar {item.Produktu_kod}: {ex.Message}");
                    }
                }
            }

            else if (fitxategiIzena == "partner_berriak.xml")
            {
                var data = await PartneraEguneratuXML(fitxategiHelbidea);
                if (data == null || data.Count == 0)
                {
                    //Debug.WriteLine("No hay datos para guardar en la base de datos.");
                    return;
                }

                foreach (var item in data)
                {
                    try
                    {
                        int result = await _database.InsertAsync(item);
                        //Debug.WriteLine($"Guardado en BD: {item.Izena}, Filas afectadas: {result}");
                    }
                    catch (Exception ex)
                    {
                        //Debug.WriteLine($"Error al guardar {item.Izena }: {ex.Message}");
                    }
                }

            }

        }






        //Eskaera zatia

        /// <summary>
        /// Eskaera kodea lortuz, bidalketa egitko informazioa lortzeko metodoa
        /// </summary>
        /// <param name="eskaeraKod"></param>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<Bidalketa> GetBidalketaByEskaeraKodAsync(int eskaeraKod)
        {
            return await _database.Table<Bidalketa>()
                .Where(b => b.Eskaera_kod == eskaeraKod)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// eskaera kodea lortuz, eskaera xehetasunak lortzeko metodoa
        /// </summary>
        /// <param name="eskaeraKod"></param>
        /// <returns>Datu baseko informazioa</returns>
        public Task<List<Eskaera_Xehetasuna>> GetEskaeraByEskaeraKodAsync(int eskaeraKod)
        {
            return _database.Table<Eskaera_Xehetasuna>().Where(e => e.Eskaera_kod == eskaeraKod).ToListAsync();
        }

        /// <summary>
        /// Kodea lortuz, katalogoaren informazioa lortzeko metodoa
        /// </summary>
        /// <param name="produktuaKod"></param>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<Katalogoa> GetProduktuaByKodAsync(int produktuaKod)
        {
            return await _database.Table<Katalogoa>().Where(p => p.Produktu_kod == produktuaKod).FirstOrDefaultAsync();
        }




        // Informeak eta estadiskica

        /// <summary>
        /// Hilabeteko eskaerak lortzeko metodoa
        /// </summary>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<List<(Eskaera_Goiburua, List<Eskaera_Xehetasuna>)>> LortuHilabetekoEskaerak()
        {
            var hilabetekoLehenEguna = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var hilabetekoAzkenEguna = hilabetekoLehenEguna.AddMonths(1).AddDays(-1);

            var eskaerak = await _database.Table<Eskaera_Goiburua>().ToListAsync();
            var eskaeraGuztiak = eskaerak
                         .Where(o => DateTime.TryParse(o.Data, out var date) && date >= hilabetekoLehenEguna && date <= hilabetekoAzkenEguna)
                         .ToList();

            var eskaereaXehetasunak = await _database.Table<Eskaera_Xehetasuna>().ToListAsync();
            var eskaeraGuztienXehetasuank = eskaereaXehetasunak.Where(d => eskaeraGuztiak.Any(o => o.Eskaera_kod == d.Eskaera_kod)).ToList();

            return eskaeraGuztiak.Select(o => (o, eskaeraGuztienXehetasuank.Where(d => d.Eskaera_kod == o.Eskaera_kod).ToList())).ToList();
        }

        //GetAllEgoitzaNagusiaAsync-rekin lortua


        /// <summary>
        /// Gehien eskatutako produktua lortzeko metodoa
        /// </summary>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<List<(string ProduktuKod, string Izena, decimal Prezioa, int TotalKantitatea)>> LortuGehienEskatutakoProduktua()
        {
            var eskaeraXehetasunak = await _database.Table<Eskaera_Xehetasuna>().ToListAsync();
            var katalogoa = await _database.Table<Katalogoa>().ToListAsync();

            var gehienSaldutakoProduktuak = eskaeraXehetasunak
                .GroupBy(e => e.Produktu_kod)
                .Select(g => new { ProduktuKod = g.Key, TotalKantitatea = g.Sum(e => e.Kantitatea) })
                .OrderByDescending(p => p.TotalKantitatea)
                .ToList();

            return gehienSaldutakoProduktuak
                .Select(p => {
                    var produktua = katalogoa.FirstOrDefault(k => k.Produktu_kod.ToString() == p.ProduktuKod);
                    return (p.ProduktuKod, produktua?.Izena ?? "Desconocido", produktua?.Prezioa ?? 0, p.TotalKantitatea);
                })
                .ToList();
        }

        /// <summary>
        /// Kantitate handien saltzen duen partnera lortzeko metodoa
        /// </summary>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<List<EskaeraObjetua>> LortuKantitateHandienSaltzenDuenPartnera()
        {
            var eskaeraGoiburuak = await _database.Table<Eskaera_Goiburua>().ToListAsync();
            var eskaeraXehetasuak = await _database.Table<Eskaera_Xehetasuna>().ToListAsync();
            var partners = await _database.Table<Partner>().ToListAsync();

            var informazioGuztia = eskaeraXehetasuak
                .Join(eskaeraGoiburuak, d => d.Eskaera_kod, o => o.Eskaera_kod, (d, o) => new { d, o })
                .Join(partners, combined => combined.o.Partner_ID, p => p.Partner_ID, (combined, p) => new
                {
                    p.Partner_ID,
                    p.Izena,
                    combined.d.Kantitatea 
                })
                .GroupBy(x => new { x.Partner_ID, x.Izena })
                .Select(g => new EskaeraObjetua
                {
                    Partner_ID = g.Key.Partner_ID,
                    Socio = g.Key.Izena,
                    Unidades_Vendidas = g.Sum(x => x.Kantitatea),  
                    Total_Vendido = 0 
                })
                .OrderByDescending(x => x.Unidades_Vendidas) 
                .ToList();

            return informazioGuztia;
        }


        /// <summary>
        /// Irabazi handien duen partnera lortzeko metodoa
        /// </summary>
        /// <returns>Datu baseko informazioa</returns>
        public async Task<List<EskaeraObjetua>> LortuIrabaziHandienDuenPartnera()
        {
            var eskaeraGoiburuak = await _database.Table<Eskaera_Goiburua>().ToListAsync();
            var eskaeraXehetasuak = await _database.Table<Eskaera_Xehetasuna>().ToListAsync();
            var partners = await _database.Table<Partner>().ToListAsync();

            var informazioGuztia = eskaeraXehetasuak
                .Join(eskaeraGoiburuak, d => d.Eskaera_kod, o => o.Eskaera_kod, (d, o) => new { d, o })
                .Join(partners, combined => combined.o.Partner_ID, p => p.Partner_ID, (combined, p) => new
                {
                    p.Partner_ID,
                    p.Izena,
                    combined.d.Kantitatea,
                    combined.d.Guztira
                })
                .GroupBy(x => new { x.Partner_ID, x.Izena }) 
                .Select(g => new EskaeraObjetua
                {
                    Partner_ID = g.Key.Partner_ID,
                    Socio = g.Key.Izena,
                    Unidades_Vendidas = g.Sum(x => x.Kantitatea),
                    Total_Vendido = g.Sum(x => x.Guztira)
                })
                .OrderByDescending(x => x.Total_Vendido)
                .ToList();

            return informazioGuztia;
        }


        /// <summary>
        /// Eskaera objetua, eskaera xehetasunak eta partneren informazioa gordetzeko klasea
        /// </summary>
        public class EskaeraObjetua
        {
            public int Partner_ID { get; set; }
            public string Socio { get; set; }
            public string Producto { get; set; }
            public int Unidades_Vendidas { get; set; }
            public decimal Total_Vendido { get; set; }
        }


        /// <summary>
        /// Egoitza nagusian hurrengo eskaera kodea lortzeko metodoa
        /// </summary>
        /// <returns>Datu baseko informazioa</returns>
        public int LortuHurregoEskaeraKod()
        {
            var lastEskaera = _database.Table<EgoitzaNagusia>().OrderByDescending(e => e.Eskaera_kod).FirstOrDefaultAsync().Result;
            int nextEskaeraKod = lastEskaera != null ? lastEskaera.Eskaera_kod + 1 : 1;

            return nextEskaeraKod;
        }
    }
}


