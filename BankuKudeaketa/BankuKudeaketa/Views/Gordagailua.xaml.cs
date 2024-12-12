using BankuKudeaketa.Modeloak;
using SQLite;
using System.Windows.Input;

namespace BankuKudeaketa.Views;

public partial class Gordagailua : ContentPage
{
    public ICommand CommandItxi { get; private set; }
    public ICommand CommandEzabatu { get; private set; }
    public ICommand CommandModifikatu { get; private set; }

    private SQLiteConnection datubasea = DBManager.Instantzia.Db;
    private Gordailua gordagailua;

    public Gordagailua(Gordailua gordailua)
	{
		InitializeComponent();
        CommandItxi = new Command(Itxi);
        CommandEzabatu = new Command(Ezabatu);
        CommandModifikatu = new Command(Modifikatu);

        ButtonIrten.Akzioa = CommandItxi;
        ButtonEzabatu.Akzioa = CommandEzabatu;
        ButtonModifikatu.Akzioa = CommandModifikatu;

        gordagailua = gordailua;
        EntrySaldo.Text = gordailua.Saldo.ToString();
        EntryDeskripzioa.Text = gordailua.Deskripzioa;
    }

    /// <summary>
    /// Horri honen lehio isten du.
    /// </summary>
    private void Itxi()
    {
        Application.Current?.CloseWindow(this.Window);
    }

    /// <summary>
    /// Datubasean gordailua ezabatzen du eta lehioa itxi.   
    /// </summary>
    private void Ezabatu()
    {
        datubasea.Insert(new del_Gordailua(gordagailua));
        datubasea.Delete(gordagailua);
        Itxi();
    }

    /// <summary>
    /// Gordailua datubasean modifikatzen du. Interfazeko datuaekin.
    /// </summary>
    private void Modifikatu()
    {
        gordagailua.Deskripzioa = EntryDeskripzioa.Text;
        gordagailua.Saldo = int.Parse(EntrySaldo.Text);

        datubasea.Insert(gordagailua);
    }

    /// <summary>
    /// Metodo honek Entry-an zebakiak bakarrik sartzeko erabiltzen da.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(e.NewTextValue, out _))
        {
            
            ((Entry)sender).Text = e.OldTextValue;
        }
    }
}