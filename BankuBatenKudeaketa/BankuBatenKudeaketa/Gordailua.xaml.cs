namespace BankuBatenKudeaketa;

public partial class Gordailua : ContentPage
{
    private string descripcion;
    private DatuBaseaMetodoak datuBasea;

    public Gordailua( string descripcion, decimal importe)
    {
        InitializeComponent();
        datuBasea = new DatuBaseaMetodoak();
        this.descripcion = descripcion;

        // Cargar la descripción y el importe en los Entry
        etyDeskribapena.Text = descripcion;
        etySaldo.Text = importe.ToString("F2"); // Formateado a dos decimales
    }

    // Modificar el importe en la base de datos
    private async void Modificar_Clicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(etySaldo.Text, out decimal nuevoImporte))
        {
            bool exito = await datuBasea.ModificarImportePorDescripcionAsync(descripcion, nuevoImporte);
            if (exito)
            {
                await DisplayAlert("Éxito", "El importe ha sido modificado.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo modificar el importe.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Ingrese un importe válido.", "OK");
        }
    }

    // Eliminar el registro de la base de datos
    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        bool confirmacion = await DisplayAlert("Confirmación", "¿Estás seguro de que deseas eliminar este depósito?", "Sí", "No");
        if (confirmacion)
        {
            bool exito = await datuBasea.EliminarDepositoPorDescripcionAsync(descripcion);
            if (exito)
            {
                await DisplayAlert("Éxito", "El depósito ha sido eliminado.", "OK");
                await Navigation.PopAsync(); // Cerrar la ventana después de eliminar
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar el depósito.", "OK");
            }
        }
    }

    // Cerrar la ventana
    private async void Cerrar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
