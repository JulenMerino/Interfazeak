using static BankuBatenKudeaketa.DatuBaseaMetodoak;

namespace BankuBatenKudeaketa;

public partial class Inprimatu : ContentPage
{
    public Inprimatu(string descripcionPrestamo, decimal importePrestamo, int plazoPrestamo, DateTime fechaPrestamo, decimal saldo, string descripcionDeposito, string nan, string izenaAbizena)
    {
        InitializeComponent();

        lblNAN.Text = nan;
        lblIzena.Text = izenaAbizena;

        // Asignar los valores del préstamo a las etiquetas del préstamo
        lblMailegua.Text = "Mailegua"; // Título de la sección
        lblMaileguDeskribapena.Text = descripcionPrestamo; // Descripción del préstamo
        lblMaileguKantitatea.Text = $"Importe: {importePrestamo:C}"; // Importe del préstamo
        lblMaileguEpea.Text = $"Plazo: {plazoPrestamo} meses"; // Plazo del préstamo
        lblMaileguHasiera.Text = $"Fecha: {fechaPrestamo.ToString("dd/MM/yyyy")}"; // Fecha del préstamo

        // Asignar los valores del depósito a las etiquetas del depósito
        lblGordailua.Text = "Gordailua"; // Título de la sección
        lblGordailuDeskribapena.Text = descripcionDeposito; // Descripción del depósito
        lblGordailuSaldo.Text = $"Saldo: {saldo:C}"; // Saldo del depósito
    }
}






