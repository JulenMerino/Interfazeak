using Ordezkaritza.Data;
using Ordezkaritza.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
namespace Ordezkaritza.Views;

public partial class FakturaIkusi : ContentPage
{
    private Partner _partnerSeleccionado;
    private readonly Database _database;
    private Bidalketa _bidalketa;
    private string _eskaeraKod;

    public FakturaIkusi(Partner partner, string eskaeraKod)
    {
        InitializeComponent();

        _database = new Database("Komertzialak.db");
        _partnerSeleccionado = partner;
        _eskaeraKod = eskaeraKod;


        ErakutsiPartnerInformazioa();
        KargatuFakturaProduktuak();
        KargatuBidalketa();
    }


    /// <summary>
    /// Bidalketa datuak kargatzen ditu.
    /// </summary>
    private async void KargatuBidalketa()
    {
        if (!string.IsNullOrEmpty(_eskaeraKod))
        {
            _bidalketa = await _database.GetBidalketaByEskaeraKodAsync(int.Parse(_eskaeraKod));

            if (_bidalketa != null)
            {
                lblFakturaData.Text = _bidalketa.Data;
                lblFakturaKodea.Text = _bidalketa.Eskaera_kod.ToString();
            }
        }
    }

    /// <summary>
    /// Partnerren informazioa erakusten du.
    /// </summary>
    private void ErakutsiPartnerInformazioa()
    {
        if (_partnerSeleccionado != null)
        {
            lblPartnerIzena.Text = _partnerSeleccionado.Izena;
            lblPartnerHelbidea.Text = _partnerSeleccionado.Helbidea;
            lblPartnerTelefonoa.Text = _partnerSeleccionado.Telefonoa;
        }
    }

    /// <summary>
    /// Produktuen informazioa taulan kargatzeko metodoa.
    /// </summary>
    private async void KargatuFakturaProduktuak()
    {
        List<Eskaera_Xehetasuna> eskaerak = await _database.GetEskaeraByEskaeraKodAsync(int.Parse(_eskaeraKod));

        tbProduktuak.Clear();

        foreach (var produktuak in eskaerak)
        {
            var cell = new ViewCell
            {
                View = new HorizontalStackLayout
                {
                    Children =
                    {
                        new Label { Text = produktuak.Deskribapena, Margin = new Thickness(200,0,100,0), WidthRequest = 200, VerticalTextAlignment = TextAlignment.Center },
                        new Label { Text = produktuak.Kantitatea.ToString(), Margin = new Thickness(0,0,100,0), WidthRequest = 200, VerticalTextAlignment = TextAlignment.Center },
                        new Label { Text = produktuak.Prezioa.ToString("C", CultureInfo.CurrentCulture), Margin = new Thickness(-50,0,100,0), WidthRequest = 150, VerticalTextAlignment = TextAlignment.Center },
                        new Label { Text = produktuak.Guztira.ToString("C", CultureInfo.CurrentCulture), Margin = new Thickness(25,0,100,0), WidthRequest = 150, VerticalTextAlignment = TextAlignment.Center }
                    }
                }
            };

            tbProduktuak.Add(cell);
        }
    }

    /// <summary>
    /// Botoia sakatzen informazioa XML fitxategian gorde eta kantitatea stock-etik kenduko da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnSortuXML_Clicked(object sender, EventArgs e)
    {
        if (_bidalketa == null || _partnerSeleccionado == null)
        {
            await DisplayAlert("Errorea", "Ezin da XML sortu: falta datuak.", "OK");
            return;
        }

        List<Eskaera_Xehetasuna> eskaerak = await _database.GetEskaeraByEskaeraKodAsync(int.Parse(_eskaeraKod));

        XElement fakturaXml = new XElement("Faktura",
            new XElement("Partner",
                new XElement("Izena", _partnerSeleccionado.Izena),
                new XElement("Helbidea", _partnerSeleccionado.Helbidea),
                new XElement("Telefonoa", _partnerSeleccionado.Telefonoa)
            ),
            new XElement("Bidalketa",
                new XElement("Data", _bidalketa.Data),
                new XElement("EskaeraKod", _bidalketa.Eskaera_kod)
            ),
            new XElement("Produktuak",
                eskaerak.Select(eskaera =>
                    new XElement("Produktua",
                        new XElement("Deskribapena", eskaera.Deskribapena),
                        new XElement("Kantitatea", eskaera.Kantitatea),
                        new XElement("Prezioa", eskaera.Prezioa.ToString("C", CultureInfo.CurrentCulture)),
                        new XElement("Guztira", eskaera.Guztira.ToString("C", CultureInfo.CurrentCulture))
                    )
                )
            )
        );

        string fitxateguiHelbidea = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Faktura.xml");
        fakturaXml.Save(fitxateguiHelbidea);

        Debug.Write(fitxateguiHelbidea);

        foreach (var eskaera in eskaerak)
        {
            var produktua = await _database.GetProduktuaByKodAsync(int.Parse(eskaera.Produktu_kod));
            if (produktua != null)
            {
                produktua.Stock = Math.Max(0, produktua.Stock - eskaera.Kantitatea);
                await _database.UpdateKatalogoaAsync(produktua);
            }
        }

        Informazioa.AktualizatuProduktuaGlobal();

        await DisplayAlert("XML Sortuta", "Faktura XML fitxategia sortu da eta stock eguneratu da.", "OK");
    }

}
