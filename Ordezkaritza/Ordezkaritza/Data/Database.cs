﻿using SQLite;


namespace Ordezkaritza.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;
        private static string _dbPath;

        public Database(string dbPath)
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "Komertzialak.db");

            // Si la base de datos no existe, copiarla desde el archivo de recursos
            if (!File.Exists(_dbPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("Komertzialak.db").Result;
                using var fileStream = File.Create(_dbPath);
                stream.CopyTo(fileStream);
            }

            // Crear la conexión asíncrona
            _database = new SQLiteAsyncConnection(_dbPath);

            // Crear las tablas si no existen
            _database.CreateTableAsync<Katalogoa>().Wait();
            _database.CreateTableAsync<Komertziala>().Wait();
            _database.CreateTableAsync<Partner>().Wait();
            _database.CreateTableAsync<Eskaera_Goiburua>().Wait();
            _database.CreateTableAsync<Eskaera_Xehetasuna>().Wait();
        }

        // Métodos para obtener todos los registros
        public Task<List<Katalogoa>> GetAllKatalogoasAsync() => _database.Table<Katalogoa>().ToListAsync();
        public Task<List<Komertziala>> GetAllKomertzialasAsync() => _database.Table<Komertziala>().ToListAsync();
        public Task<List<Partner>> GetAllPartnersAsync() => _database.Table<Partner>().ToListAsync();
        public Task<List<Eskaera_Goiburua>> GetAllEskaeraGoiburuaAsync() => _database.Table<Eskaera_Goiburua>().ToListAsync();
        public Task<List<Eskaera_Xehetasuna>> GetAllEskaeraXehetasunaAsync() => _database.Table<Eskaera_Xehetasuna>().ToListAsync();

        // Métodos para insertar nuevos registros
        public Task<int> InsertKatalogoaAsync(Katalogoa katalogoa) => _database.InsertAsync(katalogoa);
        public Task<int> InsertKomertzialaAsync(Komertziala komertziala) => _database.InsertAsync(komertziala);
        public Task<int> InsertPartnerAsync(Partner partner) => _database.InsertAsync(partner);
        public Task<int> InsertEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.InsertAsync(eskaeraGoiburua);
        public Task<int> InsertEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.InsertAsync(eskaeraXehetasuna);

        // Métodos para actualizar registros
        public Task<int> UpdateKatalogoaAsync(Katalogoa katalogoa) => _database.UpdateAsync(katalogoa);
        public Task<int> UpdateKomertzialaAsync(Komertziala komertziala) => _database.UpdateAsync(komertziala);
        public Task<int> UpdatePartnerAsync(Partner partner) => _database.UpdateAsync(partner);
        public Task<int> UpdateEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.UpdateAsync(eskaeraGoiburua);
        public Task<int> UpdateEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.UpdateAsync(eskaeraXehetasuna);

        // Métodos para eliminar registros
        public Task<int> DeleteKatalogoaAsync(Katalogoa katalogoa) => _database.DeleteAsync(katalogoa);
        public Task<int> DeleteKomertzialaAsync(Komertziala komertziala) => _database.DeleteAsync(komertziala);
        public Task<int> DeletePartnerAsync(Partner partner) => _database.DeleteAsync(partner);
        public Task<int> DeleteEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua) => _database.DeleteAsync(eskaeraGoiburua);
        public Task<int> DeleteEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna) => _database.DeleteAsync(eskaeraXehetasuna);
    }
}
