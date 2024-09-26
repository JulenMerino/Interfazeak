namespace DeskontuakDituenFaktura
{
    public partial class MainPage : ContentPage
    {
        private const decimal BEZehuneko = 21m; 

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            
            CalculateValues();
        }

        private void OnKalkulatuClicked(object sender, EventArgs e)
        {
            
            CalculateValues();
        }

        private void CalculateValues()
        {
            if (int.TryParse(etyKantitatea.Text, out int cantidad) && decimal.TryParse(etyPrezioa.Text, out decimal precioUnitario))
            {
                
                decimal ehunekoDeskontua = GetEhunekoDeskontua(cantidad);
                decimal subtotala = cantidad * precioUnitario;
                decimal deskontua = subtotala * (ehunekoDeskontua / 100);
                decimal guztiraBezGabe = subtotala - deskontua;
                decimal BEZ = guztiraBezGabe * (BEZehuneko / 100);
                decimal guztira = guztiraBezGabe + BEZ;

                
                etyDenera.Text = guztiraBezGabe.ToString("F2");
                etyDeskontua.Text = ehunekoDeskontua.ToString("F0");
                etyDeskontuaGuztia.Text = deskontua.ToString("F2");
                etyBEZ.Text = BEZehuneko.ToString("F0");
                etyBEZGuztia.Text = BEZ.ToString("F2");
                etyGuztira.Text = guztira.ToString("F2");
            }
            else
            {
               
                ClearOutputs();
            }
        }

        private decimal GetEhunekoDeskontua(int kantitatea)
        {
           
            if (kantitatea >= 1000) return 10m; 
            if (kantitatea >= 100) return 5m;   
            if (kantitatea >= 10) return 2m;    
            return 0m; 
        }

        private void ClearOutputs()
        {
            etyDenera.Text = string.Empty;
            etyDeskontua.Text = string.Empty;
            etyDeskontuaGuztia.Text = string.Empty;
            etyBEZ.Text = string.Empty;
            etyBEZGuztia.Text = string.Empty;
            etyGuztira.Text = string.Empty;
        }

        private void irten(object sender, EventArgs e)
        {
            Application.Current!.Quit();

        }

    }
}
