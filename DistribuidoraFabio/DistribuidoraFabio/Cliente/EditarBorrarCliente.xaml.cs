using Newtonsoft.Json;
using Plugin.Connectivity;
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

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarCliente : ContentPage
	{
        private int IdCliente = 0;
        public EditarBorrarCliente(int id_cliente, int codigo_c, string nombre_cliente, string ubicacion_latitud, string ubicacion_longitud, int telefono, 
            string direccion, string razon_social, int nit)
		{
			InitializeComponent();
            IdCliente = id_cliente;
            codigoEntry.Text = codigo_c.ToString();
			nombreClienteEntry.Text = nombre_cliente;
			ubicacionLatitudEntry.Text = ubicacion_latitud;
			ubicacionLongitudEntry.Text = ubicacion_longitud;
			telefonoClienteEntry.Text = telefono.ToString();
            direccionEntry.Text = direccion;
			nitClienteEntry.Text = nit.ToString();
            razonEntry.Text = razon_social;
		}
        private async void BtnEditarCliente_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(codigoEntry.Text) || (!string.IsNullOrEmpty(codigoEntry.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(nombreClienteEntry.Text) || (!string.IsNullOrEmpty(nombreClienteEntry.Text)))
                    {
                        if (!string.IsNullOrWhiteSpace(telefonoClienteEntry.Text) || (!string.IsNullOrEmpty(telefonoClienteEntry.Text)))
                        {
                            if (!string.IsNullOrWhiteSpace(direccionEntry.Text) || (!string.IsNullOrEmpty(direccionEntry.Text)))
                            {
                                if (!string.IsNullOrWhiteSpace(ubconfirmacionEntry.Text) || (!string.IsNullOrEmpty(ubconfirmacionEntry.Text)))
                                {
                                    if (!string.IsNullOrWhiteSpace(razonEntry.Text) || (!string.IsNullOrEmpty(razonEntry.Text)))
                                    {
                                        if (!string.IsNullOrWhiteSpace(nitClienteEntry.Text) || (!string.IsNullOrEmpty(nitClienteEntry.Text)))
                                        {
                                            var action = await DisplayActionSheet("GUARDAR CAMBIOS?", null, null, "SI", "NO");
                                            switch (action)
                                            {
                                                case "SI":
                                                    try
                                                    {
                                                        Models.Cliente cliente = new Models.Cliente()
                                                        {
                                                            id_cliente = IdCliente,
                                                            codigo_c = Convert.ToInt32(codigoEntry.Text),
                                                            nombre_cliente = nombreClienteEntry.Text,
                                                            ubicacion_latitud = ubicacionLatitudEntry.Text,
                                                            ubicacion_longitud = ubicacionLongitudEntry.Text,
                                                            telefono = Convert.ToInt32(telefonoClienteEntry.Text),
                                                            direccion_cliente = direccionEntry.Text,
                                                            razon_social = razonEntry.Text,
                                                            nit = Convert.ToInt32(nitClienteEntry.Text)
                                                        };

                                                        var json = JsonConvert.SerializeObject(cliente);
                                                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                                                        HttpClient client = new HttpClient();
                                                        var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/clientes/editarCliente.php", content);

                                                        if (result.StatusCode == HttpStatusCode.OK)
                                                        {
                                                            await DisplayAlert("EDITADO", "Se edito correctamente", "OK");
                                                            await Shell.Current.Navigation.PopAsync();
                                                        }
                                                        else
                                                        {
                                                            await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
                                                            await Shell.Current.Navigation.PopAsync();
                                                        }
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
                                                    }
                                                    break;
                                                case "NO":
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            await DisplayAlert("Campo vacio", "El campo de Nit esta vacio", "Ok");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Campo vacio", "El campo de Razon social esta vacio", "Ok");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Campo vacio", "El campo de Ubicacion esta vacio", "Ok");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Campo vacio", "El campo de Direccion esta vacio", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Campo vacio", "El campo de Telefono esta vacio", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Campo vacio", "El campo de Nombre esta vacio", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Campo vacio", "El campo de Codigo esta vacio", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
            }
            
        }
        private async void BtnVerUbicacion_Clicked(object sender, EventArgs e)
        {
            var location = new Location(Convert.ToDouble(ubicacionLatitudEntry.Text), Convert.ToDouble(ubicacionLongitudEntry.Text));
            var options = new MapLaunchOptions { Name = nombreClienteEntry.Text };
            await Map.OpenAsync(location, options);
        }
        private async void BtnObtenerUbicacion_Clicked(object sender, EventArgs e)
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
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
            }
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new HistorialCliente(IdCliente), true);
        }
    }
}