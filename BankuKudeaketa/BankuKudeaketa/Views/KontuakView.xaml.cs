//Nork sortua: Hegoi Ruiz
//Noiz sortua:
//
//1.Aldaketa
//Nork aldatua: Julen Merino
//Noiz aldatua: 13/12/2024

//Mugikorrean ongi ikusteko aldaketak 
//Botoi berriak sortu ditudanez, botoientzako metodoak sortu ditut
//Leiho berri bat ireki beharrean, leiho berean beste leiho baterako trantsizioa egin dut, eta itzultzeko aukera du.

using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using BankuKudeaketa.Modeloak;
using SQLite;

namespace BankuKudeaketa.Views;

public partial class KontuakView : ContentPage
{

    public SQLiteConnection datubasea = DBManager.Instantzia.Db;

    public KontuakView()
    {
        InitializeComponent();
        GailuArabera();

        PickerBezeroak.ItemsSource = datubasea.Table<Bezeroa>().ToList();
    }

    private void GailuArabera()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
        {
            ButtonModifikatu.HeightRequest = 40;
            ButtonImprimitu.HeightRequest = 40;
            ButtonEzeztatu.HeightRequest = 40;
            ButtonIrten.HeightRequest = 40;
            PickerBezeroak.HeightRequest = 40;
            EntryIzena.HeightRequest = 40;
        }
        else
        {
            ButtonModifikatu.HeightRequest = 60;
            ButtonImprimitu.HeightRequest = 60;
            ButtonEzeztatu.HeightRequest = 60;
            ButtonIrten.HeightRequest = 60;
            PickerBezeroak.HeightRequest = 10;
            EntryIzena.HeightRequest = 40;
        }
    }

    /// <summary>
    /// Pickerraren aukera aldatzen interfazeko datu guztiak aktulizatzeak enkargatzen da
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PickerBezeroak_Aukeratu(object sender, EventArgs e)
    {
        Bezeroa bezeroa = (Bezeroa)PickerBezeroak.SelectedItem;

        EntryIzena.Text = bezeroa.Izena;

        ListViewGordailuak.ItemsSource = datubasea.Table<Gordailua>().Where(g => g.Nan == bezeroa.Nan).ToList();

        ListViewMaileguak.ItemsSource = datubasea.Table<Mailegua>().Where(g => g.Nan == bezeroa.Nan).ToList();

        ButtonEzeztatu.IsEnabled = false;
        ButtonModifikatu.IsEnabled = false;
    }

    /// <summary>
    /// Maileguak Listan Item bat aukeratutakoan botoia baliagarri jartzen duen metododa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Mailegua_Aukeratu(object sender, SelectedItemChangedEventArgs e)
    {
        if (ListViewMaileguak.SelectedItem != null)
        {
            ButtonEzeztatu.IsEnabled = true;
        }
    }

    /// <summary>
    /// Gordailua Listan Item bat aukeratutakoan botoia baliagarri jartzen deun metodoa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Gordailua_Aukeratu(object sender, SelectedItemChangedEventArgs e)
    {
        if (ListViewGordailuak.SelectedItem != null)
        {
            ButtonModifikatu.IsEnabled = true;
        }
    }

    /// <summary>
    /// Gordagailuaren informazioa irikitzen du beste window batean
    /// </summary>
    private async void ButtonModifikatu_Clicked(object sender, EventArgs e)
    {
        var tempGordagailua = (Gordailua)ListViewGordailuak.SelectedItem;
        await Navigation.PushAsync(new Gordagailua(tempGordagailua));
    }

    /// <summary>
    /// Mailegua Ezabatu botoia klikatutakoan Aukeratutako Mailegua ezabatzeaz enkargatzen de funtzioa
    /// </summary>
    private void ButtonEzeztatu_Clicked(object sender, EventArgs e)
    {
        var tempMailegua = (Mailegua)ListViewMaileguak.SelectedItem;
        datubasea.Insert(new del_Mailegua(tempMailegua));
        datubasea.Delete(tempMailegua);

        PickerBezeroak_Aukeratu(new object(), new EventArgs());
    }

    /// <summary>
    /// Metodo honek Pickerrean haukeratutako Bezeroren datuak erakusten ditu beste pantaila batean
    /// </summary>
    private void ButtonImprimitu_Clicked(object sender, EventArgs e)
    {
        if (PickerBezeroak.SelectedItem == null) return;

        StringBuilder testua = new StringBuilder();

        testua.AppendLine("Bezeroa: " + ((Bezeroa)PickerBezeroak.SelectedItem));

        testua.AppendLine("Gordailuak:");

        foreach (Gordailua g in ListViewGordailuak.ItemsSource)
        {
            testua.AppendLine(g.ToString());
        }
        testua.AppendLine("Maileguak:");

        foreach (Mailegua m in ListViewMaileguak.ItemsSource)
        {
            testua.AppendLine(m.ToString());
        }


        Label label = new Label();
        label.Text = testua.ToString();

        ContentPage page = new ContentPage();
        page.Content = label;

        Window window = new Window(page);
        Application.Current?.OpenWindow(window);
    }

    /// <summary>
    /// uneko lehioa ixten du
    /// </summary>
    private void ButtonIrten_Clicked(object sender, EventArgs e)
    {
        Application.Current?.CloseWindow(this.Window);
    }

}
