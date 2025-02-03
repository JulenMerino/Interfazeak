using System.Collections.ObjectModel;
using Ordezkaritza.Data;

namespace Ordezkaritza.ViewModels
{
    public class KomertzialaViewModel
    {
        private readonly Database _database;

        public ObservableCollection<Komertziala> Komertzialas { get; set; }

        public KomertzialaViewModel()
        {
            _database = new Database("Komertzialak.db");
            Komertzialas = new ObservableCollection<Komertziala>();
            LoadKomertzialasAsync();
        }

        // Método asíncrono para cargar todos los comerciales
        public async Task LoadKomertzialasAsync()
        {
            var komertzialas = await _database.GetAllKomertzialasAsync();
            Komertzialas.Clear();

            foreach (var komertziala in komertzialas)
            {
                Komertzialas.Add(komertziala);
            }
        }

        // Método asíncrono para agregar un nuevo comercial
        public async Task AddKomertzialaAsync(Komertziala komertziala)
        {
            await _database.InsertKomertzialaAsync(komertziala);
            await LoadKomertzialasAsync(); // Recarga la lista después de la inserción
        }

        // Método asíncrono para actualizar un comercial
        public async Task UpdateKomertzialaAsync(Komertziala komertziala)
        {
            await _database.UpdateKomertzialaAsync(komertziala);
            await LoadKomertzialasAsync(); // Recarga la lista después de la actualización
        }

        // Método asíncrono para eliminar un comercial
        public async Task DeleteKomertzialaAsync(Komertziala komertziala)
        {
            await _database.DeleteKomertzialaAsync(komertziala);
            await LoadKomertzialasAsync(); // Recarga la lista después de la eliminación
        }
    }
}
