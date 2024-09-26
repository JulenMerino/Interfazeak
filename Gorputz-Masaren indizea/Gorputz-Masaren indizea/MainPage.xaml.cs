namespace Gorputz_Masaren_indizea
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Kalkulatu(object sender, EventArgs e)
        {
            double altuera;
            double pisua;

            
            altuera = Convert.ToDouble(EtyAltuera.Text);
            pisua = Convert.ToDouble(EtyPisua.Text);

            altuera /= 100;

            double gmk = pisua / Math.Pow(altuera, 2);

            GMKEntry.Text = gmk.ToString("F2");
        }
        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }

}
