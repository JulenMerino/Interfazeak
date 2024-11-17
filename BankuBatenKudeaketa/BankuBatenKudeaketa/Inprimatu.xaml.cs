using static BankuBatenKudeaketa.DatuBaseaMetodoak;

namespace BankuBatenKudeaketa;

public partial class Inprimatu : ContentPage
{
    public Inprimatu(string maileguDeskribapena, decimal maileguZenbatekoa, int maileguEpea, DateTime maileguData, decimal saldoa, string gordailuDeskribapena, string nan, string izenaAbizena)
    {
        InitializeComponent();

        lblNAN.Text = nan;
        lblIzena.Text = izenaAbizena;

        // Gordailuaren balioak gordailuaren etiketetara esleitu
        lblGordailua.Text = "Gordailua";
        lblGordailuDeskribapena.Text = $"Deskribapena: {gordailuDeskribapena}";
        lblGordailuSaldo.Text = $"Saldoa: {saldoa:C}";

        // Maileguaren balioak maileguaren etiketetara esleitu
        lblMailegua.Text = "Mailegua"; 
        lblMaileguDeskribapena.Text = $"Deskribapena: {maileguDeskribapena}"; 
        lblMaileguKantitatea.Text = $"Zenbatekoa: {maileguZenbatekoa:C}"; 
        lblMaileguEpea.Text = $"Epea: {maileguEpea} hilabete"; 
        lblMaileguHasiera.Text = $"Data: {maileguData.ToString("dd/MM/yyyy")}"; 

        
    }
}







