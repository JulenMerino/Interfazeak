﻿
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

            _database.CreateTableAsync<Katalogoa>().Wait();
            _database.CreateTableAsync<Komertziala>().Wait();
            _database.CreateTableAsync<Partner>().Wait();
            _database.CreateTableAsync<Eskaera_Goiburua>().Wait();
            _database.CreateTableAsync<Eskaera_Xehetasuna>().Wait();
            _database.CreateTableAsync<Bidalketa>().Wait();
            _database.CreateTableAsync<EgoitzaNagusia>().Wait();


        }

        // Métodos para obtener todos los registros
        public Task<List<Katalogoa>> GetAllKatalogoasAsync() => _database.Table<Katalogoa>().ToListAsync();
        public Task<List<Komertziala>> GetAllKomertzialasAsync() => _database.Table<Komertziala>().ToListAsync();
        public Task<List<Partner>> GetAllPartnersAsync() => _database.Table<Partner>().ToListAsync();
        public Task<List<Eskaera_Goiburua>> GetAllEskaeraGoiburuaAsync() => _database.Table<Eskaera_Goiburua>().ToListAsync();
        public Task<List<Eskaera_Xehetasuna>> GetAllEskaeraXehetasunaAsync() => _database.Table<Eskaera_Xehetasuna>().ToListAsync();
        public Task<List<Bidalketa>> GetAllBidalketaAsync() => _database.Table<Bidalketa>().ToListAsync();
        public Task<List<EgoitzaNagusia>> GetAllEgoitzaNagusiaAsync() => _database.Table<EgoitzaNagusia>().ToListAsync();

        // Métodos para insertar nuevos registros
        public Task<int> InsertKatalogoaAsync(Katalogoa katalogoa) => _database.InsertAsync(katalogoa);
        public Task<int> InsertKomertzialaAsync(Komertziala komertziala) => _database.InsertAsync(komertziala);
        public Task<int> InsertPartnerAsync(Partner partner) => _database.InsertAsync(partner);
        public Task<int> InsertEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.InsertAsync(eskaeraGoiburua);
        public Task<int> InsertEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.InsertAsync(eskaeraXehetasuna);
        public Task<int> InsertBidalketaAsync(Bidalketa bidalketa) => _database.InsertAsync(bidalketa);
        public Task<int> InsertEgoitzaNagusiaAsync(EgoitzaNagusia egoitzaNagusia) => _database.InsertAsync(egoitzaNagusia);

        // Métodos para actualizar registros
        public Task<int> UpdateKatalogoaAsync(Katalogoa katalogoa) => _database.UpdateAsync(katalogoa);
        public Task<int> UpdateKomertzialaAsync(Komertziala komertziala) => _database.UpdateAsync(komertziala);
        public Task<int> UpdatePartnerAsync(Partner partner) => _database.UpdateAsync(partner);
        public Task<int> UpdateEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.UpdateAsync(eskaeraGoiburua);
        public Task<int> UpdateEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.UpdateAsync(eskaeraXehetasuna);
        public Task<int> UpdateBidalketaAsync(Bidalketa bidalketa) => _database.UpdateAsync(bidalketa);
        public Task<int> UpdateEgoitzaNagusiaAsync(EgoitzaNagusia egoitzaNagusia) => _database.UpdateAsync(egoitzaNagusia);

        // Métodos para eliminar registros
        public Task<int> DeleteKatalogoaAsync(Katalogoa katalogoa) => _database.DeleteAsync(katalogoa);
        public Task<int> DeleteKomertzialaAsync(Komertziala komertziala) => _database.DeleteAsync(komertziala);
        public Task<int> DeletePartnerAsync(Partner partner) => _database.DeleteAsync(partner);
        public Task<int> DeleteEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.DeleteAsync(eskaeraGoiburua);
        public Task<int> DeleteEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.DeleteAsync(eskaeraXehetasuna);
        public Task<int> DeleteBidalketaAsync(Bidalketa bidalketa) => _database.DeleteAsync(bidalketa);
        public Task<int> DeleteEgoitzaNagusiaAsync(EgoitzaNagusia egoitzaNagusia) => _database.DeleteAsync(egoitzaNagusia);






        //XML zatia
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

        

        public async Task SaveDataFromXmlAsync(string fitxategiHelbidea)
        {
            var fitxategiIzena = Path.GetFileName(fitxategiHelbidea);

            //Debug.WriteLine($"XML fitxategia: {Path.GetFileName(filePath)}");

            if (fitxategiIzena == "Froga.xml")
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

        public async Task<Bidalketa> GetBidalketaByEskaeraKodAsync(int eskaeraKod)
        {
            return await _database.Table<Bidalketa>()
                .Where(b => b.Eskaera_kod == eskaeraKod)
                .FirstOrDefaultAsync();
        }

        public Task<List<Eskaera_Xehetasuna>> GetEskaeraByEskaeraKodAsync(int eskaeraKod)
        {
            return _database.Table<Eskaera_Xehetasuna>().Where(e => e.Eskaera_kod == eskaeraKod).ToListAsync();
        }

        public async Task<Katalogoa> GetProduktuaByKodAsync(int produktuaKod)
        {
            return await _database.Table<Katalogoa>().Where(p => p.Produktu_kod == produktuaKod).FirstOrDefaultAsync();
        }




        // Informeak eta estadiskica

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

        

        public class EskaeraObjetua
        {
            public int Partner_ID { get; set; }
            public string Socio { get; set; }
            public string Producto { get; set; }
            public int Unidades_Vendidas { get; set; }
            public decimal Total_Vendido { get; set; }
        }

        public int LortuHurregoEskaeraKod()
        {
            // Obtener el último Eskaera_kod
            var lastEskaera = _database.Table<EgoitzaNagusia>().OrderByDescending(e => e.Eskaera_kod).FirstOrDefaultAsync().Result;

            // Si no hay registros, se comienza desde 1
            int nextEskaeraKod = lastEskaera != null ? lastEskaera.Eskaera_kod + 1 : 1;

            return nextEskaeraKod;
        }
    }
}


