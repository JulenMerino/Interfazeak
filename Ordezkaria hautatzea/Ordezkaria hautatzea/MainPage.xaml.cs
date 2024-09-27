using System.Collections.ObjectModel;

namespace Ordezkaria_hautatzea
{
    public partial class MainPage : ContentPage
    {
        
        private ObservableCollection<string> ikasleakList = new ObservableCollection<string>();
        private ObservableCollection<string> ordezkoList = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            lvIkasleak.ItemsSource = ikasleakList;
            lvOrdezkariak.ItemsSource = ordezkoList;
        }

        
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            var ikasleIzena = etyIkasleIzena.Text?.Trim();
            if (!string.IsNullOrEmpty(ikasleIzena))
            {
                ikasleakList.Add(ikasleIzena);
                etyIkasleIzena.Text = string.Empty; 
            }
        }

        
        private void ausaz(object sender, EventArgs e)
        {
            if (ikasleakList.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(ikasleakList.Count);
                var hautatutakoIkaslea = ikasleakList[index]; 
                ikasleakList.RemoveAt(index); 
                ordezkoList.Add(hautatutakoIkaslea); 
            }
            else
            {
                
                DisplayAlert("Abisua", "Ez dago ikaslerik zerrendan.", "Onartu");
            }
        }

        
        private async void izendatu(object sender, EventArgs e)
        {
            if (ikasleakList.Count == 0)
            {
                
                await DisplayAlert("Abisua", "Ez dago ikaslerik Ordezkora mugituko.", "Onartu");
                return;
            }

            if (lvIkasleak.SelectedItem != null)
            {
                var hautatutakoIkaslea = lvIkasleak.SelectedItem.ToString();
                ikasleakList.Remove(hautatutakoIkaslea); 
                ordezkoList.Add(hautatutakoIkaslea); 

                
                lvIkasleak.SelectedItem = null;
            }
            else
            {
                
                await DisplayAlert("Abisua", "Hautatu behar duzu ikasle bat Ordezkora mugituko.", "Onartu");
            }
        }

        
        private async void kendu(object sender, EventArgs e)
        {
            if (ordezkoList.Count == 0)
            {
                
                await DisplayAlert("Abisua", "Ez dago ikaslerik Ordezkotik Ikasleetara mugituko.", "Onartu");
                return;
            }

            if (lvOrdezkariak.SelectedItem != null)
            {
                var hautatutakoIkaslea = lvOrdezkariak.SelectedItem.ToString();

                
                if (!ikasleakList.Contains(hautatutakoIkaslea))
                {
                    ordezkoList.Remove(hautatutakoIkaslea); 
                    ikasleakList.Add(hautatutakoIkaslea); 
                }

                
                lvOrdezkariak.SelectedItem = null;
            }
            else
            {
                
                await DisplayAlert("Abisua", "Hautatu behar duzu ikasle bat Ikasleetara mugituko.", "Onartu");
            }
        }

        
        private void hustu(object sender, EventArgs e)
        {
            if (ordezkoList.Count == 0)
            {
               
                DisplayAlert("Abisua", "Ez dago ikaslerik Ordezkotik Ikasleetara mugituko.", "Onartu");
                return;
            }

            foreach (var ikaslea in ordezkoList.ToList())
            {
                if (!ikasleakList.Contains(ikaslea))
                {
                    ordezkoList.Remove(ikaslea); 
                    ikasleakList.Add(ikaslea);
                }
            }
        }

        
        private void SelectUp(object sender, EventArgs e)
        {
            if (lvOrdezkariak.ItemsSource != null)
            {
                var lista = lvOrdezkariak.ItemsSource as ObservableCollection<string>;

                
                if (lvOrdezkariak.SelectedItem != null)
                {
                    int hautatutakoIndizea = lista.IndexOf(lvOrdezkariak.SelectedItem.ToString());
                    if (hautatutakoIndizea > 0) 
                    {
                        lvOrdezkariak.SelectedItem = lista[hautatutakoIndizea - 1]; 
                    }
                }
                else 
                {
                    lvOrdezkariak.SelectedItem = lista[0]; 
                }
            }
        }

        
        private void SelectDown(object sender, EventArgs e)
        {
            if (lvOrdezkariak.ItemsSource != null)
            {
                var lista = lvOrdezkariak.ItemsSource as ObservableCollection<string>;

                
                if (lvOrdezkariak.SelectedItem != null)
                {
                    int hautatutakoIndizea = lista.IndexOf(lvOrdezkariak.SelectedItem.ToString());
                    if (hautatutakoIndizea < lista.Count - 1) 
                    {
                        lvOrdezkariak.SelectedItem = lista[hautatutakoIndizea + 1]; // Hurrengo elementua hautatu
                    }
                }
                else 
                {
                    lvOrdezkariak.SelectedItem = lista[0]; 
                }
            }
        }
    }
}
