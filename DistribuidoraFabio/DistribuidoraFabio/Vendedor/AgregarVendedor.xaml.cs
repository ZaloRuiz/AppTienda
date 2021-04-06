using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Vendedor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarVendedor : ContentPage
	{
		public AgregarVendedor()
		{
			InitializeComponent();
		}
        private async void BtnGuardarVendedor_Clicked(object sender, EventArgs e)
        {
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(nombreVendedorEntry.Text) || (!string.IsNullOrEmpty(nombreVendedorEntry.Text)))
				{
					if (!string.IsNullOrWhiteSpace(telefonoVendedorEntry.Text) || (!string.IsNullOrEmpty(telefonoVendedorEntry.Text)))
					{
						if (!string.IsNullOrWhiteSpace(direccionVendedorEntry.Text) || (!string.IsNullOrEmpty(direccionVendedorEntry.Text)))
						{
							if (!string.IsNullOrWhiteSpace(numero_cuentaVendedorEntry.Text) || (!string.IsNullOrEmpty(numero_cuentaVendedorEntry.Text)))
							{
								if (!string.IsNullOrWhiteSpace(cedula_identidadVendedorEntry.Text) || (!string.IsNullOrEmpty(cedula_identidadVendedorEntry.Text)))
								{
									if (!string.IsNullOrWhiteSpace(usuarioEntry.Text) || (!string.IsNullOrEmpty(usuarioEntry.Text)))
									{
										if (!string.IsNullOrWhiteSpace(passwordEntry.Text) || (!string.IsNullOrEmpty(passwordEntry.Text)))
										{
											try
											{
												Vendedores vendedores = new Vendedores()
												{
													nombre = nombreVendedorEntry.Text,
													telefono = Convert.ToInt32(telefonoVendedorEntry.Text),
													direccion = direccionVendedorEntry.Text,
													numero_cuenta = numero_cuentaVendedorEntry.Text,
													cedula_identidad = cedula_identidadVendedorEntry.Text,
													usuario = usuarioEntry.Text,
													password = passwordEntry.Text
												};

												var json = JsonConvert.SerializeObject(vendedores);
												var content = new StringContent(json, Encoding.UTF8, "application/json");
												HttpClient client = new HttpClient();
												var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/vendedores/agregarVendedor.php", content);

												if (result.StatusCode == HttpStatusCode.OK)
												{
													await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
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
										}
										else
										{
											await DisplayAlert("Campo vacio", "El campo de Contraseña esta vacio", "Ok");
										}
									}
									else
									{
										await DisplayAlert("Campo vacio", "El campo de Usuario esta vacio", "Ok");
									}
								}
								else
								{
									await DisplayAlert("Campo vacio", "El campo de Cedula de identidad esta vacio", "Ok");
								}
							}
							else
							{
								await DisplayAlert("Campo vacio", "El campo de Numero de cuenta esta vacio", "Ok");
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
    }
}