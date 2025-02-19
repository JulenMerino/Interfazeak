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

        OnAktualizatuProduktua += AktualizatuProduktua;


        ProduktuaKargatu();
        KargatuPartnerrak();
    }


    // XML fitxategiak Irakurri

    /// <summary>
    /// Botoia sakatuenean, XML fitxategia aukeratzeko lehioa irekitzen da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnAukeratuXMLa_Clicked(object sender, EventArgs e)
    {
        fitxategiHelbidea = await _database.AuketatuXmlFitxategia(); 

        if (!string.IsNullOrEmpty(fitxategiHelbidea))
        {
            lblEmaitzaAukeratu.Text = Path.GetFileName(fitxategiHelbidea) + " aukeratu duzu";
        }
    }

    /// <summary>
    /// Botoia sakatuenean, aukeratutako XML fitxategiko datuak kargatzen dira datu basean.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Picker-a partnerrak aukeratzeko eta honen helbidea lortzeko.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pkPartner_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pkPartner.SelectedIndex != -1)
        {
            var selectedPartner = (Partner)pkPartner.SelectedItem;
            etyHelbidea.Text = selectedPartner?.Helbidea ?? "";
        }
    }

    /// <summary>
    /// Datu basean dauden partnerrak picker-ean kargatzen dira.
    /// </summary>
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


    /// <summary>
    /// Katalogoan sartutako produktu kantitatearen prezio totala kalulatzen du.
    /// </summary>
    /// <returns>Produktuaren prezio guztien batuketa</returns>
    private decimal TotalaKalkulatu()
    {
        return Katalogoa.Sum(p => p.PrezioTotala); 
    }

    /// <summary>
    /// Botoia sakatzean, produktuari kantitatea gehitzen du.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Botoia sakatzean, produktuari kantitatea kentzen dio.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Kalkulatutako totala entry batean sartzen du.
    /// </summary>
    private async void TotalaEguneratu()
    {
        etyPrezioTotala.Text = TotalaKalkulatu().ToString("F2");
    }


    /// <summary>
    /// Botoia sakatzean, Informazio guztia hartu eta beste orri batera bidaltzen du faktura antezeko bat sortuz eta orri haun irekitzen du.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Kantitate entry-an sartutako zenbakia produktuaren kantitatea eguneratzeko.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Katalogo tablan dauden produktuak collection view batean kargatzen dira.
    /// </summary>
    public async void ProduktuaKargatu()
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

    /// <summary>
    /// Produktu kodearen arabera irudia itzultzen du.
    /// </summary>
    /// <param name="produktuKodea"></param>
    /// <returns> Irudia </returns>
    public string LortuIrudia(int produktuKodea)
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

    /// <summary>
    /// Stok-a eguneratzeko metodoa.
    /// </summary>
    public async void AktualizatuProduktua()
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


    public static event Action OnAktualizatuProduktua;

    public static void AktualizatuProduktuaGlobal()
    {
        OnAktualizatuProduktua?.Invoke();
    }

    /// <summary>
    /// Botoia sakatzean, produktuaren stock-a gehitzeko lehioa irekitzen da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnStocKBerritu_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new Views.StockBerritu());
    }




    //Informeak/Estadisticas zatia

    /// <summary>
    /// Botoia sakatzean, hilabeteko eskaerak lortzen dira eta taulan erakusten dira.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnHilabetekoEskaerak_Clicked(object sender, EventArgs e)
    {
        var hilabetekoEskaerak = await _database.LortuHilabetekoEskaerak();
        var datuak = hilabetekoEskaerak.Select(o => $"Eskaera {o.Item1.Eskaera_kod} - Data: {o.Item1.Data}, Partner ID: {o.Item1.Partner_ID}, Produktuak: [{string.Join(", ", o.Item2.Select(d => d.Deskribapena + " - Kantitatea: " + d.Kantitatea))}]").ToList();
        EguneratuTaula(datuak);
    }

    /// <summary>
    /// Botoia sakatzean, egoitza nagusira egindako eskaerak lortzen dira eta taulan erakusten dira.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnEskaerakEgoitzaNagusira_Clicked(object sender, EventArgs e)
    {
        var egoitzaNagusia = await _database.GetAllEgoitzaNagusiaAsync();
        var datuak = egoitzaNagusia.GroupBy(e => e.Eskaera_kod)
                                   .Select(g => $"Eskaera kodea: {g.Key} [{string.Join(", ", g.Select(e => e.Izena + " - Kantitatea: " + e.Kantitatea))}]")
                                   .ToList();
        EguneratuTaula(datuak);
    }

    /// <summary>
    /// Botoia sakatzean, gehien eskatutako produktua lortzen da eta taulan erakusten da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnGehienEskatutakoProduktua_Clicked(object sender, EventArgs e)
    {
        var gehienSaldutakoProduktua = await _database.LortuGehienEskatutakoProduktua();

        var datuak = gehienSaldutakoProduktua.Select(p => $"{p.ProduktuKod} - {p.Izena}: {p.TotalKantitatea} unidades, {p.Prezioa}€").ToList();
        EguneratuTaula(datuak);
    }

    /// <summary>
    /// Botoia sakatzean, gehien saltzen duen partnera lortzen da eta taulan erakusten da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnGehienSaltzenDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var kantitateHandienPartner = await _database.LortuKantitateHandienSaltzenDuenPartnera();
        var datuak = kantitateHandienPartner.Select(d => $"{d.Socio} - {d.Unidades_Vendidas} unidades").ToList();

        EguneratuTaula(datuak);
    }

    /// <summary>
    /// Botoia sakatzean, irabazi handiena duen partnera lortzen da eta taulan erakusten da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnIrabaziHandienaDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var irabaziHandienPartner = await _database.LortuIrabaziHandienDuenPartnera();
        var datuak = irabaziHandienPartner.Select(d => $"{d.Socio} - {d.Unidades_Vendidas} unidades - {d.Total_Vendido}€").ToList();
        EguneratuTaula(datuak);
    }

    /// <summary>
    /// Taulako datuak eguneratzeko metodoa.
    /// </summary>
    /// <param name="taulakoDatuak"></param>
    private void EguneratuTaula(List<string> taulakoDatuak)
    {
        InfoSection.Clear(); 

        foreach (var eguneratuakoDatuak in taulakoDatuak)
        {
            InfoSection.Add(new TextCell { Text = eguneratuakoDatuak });
        }
    }

}


