using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace BankuBatenKudeaketa
{
    public partial class MainPage : ContentPage
    {
        private DatuBaseaMetodoak datuBasea;

        public MainPage()
        {
            InitializeComponent();

            // Inicializa la instancia de DatuBaseaMetodoak con el connection string
            datuBasea = new DatuBaseaMetodoak();

            // Llama al método para actualizar el Label con el número de clientes
            ActualizarLabelClienteCount();
            CargarPrimerCliente();
            CargarNANs();
        }

        // Método para obtener el conteo de clientes y actualizar el Label
        private async void ActualizarLabelClienteCount()
        {
            // Llamada asíncrona para evitar bloquear la interfaz de usuario
            int cantidadClientes = await Task.Run(() => datuBasea.LortuBezeroKopurua());

            // Actualiza el texto del Label en la interfaz de usuario
            lblBezeroKopuraua.Text = $"de {cantidadClientes}";
        }

        // Método para cargar la información del primer cliente
        private async void CargarPrimerCliente()
        {
            // Intentar obtener los datos del primer cliente
            var (nan, nombre) = await datuBasea.ObtenerClientePorLineaAsync(1);

            // Si el cliente se encuentra, actualizar los campos
            if (nan != null && nombre != null)
            {
                etyBezeroa.Text = "1";  // Asegurarse de que el Entry muestre el número 1
                etyNAN.Text = nan;
                etyIzena.Text = nombre;
            }
            else
            {
                // Si no se encuentra el cliente, limpiar los campos
                etyNAN.Text = "";
                etyIzena.Text = "";
            }
        }

        // Evento para el botón "Hurrengoa" (suma 1)
        private async void BtnHurrengoa_Clicked(object sender, EventArgs e)
        {
            int cantidadClientes = await Task.Run(() => datuBasea.LortuBezeroKopurua());
            // Verificar que el texto ingresado sea un número entero
            if (int.TryParse(etyBezeroa.Text, out int linea) && linea < cantidadClientes + 1)
            {
                linea++;  // Incrementar el número
                etyBezeroa.Text = linea.ToString();  // Actualizar el Entry

                // Llamar al método para obtener el cliente de la nueva línea
                var (nan, nombre) = await datuBasea.ObtenerClientePorLineaAsync(linea);

                // Si se encuentra el cliente, actualizar los campos
                if (nan != null && nombre != null)
                {
                    etyNAN.Text = nan;
                    etyIzena.Text = nombre;
                }
                else
                {
                    // Si no se encuentra el cliente, limpiar los campos
                    etyNAN.Text = "";
                    etyIzena.Text = "";
                }
            }
        }

        private async void BtnAurrekoa_Clicked(object sender, EventArgs e)
        {
            // Verificar que el texto ingresado sea un número entero
            if (int.TryParse(etyBezeroa.Text, out int linea) && linea > 1)
            {
                linea--;  // Decrementar el número
                etyBezeroa.Text = linea.ToString();  // Actualizar el Entry

                // Llamar al método para obtener el cliente de la nueva línea
                var (nan, nombre) = await datuBasea.ObtenerClientePorLineaAsync(linea);

                // Si se encuentra el cliente, actualizar los campos
                if (nan != null && nombre != null)
                {
                    etyNAN.Text = nan;
                    etyIzena.Text = nombre;
                }
                else
                {
                    // Si no se encuentra el cliente, limpiar los campos
                    etyNAN.Text = "";
                    etyIzena.Text = "";
                }
            }
        }

        private async void BtnGehitu_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  // Obtener NAN del Entry y quitar espacios
            string nombre = etyIzena.Text.Trim();  // Obtener nombre del Entry y quitar espacios

            // Verificar que el NAN y el nombre no estén vacíos
            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Error", "Por favor, ingrese todos los datos.", "OK");
                return;
            }

            // Llamar al método para insertar el nuevo cliente
            bool exito = await datuBasea.InsertarClienteAsync(nan, nombre);

            if (exito)
            {
                await DisplayAlert("Éxito", "Cliente agregado correctamente.", "OK");

                // Opcionalmente, podrías actualizar la interfaz con el nuevo cliente
                ActualizarLabelClienteCount();  // Actualizar el número de clientes
                CargarPrimerCliente();  // Opcional: cargar la información del primer cliente
            }
            else
            {
                await DisplayAlert("Error", "Hubo un problema al agregar el cliente.", "OK");
            }
        }

        private async void BtnKendu_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  // Obtener NAN del Entry y quitar espacios
            string nombre = etyIzena.Text.Trim();  // Obtener nombre del Entry y quitar espacios

            // Verificar que el NAN y el nombre no estén vacíos
            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Error", "Por favor, ingrese todos los datos.", "OK");
                return;
            }

            // Llamar al método para eliminar el cliente
            bool exito = await datuBasea.EliminarClienteAsync(nan, nombre);

            if (exito)
            {
                await DisplayAlert("Éxito", "Cliente eliminado correctamente.", "OK");

                // Opcionalmente, podrías actualizar la interfaz con la nueva cantidad de clientes
                ActualizarLabelClienteCount();  // Actualizar el número de clientes
                CargarPrimerCliente();  // Opcional: cargar la información del primer cliente
            }
            else
            {
                await DisplayAlert("Error", "Hubo un problema al eliminar el cliente.", "OK");
            }
        }

        private async void BtnGorde_Clicked(object sender, EventArgs e)
        {
            string nan = etyNAN.Text.Trim();  // Obtener NAN del Entry y quitar espacios
            string nombre = etyIzena.Text.Trim();  // Obtener nombre del Entry y quitar espacios

            // Verificar que el NAN y el nombre no estén vacíos
            if (string.IsNullOrEmpty(nan) || string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Error", "Por favor, ingrese todos los datos.", "OK");
                return;
            }

            // Llamar al método para actualizar el cliente
            bool exito = await datuBasea.ActualizarClienteAsync(nan, nombre);

            if (exito)
            {
                await DisplayAlert("Éxito", "Cliente actualizado correctamente.", "OK");

                // Opcionalmente, podrías actualizar la interfaz con la nueva cantidad de clientes
                ActualizarLabelClienteCount();  // Actualizar el número de clientes
                CargarPrimerCliente();  // Opcional: cargar la información del primer cliente
            }
            else
            {
                await DisplayAlert("Error", "Hubo un problema al actualizar el cliente.", "OK");
            }
        }





        private async void CargarNANs()
        {
            // Llamar al método para obtener todos los NAN de la base de datos
            List<string> nanList = await datuBasea.ObtenerTodosLosNANAsync();

            // Limpiar el Picker antes de agregar nuevos elementos
            PkNAN.Items.Clear();

            // Verificar si la lista de NAN no está vacía
            if (nanList != null && nanList.Count > 0)
            {
                // Agregar cada NAN al Picker
                foreach (var nan in nanList)
                {
                    PkNAN.Items.Add(nan);  // Añadimos el NAN al Picker
                }
            }
            else
            {
                await DisplayAlert("Error", "No se encontraron clientes.", "OK");
            }
        }

        private async void PkNAN_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar que hay un item seleccionado
            if (PkNAN.SelectedIndex != -1)
            {
                // Obtener el NAN seleccionado
                string selectedNAN = PkNAN.SelectedItem.ToString();

                // Llamar al método para obtener el nombre del cliente por el NAN
                var (nan, nombre) = await datuBasea.ObtenerClientePorNANAsync(selectedNAN);

                // Si el cliente se encuentra, actualizar el campo 'etyIzenaAbizena' con el nombre
                if (nombre != null)
                {
                    EtyIzenaAbizena.Text = nombre;
                }
                else
                {
                    // Si no se encuentra el cliente, mostrar mensaje o dejar el campo vacío
                    EtyIzenaAbizena.Text = "";
                    await DisplayAlert("Error", "Cliente no encontrado.", "OK");
                }

                List<string> cuentas = await datuBasea.ObtenerCuentasPorNANAsync(selectedNAN);

                // Limpiar la lista de cuentas en el ListView
                LvDeskribapenaDepositua.ItemsSource = null;

                // Verificar si se encontraron cuentas
                if (cuentas != null && cuentas.Count > 0)
                {
                    // Asignar la lista de cuentas al ListView
                    LvDeskribapenaDepositua.ItemsSource = cuentas;
                }
                else
                {
                    // Mostrar un mensaje si no se encontraron cuentas
                    await DisplayAlert("No se encontraron cuentas", "Este cliente no tiene cuentas asociadas.", "OK");
                }

                List<string> prestamo = await datuBasea.ObtenerPrestamoPorNANAsync(selectedNAN);

                // Limpiar la lista de cuentas en el ListView
                LvDeskribapenaMailegua.ItemsSource = null;

                // Verificar si se encontraron cuentas
                if (prestamo != null && prestamo.Count > 0)
                {
                    // Asignar la lista de cuentas al ListView
                    LvDeskribapenaMailegua.ItemsSource = prestamo;
                }
                else
                {
                    // Mostrar un mensaje si no se encontraron cuentas
                    await DisplayAlert("No se encontraron prestamos", "Este cliente no tiene prestamos asociados.", "OK");
                }
            }
        }

        private void LvDeskribapenaDepositua_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                // Habilitar el botón BtnAldatuDepositua cuando se selecciona un item
                BtnAldatuDepositua.IsEnabled = true;
            }
            else
            {
                // Deshabilitar el botón BtnAldatuDepositua si no hay selección
                BtnAldatuDepositua.IsEnabled = false;
            }
        }

        private void LvDeskribapenaMailegua_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                // Habilitar el botón BtnMaileguaEzeztatu cuando se selecciona un item
                BtnMaileguaEzeztatu.IsEnabled = true;
            }
            else
            {
                // Deshabilitar el botón BtnMaileguaEzeztatu si no hay selección
                BtnMaileguaEzeztatu.IsEnabled = false;
            }
        }

        private async void BtnAldatuDepositua_Clicked(object sender, EventArgs e)
        {
            if (LvDeskribapenaDepositua.SelectedItem != null)
            {
                string descripcionSeleccionada = LvDeskribapenaDepositua.SelectedItem.ToString();
                decimal? importe = await datuBasea.ObtenerSaldoPorDescripcionAsync(descripcionSeleccionada);

                if (importe.HasValue)
                {
                    await Navigation.PushAsync(new Gordailua(descripcionSeleccionada, importe.Value));
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo obtener el importe del depósito.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Atención", "Por favor, selecciona una cuenta de la lista.", "OK");
            }
        }

        private async void BtnMaileguaEzeztatu_Clicked(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado algo en el Picker
            if (LvDeskribapenaMailegua.SelectedItem != null)
            {
                // Obtener la descripción seleccionada del Picker
                string descripcionSeleccionada = LvDeskribapenaMailegua.SelectedItem.ToString();
                string nanSeleccionada= PkNAN.SelectedItem.ToString();

                // Confirmación de eliminación
                bool confirmacion = await DisplayAlert("Confirmación", "¿Estás seguro de que deseas eliminar este préstamo?", "Sí", "No");

                if (confirmacion)
                {
                    // Llamada al método de eliminación en la base de datos
                    bool exito = await datuBasea.EliminarMaileguaPorDescripcionAsync(descripcionSeleccionada);

                    if (exito)
                    {
                        CargarListaMaileguak(nanSeleccionada);

                        await DisplayAlert("Éxito", "El préstamo ha sido eliminado.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el préstamo de la base de datos.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Atención", "Por favor, selecciona un préstamo del Picker para eliminar.", "OK");
            }
        }

        private async Task CargarListaMaileguak(string pkNan)
        {
            var listaMaileguak = await datuBasea.ObtenerPrestamoPorNANAsync(pkNan);
            LvDeskribapenaMailegua.ItemsSource = listaMaileguak;
        }




        private void BtnIrten_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
