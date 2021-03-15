using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapaCliente : ContentPage
	{
		public MapaCliente()
		{
			InitializeComponent();
			MostrarMapaAsync();
			Device.BeginInvokeOnMainThread(() => AskPermission());
		}
        private async void MostrarMapaAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/clientes/listaCliente.php");
                        var clientes = JsonConvert.DeserializeObject<List<Models.Cliente>>(response);

                        var location = await Geolocation.GetLastKnownLocationAsync();

                        var map = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude),
                        Distance.FromKilometers(8)))
                        {
                            IsShowingUser = true,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                        };

                        foreach (var item in clientes)
                        {
                            double latitud = Convert.ToDouble(item.ubicacion_latitud);
                            double longitud = Convert.ToDouble(item.ubicacion_longitud);
                            var position = new Position(latitud, longitud);

                            var pin = new Pin
                            {
                                Type = PinType.Place,
                                Position = position,
                                Label = item.nombre_cliente,
                                Address = item.telefono.ToString(),
                            };

                            map.Pins.Add(pin);
                        }
                        Content = map;
                    }

                    catch (Exception err)
                    {
                        await DisplayAlert("Location Denied", err.ToString(), "OK");
                    }
                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Location Denied", ex.ToString(), "OK");
            }
        }

        async void AskPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Request", "This app needs to access device location. Please allow access for location.", "Ok");
                try
                {
                    await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("ERROR", ex.ToString(), "OK");
                    //Location.Text = "Error: " + ex;
                }
            }
        }
    }
}