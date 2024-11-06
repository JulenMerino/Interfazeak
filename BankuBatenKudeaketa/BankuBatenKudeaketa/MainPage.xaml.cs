namespace BankuBatenKudeaketa
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            
        }




        private void BtnIrten_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }

}
