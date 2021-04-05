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
using Plugin.Connectivity;

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarCliente : ContentPage
	{
		private int _codigo_cliente;
		private int _contador_cliente;
		public AgregarCliente()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/clientes/listaCodigoCliente.php");
					var c_cliente = JsonConvert.DeserializeObject<List<Models.Contador_cliente>>(response);

					foreach(var item in c_cliente)
					{
						_contador_cliente = item.c_cont;
					}
					_codigo_cliente = _contador_cliente + 1;
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void BtnGuardarCliente_Clicked(object sender, EventArgs e)
        {
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(nombreEntry.Text) || (!string.IsNullOrEmpty(nombreEntry.Text)))
				{
					if (!string.IsNullOrWhiteSpace(telefonoEntry.Text) || (!string.IsNullOrEmpty(telefonoEntry.Text)))
					{
						if (!string.IsNullOrWhiteSpace(direccionEntry.Text) || (!string.IsNullOrEmpty(direccionEntry.Text)))
						{
							if (!string.IsNullOrWhiteSpace(ubicacionLatitudEntry.Text) || (!string.IsNullOrEmpty(ubicacionLatitudEntry.Text)))
							{
								if (!string.IsNullOrWhiteSpace(ubicacionLongitudEntry.Text) || (!string.IsNullOrEmpty(ubicacionLongitudEntry.Text)))
								{
									if (!string.IsNullOrWhiteSpace(ubconfirmacionEntry.Text) || (!string.IsNullOrEmpty(ubconfirmacionEntry.Text)))
									{
										if (!string.IsNullOrWhiteSpace(razEntry.Text) || (!string.IsNullOrEmpty(razEntry.Text)))
										{
											if (!string.IsNullOrWhiteSpace(nitEntry.Text) || (!string.IsNullOrEmpty(nitEntry.Text)))
											{
												try
												{
													Models.Cliente cliente = new Models.Cliente()
													{
														codigo_c = _codigo_cliente,
														nombre_cliente = nombreEntry.Text,
														direccion_cliente = direccionEntry.Text,
														ubicacion_latitud = ubicacionLatitudEntry.Text,
														ubicacion_longitud = ubicacionLongitudEntry.Text,
														telefono = Convert.ToInt32(telefonoEntry.Text),
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
														await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
														await Navigation.PopAsync();
													}
												}
												catch (Exception err)
												{
													await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
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
									await DisplayAlert("Campo vacio", "El campo de Ubicacion esta vacio", "Ok");
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
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
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
    }
}