using System.Text;
using System.Windows.Input;
using BankuKudeaketa.Modeloak;
using SQLite;

namespace BankuKudeaketa.Views;

public partial class KontuakView : ContentPage
{
    public ICommand CommandGordagailua { get; private set; }
    public ICommand CommandItxi { get; private set; }
    public ICommand CommandEzabatuMailegua { get; private set; }

    public ICommand CommandImprimatu { get; private set; }


    public SQLiteConnection datubasea = DBManager.Instantzia.Db;

    public KontuakView()
    {
        InitializeComponent();

        CommandGordagailua = new Command(IrekiGordagailua);
        CommandItxi = new Command(Itxi);
        CommandEzabatuMailegua = new Command(EzabatuMailegua);
        CommandImprimatu = new Command(Imprimatu);

        ButtonModifikatu.Akzioa = CommandGordagailua;
        ButtonIrten.Akzioa = CommandItxi;
        ButtonEzeztatu.Akzioa = CommandEzabatuMailegua;
        ButtonImprimitu.Akzioa = CommandImprimatu;

        PickerBezeroak.ItemsSource = datubasea.Table<Bezeroa>().ToList();
    }
    /// <summary>
    /// Gordagailuaren informazioa irikitzen du beste window batean
    /// </summary>
    private void IrekiGordagailua()
    {
        var tempGordagailua = (Gordailua)ListViewGordailuak.SelectedItem;
        var lehioa = new Gordagailua(tempGordagailua);
        var window = new Window(lehioa)
        {
            Width = 500,
            Height = 300
        };
        Application.Current?.OpenWindow(window);
    }

    /// <summary>
    /// uneko lehioa ixten du
    /// </summary>
    private void Itxi()
    {
        Application.Current?.CloseWindow(this.Window);
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

        ButtonEzeztatu.Aktibatuta = false;
        ButtonModifikatu.Aktibatuta = false;
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
            ButtonEzeztatu.Aktibatuta = true;
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
            ButtonModifikatu.Aktibatuta = true;
        }
    }

    /// <summary>
    /// Mailegua Ezabatu botoia klikatutakoan Aukeratutako Mailegua ezabatzeaz enkargatzen de funtzioa
    /// </summary>
    private void EzabatuMailegua()
    {
        var tempMailegua = (Mailegua)ListViewMaileguak.SelectedItem;
        datubasea.Insert(new del_Mailegua(tempMailegua));
        datubasea.Delete(tempMailegua);

        PickerBezeroak_Aukeratu(new object(), new EventArgs());
    }

    /// <summary>
    /// Metodo honek Pickerrean haukeratutako Bezeroren datuak erakusten ditu beste pantaila batean
    /// </summary>
    private void Imprimatu()
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
}
