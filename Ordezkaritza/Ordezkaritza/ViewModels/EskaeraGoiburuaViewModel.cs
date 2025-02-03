using System.Collections.ObjectModel;
using Ordezkaritza.Data;

namespace Ordezkaritza.ViewModels
{
    public class EskaeraGoiburuaViewModel
    {
        private readonly Database _database;

        public ObservableCollection<Eskaera_Goiburua> EskaeraGoiburuaList { get; set; }

        public EskaeraGoiburuaViewModel()
        {
            _database = new Database("Komertzialak.db");
            EskaeraGoiburuaList = new ObservableCollection<Eskaera_Goiburua>();
            LoadEskaeraGoiburuaAsync();
        }

        // Método asíncrono para cargar todos los encabezados de pedidos
        public async Task LoadEskaeraGoiburuaAsync()
        {
            var eskaeraGoiburua = await _database.GetAllEskaeraGoiburuaAsync();
            EskaeraGoiburuaList.Clear();

            foreach (var item in eskaeraGoiburua)
            {
                EskaeraGoiburuaList.Add(item);
            }
        }

        // Método asíncrono para agregar un nuevo encabezado de pedido
        public async Task AddEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua)
        {
            await _database.InsertEskaeraGoiburuaAsync(eskaeraGoiburua);
            await LoadEskaeraGoiburuaAsync(); // Recarga la lista después de la inserción
        }

        // Método asíncrono para actualizar un encabezado de pedido
        public async Task UpdateEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua)
        {
            await _database.UpdateEskaeraGoiburuaAsync(eskaeraGoiburua);
            await LoadEskaeraGoiburuaAsync(); // Recarga la lista después de la actualización
        }

        // Método asíncrono para eliminar un encabezado de pedido
        public async Task DeleteEskaeraGoiburuaAsync(Eskaera_Goiburua eskaeraGoiburua)
        {
            await _database.DeleteEskaeraGoiburuaAsync(eskaeraGoiburua);
            await LoadEskaeraGoiburuaAsync(); // Recarga la lista después de la eliminación
        }
    }
}
