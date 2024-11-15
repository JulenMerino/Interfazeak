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

        // Cargar la descripci�n y el importe en los Entry
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
                await DisplayAlert("�xito", "El importe ha sido modificado.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo modificar el importe.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Ingrese un importe v�lido.", "OK");
        }
    }

    // Eliminar el registro de la base de datos
    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        bool confirmacion = await DisplayAlert("Confirmaci�n", "�Est�s seguro de que deseas eliminar este dep�sito?", "S�", "No");
        if (confirmacion)
        {
            bool exito = await datuBasea.EliminarDepositoPorDescripcionAsync(descripcion);
            if (exito)
            {
                await DisplayAlert("�xito", "El dep�sito ha sido eliminado.", "OK");
                await Navigation.PopAsync(); // Cerrar la ventana despu�s de eliminar
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar el dep�sito.", "OK");
            }
        }
    }

    // Cerrar la ventana
    private async void Cerrar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
