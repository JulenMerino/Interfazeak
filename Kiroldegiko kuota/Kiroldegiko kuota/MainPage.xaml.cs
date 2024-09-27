namespace Kiroldegiko_kuota
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radioButton = sender as RadioButton; if (radioButton.IsChecked) ;
        }
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            //Checkboxa sakatuta dagoen konprobatu
            if (checkBox.IsChecked)
            {
                // Ikusi zein dagoen aukeratuta
                if (checkBox == chDesgaitasuna)
                {
                    chDesgaitasuna.IsChecked = false; // Beste aukera desmarkatu
                }
                else if (checkBox == chFamiliaUgaria)
                {
                    chFamiliaUgaria.IsChecked = false; // Beste aukera desmarkatu

                }
            }
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
        }

        private void Kalkulatu(object sender, EventArgs e)
        {
            double prezioabase = 0;
            int Urtea = 365;
            int Astea = 7;
            int BiAste = 14;
            int hilabetea = 31;
            int Eguna = 1;

            if (rbUmea.IsChecked)
            {
                prezioabase = 2;
                if (chFamiliaUgaria.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 25) / 100;

                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }
                }
                else if (chDesgaitasuna.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 50) / 100;

                    //Desgaituen deskontua aplikatu
                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }

                }
            }
            else if (rbFamilia.IsChecked)
            {
                prezioabase = 5;
                if (chFamiliaUgaria.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 25) / 100;

                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }
                }
                else if (chDesgaitasuna.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 50) / 100;

                    //Desgaituen deskontua aplikatu
                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }

                }
                // Familien Kostu Basea ezarri
            }
            else if (rbJubilatua.IsChecked)
            {
                prezioabase = 4; // jubilatuen kostu basea ezarri
                if (chFamiliaUgaria.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 25) / 100;

                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }
                }
                else if (chDesgaitasuna.IsChecked)
                {
                    prezioabase = prezioabase - (prezioabase * 50) / 100;

                    //Desgaituen deskontua aplikatu
                    string selectedOption = (string)optionPicker.SelectedItem;
                    if (selectedOption == "Urtekoa")
                    {
                        prezioabase = prezioabase * Urtea;
                    }
                    else if (selectedOption == "Hilabetekoa")
                    {
                        prezioabase = prezioabase * hilabetea;
                    }
                    else if (selectedOption == "Bi astekoa")
                    {
                        prezioabase = prezioabase * BiAste;
                    }
                    else if (selectedOption == "Astekoa")
                    {
                        prezioabase = prezioabase * Astea;
                    }
                    else if (selectedOption == "Egunekoa")
                    {
                        prezioabase = prezioabase * Eguna;
                    }

                }
            }





            lblPrezioa.Text = prezioabase + " €";
        }

        private void garbitu(object sender, EventArgs e)
        {

            rbUmea.IsChecked = false;
            rbFamilia.IsChecked = false;
            rbJubilatua.IsChecked = false;

            lblPrezioa.Text = "";
            chDesgaitasuna.IsChecked = false;
            chFamiliaUgaria.IsChecked = false;
        }

        private void irten(object sender, EventArgs e)
        {
            Application.Current!.Quit();

        }




    }

}
