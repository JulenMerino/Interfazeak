namespace BankuBatenKudeaketa;

public partial class Gordailua : ContentPage
{
    private string deskripzioa;
    private DatuBaseaMetodoak datuBasea;

    public Gordailua(string deskripzioa, decimal zenbatekoa)
    {
        InitializeComponent();
        datuBasea = new DatuBaseaMetodoak();
        this.deskripzioa = deskripzioa;

        // Deskripzioa eta zenbatekoa Entry-etan kargatu
        etyDeskribapena.Text = deskripzioa;
        etySaldo.Text = zenbatekoa.ToString("F2"); // Bi hamartarretan formatatuta
    }


    /// <summary>
    /// Metodo honek saldoaren aldaketa kudeatzen du botoian klik egitean. Sartutako zenbatekoa baliozko zenbaki bat den egiaztatzen du,
    /// eta deskripzioaren arabera datu-basean eguneratu egiten du. Operazioaren emaitza kontuan hartuta, arrakasta edo errore mezu bat erakusten du.
    /// </summary>
    private async void Aldatu_Clicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(etySaldo.Text, out decimal berriZenbatekoa))
        {
            bool arrakasta = await datuBasea.AldatuZenbatekoaDeskribapenarenAraberaAsync(deskripzioa, berriZenbatekoa);
            if (arrakasta)
            {
                await DisplayAlert("Arrakasta", "Zenbatekoa aldatu da.", "OK");
            }
            else
            {
                await DisplayAlert("Errore", "Ez da posible zenbatekoa aldatzea.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Errore", "Mesedez, sartu zenbateko baliodun bat.", "OK");
        }
    }

    /// <summary>
    /// Metodo honek gordailu bat ezabatzeko prozesua kudeatzen du. Erabiltzaileari baieztapena eskatzen dio lehenik,
    /// eta baietz esaten badu, datu-basean gordailua ezabatzen du. Prozesuaren emaitza kontuan hartuta, arrakasta edo errore mezu bat erakusten da.
    /// </summary>
    private async void Ezabatu_Clicked(object sender, EventArgs e)
    {
        bool baieztapena = await DisplayAlert("Baieztapena", "Ziur zaude gordailu hau ezabatu nahi duzula?", "Bai", "Ez");
        if (baieztapena)
        {
            bool arrakasta = await datuBasea.EzabatuGordailuaDeskribapenarenAraberaAsync(deskripzioa);
            if (arrakasta)
            {
                await DisplayAlert("Arrakasta", "Gordailua ondo ezabatu da.", "OK");
                await Navigation.PopAsync(); // Leihoa itxi ezabatzean
            }
            else
            {
                await DisplayAlert("Errore", "Ez da posible gordailua ezabatzea.", "OK");
            }
        }
    }

    /// <summary>
    /// Metodo honek orain dagoen leihoa itxiko du, erabiltzaileri aurreko pantailara bueltatzeko aukera emanez.
    /// </summary>
    private async void Itxi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
