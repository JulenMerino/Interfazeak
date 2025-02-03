using System.Collections.ObjectModel;
using Ordezkaritza.Data;

namespace Ordezkaritza.ViewModels
{
    public class PartnerViewModel
    {
        private readonly Database _database;

        public ObservableCollection<Partner> Partners { get; set; }

        public PartnerViewModel()
        {
            _database = new Database("Komertzialak.db");
            Partners = new ObservableCollection<Partner>();
            LoadPartnersAsync();
        }

        // Método asíncrono para cargar todos los socios
        public async Task LoadPartnersAsync()
        {
            var partners = await _database.GetAllPartnersAsync();
            Partners.Clear();

            foreach (var partner in partners)
            {
                Partners.Add(partner);
            }
        }

        // Método asíncrono para agregar un nuevo socio
        public async Task AddPartnerAsync(Partner partner)
        {
            await _database.InsertPartnerAsync(partner);
            await LoadPartnersAsync(); // Recarga la lista después de la inserción
        }

        // Método asíncrono para actualizar un socio
        public async Task UpdatePartnerAsync(Partner partner)
        {
            await _database.UpdatePartnerAsync(partner);
            await LoadPartnersAsync(); // Recarga la lista después de la actualización
        }

        // Método asíncrono para eliminar un socio
        public async Task DeletePartnerAsync(Partner partner)
        {
            await _database.DeletePartnerAsync(partner);
            await LoadPartnersAsync(); // Recarga la lista después de la eliminación
        }
    }
}
