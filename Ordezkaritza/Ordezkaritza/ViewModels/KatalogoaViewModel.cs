using System.Collections.ObjectModel;
using Ordezkaritza.Data;

namespace Ordezkaritza.ViewModels
{
    public class KatalogoaViewModel
    {
        private readonly Database _database;

        public ObservableCollection<Katalogoa> Katalogoas { get; set; }

        public KatalogoaViewModel()
        {
            _database = new Database("Komertzialak.db");
            Katalogoas = new ObservableCollection<Katalogoa>();
            LoadKatalogoasAsync();
        }

        // Método asíncrono para cargar todos los productos
        public async Task LoadKatalogoasAsync()
        {
            var katalogoas = await _database.GetAllKatalogoasAsync();
            Katalogoas.Clear();

            foreach (var katalogoa in katalogoas)
            {
                Katalogoas.Add(katalogoa);
            }
        }

        // Método asíncrono para agregar un nuevo producto
        public async Task AddKatalogoaAsync(Katalogoa katalogoa)
        {
            await _database.InsertKatalogoaAsync(katalogoa);
            await LoadKatalogoasAsync(); // Recarga la lista después de la inserción
        }

        // Método asíncrono para actualizar un producto
        public async Task UpdateKatalogoaAsync(Katalogoa katalogoa)
        {
            await _database.UpdateKatalogoaAsync(katalogoa);
            await LoadKatalogoasAsync(); // Recarga la lista después de la actualización
        }

        // Método asíncrono para eliminar un producto
        public async Task DeleteKatalogoaAsync(Katalogoa katalogoa)
        {
            await _database.DeleteKatalogoaAsync(katalogoa);
            await LoadKatalogoasAsync(); // Recarga la lista después de la eliminación
        }
    }
}
