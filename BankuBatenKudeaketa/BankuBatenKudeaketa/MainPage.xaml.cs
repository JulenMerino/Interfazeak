using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using static BankuBatenKudeaketa.DatuBaseaMetodoak;

namespace BankuBatenKudeaketa
{
    public partial class MainPage : ContentPage
    {
        private DatuBaseaMetodoak datuBasea;

        public MainPage()
        {
            InitializeComponent();

            datuBasea = new DatuBaseaMetodoak();

            EguneratuLabelBezeroKopurua();
            LehenengoBezeroaKargatu();
            NANakKargatu();
        }



        // Bezero zatia



        /// <summary>
        /// Bezero kopurua datu basean lortzen duen eta lortutako balioa interfazearen etiketan
        /// eguneratzen duen metodoa.
        /// </summary>
        private async void EguneratuLabelBezeroKopurua()
        {
            int bezeroKopurua = await Task.Run(() => datuBasea.LortuBezeroKopurua());

            lblBezeroKopuraua.Text = $"de {bezeroKopurua}";
        }


        /// <summary>
        /// Lehenengo bezeroaren datuak lortzen ditu datu-baseatik eta eremuak eguneratzen ditu bezeroaren informazioarekin, 
        /// aurkitu ezean eremuak garbitzen ditu.
        /// </summary>
        private async void LehenengoBezeroaKargatu()
        {
            var (nan, izena) = await datuBasea.LortuBezeroaLerroarenAraberaAsync(1);

            if (nan != null && izena != null)
            {
                etyBezeroa.Text = "1";  
                etyNAN.Text = nan;
                etyIzena.Text = izena;
            }
            else
            {

                etyNAN.Text = "";
                etyIzena.Text = "";
            }
        }


        /// <summary>
        /// Hurrengo bezeroaren datuak lortzen ditu eta eremuak eguneratzen ditu. Bezeroa ez badago, eremuak garbitzen ditu.
        /// </summary>
        private async void BtnHurrengoa_Clicked(object sender, EventArgs e)
        {
            int bezeroKopurua = await Task.Run(() => datuBasea.LortuBezeroKopurua());
            if (int.TryParse(etyBezeroa.Text, out int lerroa) && lerroa < bezeroKopurua + 1)
            {
                lerroa++;
                etyBezeroa.Text = lerroa.ToString();

                var (nan, izena) = await datuBasea.LortuBezeroaLerroarenAraberaAsync(lerroa);

                if (nan != null && izena != null)
                {
                    etyNAN.Text = nan;
                    etyIzena.Text = izena;
                }
                else
                {
                    etyNAN.Text = "";
                    etyIzena.Text = "";
                }
            }
        }


        /// <summary>
        /// Aurreko bezeroaren datuak lortzen ditu eta eremuak eguneratzen ditu. Bezeroa ez badago, eremuak garbitzen ditu.
        /// </summary>
        private async void BtnAurrekoa_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(etyBezeroa.Text, out int lerroa) && lerroa > 1)
            {
                lerroa--;  
                etyBezeroa.Text = lerroa.ToString();  

                var (nan, izena) = await datuBasea.LortuBezeroaLerroarenAraberaAsync(lerroa);

                if (nan != null && izena != null)
                {
                    etyNAN.Text = nan;
                    etyIzena.Text = izena;
                }
                else
                {
                    etyNAN.Text = "";
                    etyIzena.Text = "";
                }
            }
        }


        /// <summary>
        /// Bezero berria datu-basean gehitzen du. NAN eta izena sartu gabe badaude, errore-mezua erakusten du. 
        /// Bezeroa ondo gehitzen bada, interfazea eguneratzen du eta mezu bat erakusten du.
        /// </summary>
        private async void BtnGehitu_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  
            string izena = etyIzena.Text.Trim();  


            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(izena))
            {
                await DisplayAlert("Errore", "Mesedez, sartu datu guztiak.", "OK");
                return;
            }

            bool arrakasta = await datuBasea.TxertatuBezeroaAsync(nan, izena);

            if (arrakasta)
            {
                await DisplayAlert("Arrakasta", "Bezeroa ondo gehitu da.", "OK");

                EguneratuLabelBezeroKopurua();  
                LehenengoBezeroaKargatu();  
            }
            else
            {
                await DisplayAlert("Errore", "Bezeroa gehitzeko arazo bat egon da.", "OK");
            }
        }


        /// <summary>
        /// Bezero bat datu-basean ezabatzen du. NAN eta izena sartu gabe badaude, errore-mezua erakusten du. 
        /// Bezeroa ondo ezabatzen bada, interfazea eguneratzen du eta mezu bat erakusten du.
        /// </summary>
        private async void BtnKendu_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  
            string izena = etyIzena.Text.Trim();  

            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(izena))
            {
                await DisplayAlert("Errore", "Mesedez, sartu datu guztiak.", "OK");
                return;
            }

            bool arrakasta = await datuBasea.EzabatuBezeroaAsync(nan, izena);

            if (arrakasta)
            {
                await DisplayAlert("Arrakasta", "Bezeroa ondo ezabatu da.", "OK");

                EguneratuLabelBezeroKopurua();  
                LehenengoBezeroaKargatu();  
            }
            else
            {
                await DisplayAlert("Errore", "Bezeroa ezabatzeko arazo bat egon da.", "OK");
            }
        }


        /// <summary>
        /// Bezero baten datuak eguneratzen ditu datu-basean. NAN eta izena sartu gabe badaude, errore-mezua erakusten du. 
        /// Bezeroa ondo eguneratzen bada, interfazea eguneratzen du eta mezu bat erakusten du.
        /// </summary>
        private async void BtnGorde_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  
            string izena = etyIzena.Text.Trim();  

            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(izena))
            {
                await DisplayAlert("Errore", "Mesedez, sartu datu guztiak.", "OK");
                return;
            }

            bool arrakasta = await datuBasea.EguneratuBezeroaAsync(nan, izena);

            if (arrakasta)
            {
                await DisplayAlert("Arrakasta", "Bezeroa ondo eguneratu da.", "OK");

                EguneratuLabelBezeroKopurua();  
                LehenengoBezeroaKargatu();  
            }
            else
            {
                await DisplayAlert("Errore", "Bezeroa eguneratzeko arazo bat egon da.", "OK");
            }
        }



        // Kontuen kudeaketa zatia



        /// <summary>
        /// Datu-basean dauden NAN guztiak kargatzen ditu eta hauek Picker-aren elementu gisa erakusten ditu.
        /// NAN zerrenda hutsik bada, errore-mezu bat erakusten du.
        /// </summary>
        private async void NANakKargatu()
        {
            List<string> nanLista = await datuBasea.LortuNanGuztiakAsync();

            PkNAN.Items.Clear();

            if (nanLista != null && nanLista.Count > 0)
            {
                PkNAN.ItemsSource = nanLista;  
            }
            else
            {
                await DisplayAlert("Ezin izan da NAN zerrenda lortu", "Ez dago NAN daturik.", "OK");
            }
        }


        /// <summary>
        /// Picker-aren elementu bat hautatzean, aukeratutako NANaren arabera bezeroaren izena eta kontuak, 
        /// baita maileguak ere kargatzen ditu. Bezeroa aurkitzen ez bada, errore-mezua erakusten du.
        /// </summary>
        private async void PkNAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PkNAN.SelectedIndex != -1)
            {
                string selectedNAN = PkNAN.SelectedItem.ToString();

                var (nan, izena) = await datuBasea.LortuBezeroaNanarenAraberaAsync(selectedNAN);

                if (izena != null)
                {
                    EtyIzenaAbizena.Text = izena;
                }
                else
                {
                    EtyIzenaAbizena.Text = "";
                    await DisplayAlert("Errore", "Bezeroa ez da aurkitu.", "OK");
                }

                List<string> kontuak = await datuBasea.LortuKontuakNanarenAraberaAsync(selectedNAN);

                LvDeskribapenaDepositua.ItemsSource = null;

                if (kontuak != null && kontuak.Count > 0)
                {
                    LvDeskribapenaDepositua.ItemsSource = kontuak;
                }
                else
                {
                    await DisplayAlert("Ez dira kontuak aurkitu", "Bezero honek ez du konturik.", "OK");
                }

                List<string> maileguak = await datuBasea.LortuMaileguakNanarenAraberaAsync(selectedNAN);

                LvDeskribapenaMailegua.ItemsSource = null;

                if (maileguak != null && maileguak.Count > 0)
                {
                    LvDeskribapenaMailegua.ItemsSource = maileguak;
                }
                else
                {
                    await DisplayAlert("Ez dira maileguak aurkitu", "Bezero honek ez du mailegurik.", "OK");
                }
            }
        }


        /// <summary>
        /// ListView-aren elementu bat hautatzen denean, "Aldatu Depositua" botoia gaitzen du. 
        /// Hautapenik ez badago, botoia desgaitzen du.
        /// </summary>
        private void LvDeskribapenaDepositua_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                BtnAldatuDepositua.IsEnabled = true;
            }
            else
            {
                BtnAldatuDepositua.IsEnabled = false;
            }
        }


        /// <summary>
        /// ListView-aren elementu bat hautatzen denean, "Mailegua Ezeztatu" botoia gaitzen du. 
        /// Hautapenik ez badago, botoia desgaitzen du.
        /// </summary>
        private void LvDeskribapenaMailegua_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                BtnMaileguaEzeztatu.IsEnabled = true;
            }
            else
            {
                BtnMaileguaEzeztatu.IsEnabled = false;
            }
        }


        /// <summary>
        /// "Aldatu Depo" botoia sakatzen denean, hautatutako gordailuaren deskripzioaren arabera saldoa lortzen du,
        /// eta saldo hori beste orri batera igarotzeko erabiltzen du. Kontu bat hautatu ez bada, errore-mezu bat erakusten du.
        /// </summary>
        private async void BtnAldatuDepositua_Clicked(object sender, EventArgs e)
        {
            if (LvDeskribapenaDepositua.SelectedItem != null)
            {
                string NanHautatua = PkNAN.SelectedItem.ToString();
                string deskripzioHautatua = LvDeskribapenaDepositua.SelectedItem.ToString();
                decimal? kantitatea = await datuBasea.LortuSaldoaDeskripzioarenAraberaAsync(NanHautatua, deskripzioHautatua);

                if (kantitatea.HasValue)
                {
                    await Navigation.PushAsync(new Gordailua(deskripzioHautatua, kantitatea.Value));
                }
                else
                {
                    await DisplayAlert("Errore", "Ez da posible lortzea gordailuaren zenbatekoa.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Adi", "Mesedez, hautatu kontu bat zerrendan.", "OK");
            }
        }


        /// <summary>
        /// Mailegu bat ezabatzeko prozesua kudeatzen du. Lehenik, erabiltzaileari baieztapena eskatzen dio
        /// eta, ondoren, datu basean mailegua ezabatzen saiatzen da. Arrakasta edo akatsaren arabera
        /// eguneratzen du interfazea eta mezuak erakusten ditu.
        /// </summary>
        private async void BtnMaileguaEzeztatu_Clicked(object sender, EventArgs e)
        {
 
            if (LvDeskribapenaMailegua.SelectedItem != null)
            {
                string deskripzioHautatua = LvDeskribapenaMailegua.SelectedItem.ToString();
                string nanHautatua = PkNAN.SelectedItem.ToString();

                bool baieztapena = await DisplayAlert("Konfirmazioa", "Ziur zaude mailegu hau ezabatu nahi duzula?", "Bai", "Ez");

                if (baieztapena)
                {
 
                    bool arrakasta = await datuBasea.EzabatuMaileguaDeskripzioarenAraberaAsync(deskripzioHautatua);

                    if (arrakasta)
                    {
                        KargatuMaileguenZerrenda(nanHautatua);
                        await DisplayAlert("Arrakasta", "Mailegua ondo ezabatu da.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Errore", "Ez da posible mailegua ezabatzea datu-basean.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Adi", "Mesedez, hautatu mailegu bat ezabatzeko.", "OK");
            }
        }


        /// <summary>
        /// "Inprimatu" botoia sakatzen denean, hautatutako bezeroaren, maileguaren, eta gordailuaren datuak lortzen ditu 
        /// eta inprimatzeko orri batera bideratzen ditu. Erabiltzailearen datuak edo mailegu edo saldoaren informazio falta badago,
        /// lehenetsitako balioekin inprimatzen da. Akats bat gertatzen bada, mezu bat erakusten du.
        /// </summary>
        private async void BtnInprimatu_Clicked(object sender, EventArgs e)
        {
            if (PkNAN.SelectedItem != null)
            {
                string NanHautatua = PkNAN.SelectedItem.ToString();
                string MaileguDeskripzioa = LvDeskribapenaMailegua.SelectedItem?.ToString() ?? string.Empty;
                string GordailuDeskripzioa = LvDeskribapenaDepositua.SelectedItem?.ToString() ?? "Ez dago eskuragarri";
                string IzenaAbizenaHautatua = EtyIzenaAbizena.Text; 

                try
                {
                    var mailegua = await datuBasea.LortuMaileguaNanEtaDeskribapenarenAraberaAsync(NanHautatua, MaileguDeskripzioa);
                    decimal? saldo = await datuBasea.LortuSaldoaDeskripzioarenAraberaAsync(NanHautatua, GordailuDeskripzioa);
                    string deskripzioMailegua = mailegua.Deskribapena ?? "Ez dago eskuragarri";
                    decimal kantitateaMailegua = mailegua.Zenbatekoa;  
                    int epeaMailegua = mailegua.Epea;  
                    DateTime dataMailegua = mailegua.Data == DateTime.MinValue ? DateTime.MinValue : mailegua.Data;
                    decimal saldoAzkena = saldo ?? 0.0m;

                    await Navigation.PushAsync(new Inprimatu(
                        deskripzioMailegua,
                        kantitateaMailegua,
                        epeaMailegua,
                        dataMailegua,
                        saldoAzkena,
                        GordailuDeskripzioa,
                        NanHautatua,
                        IzenaAbizenaHautatua
                    ));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Errore", "Datuak prozesatzerakoan akats bat egon da: " + ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Adi", "Mesedez, hautatu erabiltzaile bat.", "OK");
            }
        }


        /// <summary>
        /// Bezeroaren NAN-a erabiliz, bezeroaren maileguen zerrenda kargatzen du eta 
        /// hori ListView batean erakusten du. Zerrenda lortzeko datuBasea klaseko metodo bat erabiltzen da.
        /// </summary>
        private async Task KargatuMaileguenZerrenda(string pkNan)
        {
            var maileguenZerrenda = await datuBasea.LortuMaileguakNanarenAraberaAsync(pkNan);
            LvDeskribapenaMailegua.ItemsSource = maileguenZerrenda;
        }


        /// <summary>
        /// Botoiaren klikarekin, aplikazioa itzali egiten da.
        /// </summary>
        private void BtnIrten_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }


    }
}
