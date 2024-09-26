namespace Batzen_ikasten
{
    public partial class MainPage : ContentPage
    {
        int batugaia1;
        int batugaia2;
        int sartutakoEmaitza;

        public MainPage()
        {
            InitializeComponent();
        }

        private void balioBerriak(object sender, EventArgs e)
        {
            Random rnd = new Random();
            batugaia1 = (int)(rnd.NextDouble() * 10) + 1;
            batugaia2 = (int)(rnd.NextDouble() * 10) + 1;

            EtyLehenBatugaia.Text = batugaia1.ToString();
            EtyBigarrenBatugaia.Text = batugaia2.ToString();

            EtySartutakoa.Text = "";
            EtyEmaitza.Text = "";
        }

        private void egiaztatu(object sender, EventArgs e)
        {
            int sartutakoEmaitza;
            int.TryParse(EtySartutakoa.Text, out sartutakoEmaitza);

            int batuketa = batugaia1 + batugaia2;

            if (sartutakoEmaitza == batuketa)
            {
                EtyEmaitza.Text = "Zorionak, Badakizu batuketa hori egiten!";
            }
            else
            {
                EtyEmaitza.Text = "Ez duzu asmatu, erantzun zuzena: " + batuketa.ToString();
            }
        }
        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }

}
