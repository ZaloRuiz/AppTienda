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
	public partial class EditarBorrarVendedor : ContentPage
	{
        private int IdVendedor;
        public EditarBorrarVendedor(int id_vendedor, string nombre, int telefono, string direccion, string numero_cuenta, string cedula_identidad,
			string usuario, string password)
		{
			InitializeComponent();
            IdVendedor = id_vendedor;
			nombreEntry.Text = nombre;
			telefonoEntry.Text = telefono.ToString();
			direccionEntry.Text = direccion;
			numero_cuentaEntry.Text = numero_cuenta;
			cedulaEntry.Text = cedula_identidad;
			usuarioEntry.Text = usuario;
			passwordEntry.Text = password;
		}
        private async void BtnEditarVendedor_Clicked(object sender, EventArgs e)
        {
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(nombreEntry.Text) || (!string.IsNullOrEmpty(nombreEntry.Text)))
				{
					if (!string.IsNullOrWhiteSpace(telefonoEntry.Text) || (!string.IsNullOrEmpty(telefonoEntry.Text)))
					{
						if (!string.IsNullOrWhiteSpace(direccionEntry.Text) || (!string.IsNullOrEmpty(direccionEntry.Text)))
						{
							if (!string.IsNullOrWhiteSpace(numero_cuentaEntry.Text) || (!string.IsNullOrEmpty(numero_cuentaEntry.Text)))
							{
								if (!string.IsNullOrWhiteSpace(cedulaEntry.Text) || (!string.IsNullOrEmpty(cedulaEntry.Text)))
								{
									if (!string.IsNullOrWhiteSpace(usuarioEntry.Text) || (!string.IsNullOrEmpty(usuarioEntry.Text)))
									{
										if (!string.IsNullOrWhiteSpace(passwordEntry.Text) || (!string.IsNullOrEmpty(passwordEntry.Text)))
										{
											try
											{
												Vendedores vendedores = new Vendedores()
												{
													id_vendedor = IdVendedor,
													nombre = nombreEntry.Text,
													telefono = Convert.ToInt32(telefonoEntry.Text),
													direccion = direccionEntry.Text,
													numero_cuenta = numero_cuentaEntry.Text,
													cedula_identidad = cedulaEntry.Text,
													usuario = usuarioEntry.Text,
													password = passwordEntry.Text
												};

												var json = JsonConvert.SerializeObject(vendedores);
												var content = new StringContent(json, Encoding.UTF8, "application/json");
												HttpClient client = new HttpClient();
												var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/vendedores/editarVendedor.php", content);

												if (result.StatusCode == HttpStatusCode.OK)
												{
													await DisplayAlert("EDITADO", "Se edito correctamente", "OK");
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
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistorialVendedor(IdVendedor));
        }
    }
}