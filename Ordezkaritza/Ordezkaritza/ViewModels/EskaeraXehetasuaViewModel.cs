using System.Collections.ObjectModel;
using Ordezkaritza.Data;

namespace Ordezkaritza.ViewModels
{
    public class EskaeraXehetasunaViewModel
    {
        private readonly Database _database;

        public ObservableCollection<Eskaera_Xehetasuna> EskaeraXehetasunaList { get; set; }

        public EskaeraXehetasunaViewModel()
        {
            _database = new Database("Komertzialak.db");
            EskaeraXehetasunaList = new ObservableCollection<Eskaera_Xehetasuna>();
            LoadEskaeraXehetasunaAsync();
        }

        // Método asíncrono para cargar todos los detalles de pedidos
        public async Task LoadEskaeraXehetasunaAsync()
        {
            var eskaeraXehetasuna = await _database.GetAllEskaeraXehetasunaAsync();
            EskaeraXehetasunaList.Clear();

            foreach (var item in eskaeraXehetasuna)
            {
                EskaeraXehetasunaList.Add(item);
            }
        }

        // Método asíncrono para agregar un nuevo detalle de pedido
        public async Task AddEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna)
        {
            await _database.InsertEskaeraXehetasunaAsync(eskaeraXehetasuna);
            await LoadEskaeraXehetasunaAsync(); // Recarga la lista después de la inserción
        }

        // Método asíncrono para actualizar un detalle de pedido
        public async Task UpdateEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna)
        {
            await _database.UpdateEskaeraXehetasunaAsync(eskaeraXehetasuna);
            await LoadEskaeraXehetasunaAsync(); // Recarga la lista después de la actualización
        }

        // Método asíncrono para eliminar un detalle de pedido
        public async Task DeleteEskaeraXehetasunaAsync(Eskaera_Xehetasuna eskaeraXehetasuna)
        {
            await _database.DeleteEskaeraXehetasunaAsync(eskaeraXehetasuna);
            await LoadEskaeraXehetasunaAsync(); // Recarga la lista después de la eliminación
        }
    }
}
