//Nork sortua: Hegoi Ruiz
//Noiz sortua:


//1.Aldaketa
//Nork aldatua: Julen Merino
//Noiz aldatua: 13/12/2024

//Botoi berriak sortu ditudanez, botoientzako metodoak sortu ditut

using BankuKudeaketa.Modeloak;
using SQLite;
using System.Windows.Input;

namespace BankuKudeaketa.Views;

public partial class Gordagailua : ContentPage
{

    private SQLiteConnection datubasea = DBManager.Instantzia.Db;
    private Gordailua gordagailua;

    public Gordagailua(Gordailua gordailua)
	{
		InitializeComponent();

        gordagailua = gordailua;
        EntrySaldo.Text = gordailua.Saldo.ToString();
        EntryDeskripzioa.Text = gordailua.Deskripzioa;
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

    /// <summary>
    /// Datubasean gordailua ezabatzen du eta lehioa itxi.   
    /// </summary>
    private void ButtonEzabatu_Clicked(object sender, EventArgs e)
    {
        datubasea.Insert(new del_Gordailua(gordagailua));
        datubasea.Delete(gordagailua);
        Application.Current?.CloseWindow(this.Window);
    }

    private void ButtonModifikatu_Clicked(object sender, EventArgs e)
    {
        gordagailua.Deskripzioa = EntryDeskripzioa.Text;
        gordagailua.Saldo = int.Parse(EntrySaldo.Text);

        datubasea.Insert(gordagailua);
    }

    /// <summary>
    /// Horri honen lehio isten du.
    /// </summary>
    private void ButtonIrten_Clicked(object sender, EventArgs e)
    {
        Application.Current?.CloseWindow(this.Window);
    }
}