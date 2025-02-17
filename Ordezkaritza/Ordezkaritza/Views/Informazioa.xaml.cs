using Ordezkaritza.Data;
using Ordezkaritza.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Ordezkaritza.Views;

public partial class Informazioa : ContentPage
{
    private readonly Database _database;
    private string fitxategiHelbidea;
    public ObservableCollection<Katalogoa> Katalogoa { get; set; }
    private ObservableCollection<Partner> Partners;

    public Informazioa()
    {
        InitializeComponent();

        _database = new Database("Komertzialak.db");
        Katalogoa = new ObservableCollection<Katalogoa>();
        Partners = new ObservableCollection<Partner>();


        ProduktuaKargatu();
        KargatuPartnerrak();
    }


    // XML fitxategiak Irakurri

    private async void btnAukeratuXMLa_Clicked(object sender, EventArgs e)
    {
        fitxategiHelbidea = await _database.PickXmlFileAsync(); 

        if (!string.IsNullOrEmpty(fitxategiHelbidea))
        {
            lblEmaitzaAukeratu.Text = Path.GetFileName(fitxategiHelbidea) + " aukeratu duzu";
        }
    }


    private async void btnKargatuDatuak_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(fitxategiHelbidea))
        {
            await DisplayAlert("Errorea", "Mesedez, hautatu XML fitxategia lehenik.", "OK");
            return;
        }

        await _database.SaveDataFromXmlAsync(fitxategiHelbidea); 


        var katalogoLista = await _database.GetAllKatalogoasAsync();
        Katalogoa.Clear();
        foreach (var produktua in katalogoLista)
        {
            Katalogoa.Add(produktua);
        }

        await DisplayAlert("Arrakasta", "Datuak ongi kargatu dira!", "OK");
    }











    //Eskaera zatia

    private void pkPartner_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pkPartner.SelectedIndex != -1)
        {
            var selectedPartner = (Partner)pkPartner.SelectedItem;
            etyHelbidea.Text = selectedPartner?.Helbidea ?? "";
        }
    }

    private async void KargatuPartnerrak()
    {
        var partners = await _database.GetAllPartnersAsync();
        Partners.Clear();
        foreach (var partner in partners)
        {
            Partners.Add(partner);
        }
        pkPartner.ItemsSource = Partners;
    }



    private decimal TotalaKalkulatu()
    {
        return Katalogoa.Sum(p => p.PrezioTotala); 
    }

    private void btnGehituKantitatea_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Katalogoa produktuKantitatea)
        {
            if (produktuKantitatea.Kantitatea < produktuKantitatea.Stock) 
            {
                produktuKantitatea.Kantitatea++;
                TotalaEguneratu(); 
            }
        }
    }


    private void btnKenduaKantitatea_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Katalogoa produktuKantitatea)
        {
            if (produktuKantitatea.Kantitatea > 0) 
            {
                produktuKantitatea.Kantitatea--;
                TotalaEguneratu(); 
            }
        }
    }

    private async void TotalaEguneratu()
    {
        etyPrezioTotala.Text = TotalaKalkulatu().ToString("F2");
    }

    private async void btnFaktura_Clicked(object sender, EventArgs e)
    {
        var AukeratutatkoPartnerra = pkPartner.SelectedItem as Partner;
        if (AukeratutatkoPartnerra == null)
        {
            await DisplayAlert("Error", "Selecciona un partner antes de continuar.", "OK");
            return;
        }

        var eskaeraGoiburua = new Eskaera_Goiburua
        {
            Partner_ID = AukeratutatkoPartnerra.Partner_ID,      
            Komertzial_ID = AukeratutatkoPartnerra.ID_komertzial, 
            Egoera = "Pendiente",
            Data = dpData.Date.ToString("yyyy/MM/dd")
        };

        await _database.InsertEskaeraGoiburuaAsync(eskaeraGoiburua);


        foreach (var produktuInformazioa in Katalogoa)
        {
            if (produktuInformazioa.Kantitatea > 0)
            {
                var eskaeraXehetasunak = new Eskaera_Xehetasuna
                {
                    Eskaera_kod = eskaeraGoiburua.Eskaera_kod,
                    Produktu_kod = produktuInformazioa.Produktu_kod.ToString(),
                    Deskribapena = produktuInformazioa.Izena,
                    Guztira = produktuInformazioa.Prezioa * produktuInformazioa.Kantitatea,
                    Prezioa = produktuInformazioa.Prezioa,
                    Kantitatea = produktuInformazioa.Kantitatea
                };

                Debug.WriteLine($"Eskaera: {eskaeraXehetasunak.Produktu_kod} - {eskaeraXehetasunak.Deskribapena} - {eskaeraXehetasunak.Kantitatea} - {eskaeraXehetasunak.Guztira} - {eskaeraXehetasunak.Eskaera_kod}");

                await _database.InsertEskaeraXehetasunaAsync(eskaeraXehetasunak);
            }
        }

        var bidalketa = new Bidalketa
        {
            Enpresa_izena = "Correos",
            Data = dpData.Date.ToString("yyyy/MM/dd"),
            Eskaera_kod = eskaeraGoiburua.Eskaera_kod
        };

        await _database.InsertBidalketaAsync(bidalketa);


        TotalaEguneratu();

        await Navigation.PushAsync(new Views.FakturaIkusi(AukeratutatkoPartnerra, eskaeraGoiburua.Eskaera_kod.ToString()));
    }


    private void etyKantitatea_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Katalogoa produktuKantitatea)
        {
            if (int.TryParse(entry.Text, out int kantitateBerria))
            {
                produktuKantitatea.Kantitatea = kantitateBerria; 
            }
            else
            {
                entry.Text = produktuKantitatea.Kantitatea.ToString(); 
            }
        }
    }















    //Stock zatia


    private async void ProduktuaKargatu()
    {
        var produktuak = await _database.GetAllKatalogoasAsync();

        Katalogoa.Clear();

        foreach (var produktuenInformazioa in produktuak)
        {
            Katalogoa.Add(new Katalogoa
            {
                Produktu_kod = produktuenInformazioa.Produktu_kod,
                Izena = produktuenInformazioa.Izena,
                Prezioa = produktuenInformazioa.Prezioa,
                Stock = produktuenInformazioa.Stock,
                Irudia = LortuIrudia(produktuenInformazioa.Produktu_kod) 
            });
        }

        ProduktuakColection.ItemsSource = Katalogoa;
        cvEskaera.ItemsSource = Katalogoa;
    }

    private string LortuIrudia(int produktuKodea)
    {

        switch (produktuKodea)
        {
            case 1: 
                return "analogiko1.png";
            case 2: 
                return "analogiko2.png";
            case 3: 
                return "analogiko3.png";
            default:
                return "default_image.png";  
        }
    }

    private async void btnStocKBerritu_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new Views.StockBerritu());
    }




    //Informeak/Estadisticas zatia


    private void btnHilabetekoEskaerak_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Eskaera 1", "Eskaera 2", "Eskaera 3" };
        EguneratuTaula(datos);
    }

    private void btnEskaerakEgoitzaNagusira_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Egoitza Nagusira 1", "Egoitza Nagusira 2" };
        EguneratuTaula(datos);
    }

    private void btnGehienEskatutakoProduktua_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Produktua A", "Produktua B", "Produktua C" };
        EguneratuTaula(datos);
    }

    private void btnGehienSaltzenDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Bazkidea 1", "Bazkidea 2" };
        EguneratuTaula(datos);
    }

    private void btnIrabaziHandienaDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Bazkidea X - 1000€", "Bazkidea Y - 900€" };
        EguneratuTaula(datos);
    }

    private void EguneratuTaula(List<string> taulakoDatuak)
    {
        InfoSection.Clear(); 

        foreach (var eguneratuakoDatuak in taulakoDatuak)
        {
            InfoSection.Add(new TextCell { Text = eguneratuakoDatuak });
        }
    }





    

}


