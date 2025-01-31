using Xamarin.Essentials;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArgazkiKamera
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ArgazkiaAtera_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Toma la foto
                var argazkiStream = await KameraView.TakePhotoAsync(Camera.MAUI.ImageFormat.PNG);

                if (argazkiStream != null)
                {
                    // Extrae coordenadas GPS de la imagen
                    var coordinates = await ObtenerCoordenadasGps(argazkiStream);

                    if (coordinates != null)
                    {
                        // Muestra las coordenadas obtenidas
                        await DisplayAlert("Coordenadas GPS", $"Latitud: {coordinates.Value.Latitude}\nLongitud: {coordinates.Value.Longitude}\nAltitud: {coordinates.Value.Altitude}", "Ados");
                    }
                    else
                    {
                        await DisplayAlert("Errorea", "Ez daude GPS koordinatak irudian.", "Ados");
                    }


                }
                else
                {
                    await DisplayAlert("Errorea", "Ezin izan da irudia harrapatu.", "Ados");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Errorea", $"Errore bat gertatu da: {ex.Message}", "Ados");
            }
        }

        private async Task GuardarImagenEnGaleria(Stream argazkiStream, string fileName)
        {
            try
            {
                // Obtén la ruta para guardar la imagen en el almacenamiento local
                var folder = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(folder, fileName);

                // Guarda la imagen en el almacenamiento local
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await argazkiStream.CopyToAsync(fileStream);
                }

                // Verifica si la plataforma es Android y agrega la imagen a la galería
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    var file = new Java.IO.File(filePath);
                    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    var contentUri = Android.Net.Uri.FromFile(file);
                    mediaScanIntent.SetData(contentUri);
                    Android.App.Application.Context.SendBroadcast(mediaScanIntent);
                }

                // Muestra un mensaje indicando que la imagen ha sido guardada
                await DisplayAlert("Éxito", "La imagen se ha guardado en la galería.", "Ados");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un error al guardar la imagen: {ex.Message}", "Ados");
            }
        }



    }
}
