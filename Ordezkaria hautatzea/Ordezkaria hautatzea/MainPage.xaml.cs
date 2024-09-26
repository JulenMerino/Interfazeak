using System.Collections.ObjectModel;

namespace Ordezkaria_hautatzea
{
    public partial class MainPage : ContentPage
    {
        // Colecciones para las listas
        private ObservableCollection<string> ikasleakList = new ObservableCollection<string>();
        private ObservableCollection<string> ordezkoList = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            lvIkasleak.ItemsSource = ikasleakList;
            lvOrdezkariak.ItemsSource = ordezkoList;
        }

        // Método para añadir alumno
        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            var studentName = etyIkasleIzena.Text?.Trim();
            if (!string.IsNullOrEmpty(studentName))
            {
                ikasleakList.Add(studentName);
                etyIkasleIzena.Text = string.Empty; // Limpiar el Entry
            }
        }

        // Método para seleccionar un alumno aleatorio y moverlo a Ordezko
        private void ausaz(object sender, EventArgs e)
        {
            if (ikasleakList.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(ikasleakList.Count);
                var selectedStudent = ikasleakList[index]; // Seleccionar el alumno aleatorio
                ikasleakList.RemoveAt(index); // Remover de Ikasleak
                ordezkoList.Add(selectedStudent); // Añadir a Ordezko
            }
            else
            {
                // Mostrar alerta si Ikasleak está vacío
                DisplayAlert("Advertencia", "No hay ikasles en la lista.", "Aceptar");
            }
        }

        // Mover seleccionado de Ikasleak a Ordezko y deseleccionar
        private async void izendatu(object sender, EventArgs e)
        {
            if (ikasleakList.Count == 0)
            {
                // Mostrar alerta si Ikasleak está vacío
                await DisplayAlert("Advertencia", "No hay ikasles para mover a Ordezko.", "Aceptar");
                return;
            }

            if (lvIkasleak.SelectedItem != null)
            {
                var selectedStudent = lvIkasleak.SelectedItem.ToString();
                ikasleakList.Remove(selectedStudent); // Remover de Ikasleak
                ordezkoList.Add(selectedStudent); // Añadir a Ordezko

                // Limpiar la selección
                lvIkasleak.SelectedItem = null;
            }
            else
            {
                // Mostrar alerta si no hay selección
                await DisplayAlert("Advertencia", "Tienes que seleccionar un alumno antes de moverlo a Ordezko.", "Aceptar");
            }
        }

        // Mover seleccionado de Ordezko a Ikasleak y deseleccionar
        private async void kendu(object sender, EventArgs e)
        {
            if (ordezkoList.Count == 0)
            {
                // Mostrar alerta si Ordezko está vacío
                await DisplayAlert("Advertencia", "No hay ikasles en la lista de Ordezko para mover a Ikasleak.", "Aceptar");
                return;
            }

            if (lvOrdezkariak.SelectedItem != null)
            {
                var selectedStudent = lvOrdezkariak.SelectedItem.ToString();

                // Solo mover si el alumno no está ya en Ikasleak
                if (!ikasleakList.Contains(selectedStudent))
                {
                    ordezkoList.Remove(selectedStudent); // Remover de Ordezko
                    ikasleakList.Add(selectedStudent); // Añadir a Ikasleak
                }

                // Limpiar la selección
                lvOrdezkariak.SelectedItem = null;
            }
            else
            {
                // Mostrar alerta si no hay selección
                await DisplayAlert("Advertencia", "Tienes que seleccionar un alumno antes de moverlo a Ikasleak.", "Aceptar");
            }
        }

        // Mover todos los de Ordezko a Ikasleak
        private void hustu(object sender, EventArgs e)
        {
            if (ordezkoList.Count == 0)
            {
                // Mostrar alerta si Ordezko está vacío
                DisplayAlert("Advertencia", "No hay ikasles en la lista de Ordezko para mover a Ikasleak.", "Aceptar");
                return;
            }

            foreach (var student in ordezkoList.ToList())
            {
                if (!ikasleakList.Contains(student))
                {
                    ordezkoList.Remove(student); // Remover de Ordezko
                    ikasleakList.Add(student); // Añadir a Ikasleak
                }
            }
        }

        // Método para seleccionar el elemento anterior
        private void SelectUp(object sender, EventArgs e)
        {
            if (lvOrdezkariak.ItemsSource != null)
            {
                var list = lvOrdezkariak.ItemsSource as ObservableCollection<string>;

                // Si hay un elemento seleccionado
                if (lvOrdezkariak.SelectedItem != null)
                {
                    int selectedIndex = list.IndexOf(lvOrdezkariak.SelectedItem.ToString());
                    if (selectedIndex > 0) // Comprobar que no estamos en el primer elemento
                    {
                        lvOrdezkariak.SelectedItem = list[selectedIndex - 1]; // Seleccionar el elemento anterior
                    }
                }
                else // Si no hay ningún elemento seleccionado
                {
                    lvOrdezkariak.SelectedItem = list[0]; // Seleccionar el primer elemento
                }
            }
        }

        // Método para seleccionar el elemento siguiente
        private void SelectDown(object sender, EventArgs e)
        {
            if (lvOrdezkariak.ItemsSource != null)
            {
                var list = lvOrdezkariak.ItemsSource as ObservableCollection<string>;

                // Si hay un elemento seleccionado
                if (lvOrdezkariak.SelectedItem != null)
                {
                    int selectedIndex = list.IndexOf(lvOrdezkariak.SelectedItem.ToString());
                    if (selectedIndex < list.Count - 1) // Comprobar que no estamos en el último elemento
                    {
                        lvOrdezkariak.SelectedItem = list[selectedIndex + 1]; // Seleccionar el elemento siguiente
                    }
                }
                else // Si no hay ningún elemento seleccionado
                {
                    lvOrdezkariak.SelectedItem = list[0]; // Seleccionar el primer elemento
                }
            }
        }
    }
}