using static BankuBatenKudeaketa.DatuBaseaMetodoak;

namespace BankuBatenKudeaketa;

public partial class Inprimatu : ContentPage
{
    public Inprimatu(string descripcionPrestamo, decimal importePrestamo, int plazoPrestamo, DateTime fechaPrestamo, decimal saldo, string descripcionDeposito, string nan, string izenaAbizena)
    {
        InitializeComponent();

        lblNAN.Text = nan;
        lblIzena.Text = izenaAbizena;

        // Asignar los valores del pr�stamo a las etiquetas del pr�stamo
        lblMailegua.Text = "Mailegua"; // T�tulo de la secci�n
        lblMaileguDeskribapena.Text = descripcionPrestamo; // Descripci�n del pr�stamo
        lblMaileguKantitatea.Text = $"Importe: {importePrestamo:C}"; // Importe del pr�stamo
        lblMaileguEpea.Text = $"Plazo: {plazoPrestamo} meses"; // Plazo del pr�stamo
        lblMaileguHasiera.Text = $"Fecha: {fechaPrestamo.ToString("dd/MM/yyyy")}"; // Fecha del pr�stamo

        // Asignar los valores del dep�sito a las etiquetas del dep�sito
        lblGordailua.Text = "Gordailua"; // T�tulo de la secci�n
        lblGordailuDeskribapena.Text = descripcionDeposito; // Descripci�n del dep�sito
        lblGordailuSaldo.Text = $"Saldo: {saldo:C}"; // Saldo del dep�sito
    }
}






