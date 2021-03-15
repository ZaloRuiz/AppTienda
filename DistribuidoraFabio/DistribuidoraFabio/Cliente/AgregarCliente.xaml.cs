using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DistribuidoraFabio.Models;

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarCliente : ContentPage
	{
		public AgregarCliente()
		{
			InitializeComponent();
		}
        private async void BtnGuardarCliente_Clicked(object sender, EventArgs e)
        {
			try
			{
				Models.Cliente cliente = new Models.Cliente()
				{
					nombre_cliente = nombreEntry.Text,
					ubicacion_latitud = ubicacionLatitudEntry.Text,
					ubicacion_longitud = ubicacionLongitudEntry.Text,
					telefono = Convert.ToInt32(telefonoEntry.Text),
					direccion_cliente = direccionEntry.Text,
					razon_social = razEntry.Text,
					nit = Convert.ToInt32(nitEntry.Text)
				};

				var json = JsonConvert.SerializeObject(cliente);

				var content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpClient client = new HttpClient();

				var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/clientes/agregarCliente.php", content);

				if (result.StatusCode == HttpStatusCode.OK)
				{
					await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
					await Navigation.PopAsync();
				}
				else
				{
					await DisplayAlert("ERROR", result.StatusCode.ToString(), "OK");
					await Navigation.PopAsync();
				}
			}
			catch(Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}

        async void BtnUbicacion_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    ubicacionLatitudEntry.Text = location.Latitude.ToString();
                    ubicacionLongitudEntry.Text = location.Longitude.ToString();
                    ubconfirmacionEntry.Text = "Ubicacion Guardada";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
        }
    }
}