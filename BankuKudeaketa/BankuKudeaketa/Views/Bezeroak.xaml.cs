//Nork sortua: Hegoi Ruiz
//Noiz sortua:
//
//1.Aldaketa
//Nork aldatua: Julen Merino
//Noiz aldatua: 13/12/2024

//Mugikorrean ongi ikusteko aldaketak 

using BankuKudeaketa.Modeloak;
using SQLite;

namespace BankuKudeaketa.Views;


public partial class BezeroakView : ContentPage
{

    private long _posizioa = 1;
    private long _posizioMax;
    public SQLiteConnection datubasea = DBManager.Instantzia.Db;
    public Bezeroa bezeroa;
    public BezeroakView()
	{
        
        bezeroa = datubasea.Find<Bezeroa>(_posizioa);
        _posizioMax = datubasea.Table<Bezeroa>().Count() -1;

        InitializeComponent();
        AldatuErregistroa(_posizioa);
        GailuArabera();

    }

    private void GailuArabera()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
        {
            BotoienTamaina(40); 
            EntryBezeroakCurrent.WidthRequest = 40;
            EntryBezeroakCurrent.FontSize = 15;
            LabelBezeroakCount.WidthRequest = 40;
            LabelBezeroakCount.FontSize = 15;
            LabelDNI.FontSize = 12;
            LabelDNI.HeightRequest = 15;
            EntryNan.FontSize = 12;
            EntryNan.WidthRequest = 100;
            LabelIzena.FontSize = 12;
            LabelIzena.HeightRequest = 15;
            LabelIzena.Margin = new Thickness(7, 0, 0, 0);
            EntryIzena.FontSize = 12;
            EntryIzena.WidthRequest = 100;


        }
        else
        {
            BotoienTamaina(100);
            EntryBezeroakCurrent.WidthRequest = 100;
            EntryBezeroakCurrent.FontSize = 40;
            LabelBezeroakCount.WidthRequest = 100;
            LabelBezeroakCount.FontSize = 40;
            LabelDNI.FontSize = 40;
            LabelDNI.HeightRequest = 100;
            EntryNan.FontSize = 40;
            EntryNan.WidthRequest = 300;
            LabelIzena.FontSize = 40;
            LabelIzena.HeightRequest = 100;
            LabelIzena.Margin = new Thickness(-12, 0, 0, 0);
            EntryIzena.FontSize = 40;
            EntryIzena.WidthRequest = 300;
        }
    }

    // Método para actualizar el tamaño de los botones.
    private void BotoienTamaina(int tamaina)
    { 
        ImageButton[] botoiak = {
            ImageButtonHasiera,
            ImageButtonAtzera,
            ImageButtonAurrera,
            ImageButtonBukaera,
            ImageButtonGehitu,
            ImageButtonEzabatu,
            ImageButtonGorde,
            
        };

        foreach (var botoienTamaina in botoiak)
        {
            botoienTamaina.WidthRequest = tamaina;
            botoienTamaina.HeightRequest = tamaina;
        }
    }

    /// <summary>
    /// Aukeratutako erregistroa aldatzen denean ejutatzen den kodea honek erregistroaren datuen arabera betetzen du interfazea
    /// </summary>
    /// <param name="posizioa"> bezeroaren id-a</param> 
    private void AldatuErregistroa(long  posizioa)
    {
        _posizioa = posizioa;
        var bezeroak = datubasea.Table<Bezeroa>().ToArray();
        bezeroa = bezeroak[posizioa];

        EntryIzena.Text = bezeroa?.Izena ?? "";
        EntryNan.Text = bezeroa?.Nan ?? "";

        EntryBezeroakCurrent.Text = _posizioa.ToString();

        _posizioMax = datubasea.Table<Bezeroa>().Count() -1;
        LabelBezeroakCount.Text = "-" + _posizioMax + "-tik";

    }
    /// <summary>
    /// 1-id duen bezerora datuak erakusten ditu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LehenErregistrora(object sender, EventArgs e)
    {
        AldatuErregistroa(1);
    }

    /// <summary>
    /// Entryan sartutako id-ra aldatzen du bezeroa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EntryIdAldatu(object sender, TextChangedEventArgs e)
    {
        _posizioa = long.Parse(EntryBezeroakCurrent.Text);
        if (_posizioa < 0 || _posizioa > _posizioMax) 
        {
            EntryBezeroakCurrent.Text = _posizioa.ToString();
            return; 
        }

        AldatuErregistroa(_posizioa);
    }

    /// <summary>
    /// haurreko bezerora itzultzen da exititzen bada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ErregistroaAtzera(object sender, EventArgs e)
    {
        var posizioa = _posizioa - 1;
        if (posizioa < 0 || posizioa > _posizioMax)
        {
            EntryBezeroakCurrent.Text = _posizioa.ToString();
            return;
        }
        AldatuErregistroa(posizioa);
    }

    /// <summary>
    /// Hurrengo bezerora pasatzen da exisitzen bada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ErregistroaAurrera(object sender, EventArgs e)
    {
        var posizioa = _posizioa + 1;
        if (posizioa < 0 || posizioa > _posizioMax)
        {
            EntryBezeroakCurrent.Text = _posizioa.ToString();
            return;
        }
        AldatuErregistroa(posizioa);
    }
    /// <summary>
    /// Azken bezerora joaten da
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AzkenErregistrora(object sender, EventArgs e)
    {
        AldatuErregistroa(_posizioMax);
    }

    /// <summary>
    /// Bezero berri bate sortzeko pantaila erakusten du
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BezeroBerria(object sender, EventArgs e)
    {
        AldatuErregistroa(_posizioMax + 1);
    }
    /// <summary>
    /// Aukeratutako bezeroa ezabatzen du
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EzabatuBezeroa(object sender, EventArgs e)
    {
        datubasea.Insert(new del_Bezeroa(bezeroa));
        datubasea.Delete(bezeroa);
        AldatuErregistroa(_posizioMax);
       
    }

    /// <summary>
    /// Bezeroa exititzen bada hau aktualizatzen du bestela berria sortu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Gordebezeroa(object sender, EventArgs e)
    {
        bezeroa = new Bezeroa();
        bezeroa.Izena = EntryIzena.Text;
        bezeroa.Nan = EntryNan.Text;
        if (datubasea.Find<Bezeroa>(bezeroa.Id) != null)
        {
        datubasea.Update(bezeroa);
            
        }
        else
        {
            datubasea.Insert(bezeroa);
        }
        AldatuErregistroa(_posizioa);
    }
}