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

namespace DistribuidoraFabio.Producto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarProducto : ContentPage
	{
        private int IdProd;
        private int IdTipProd;
		public EditarBorrarProducto(int id_producto, string nombre_producto, string nombre_tipo_producto, int stock,
			decimal stock_valorado, decimal promedio, decimal precio_venta, decimal producto_alerta)
		{
			InitializeComponent();
			nombreProdEntry.Text = nombre_producto;
			idTProdEntry.Text = nombre_tipo_producto.ToString();
			stockEntry.Text = stock.ToString();
			stockValoradoEntry.Text = stock_valorado.ToString();
			promedioEntry.Text = promedio.ToString();
			precioventaEntry.Text = precio_venta.ToString();
			alertaEntry.Text = producto_alerta.ToString();
            IdProd = id_producto;
            GetTipProd();
        }
        private async void GetTipProd()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tipoproductos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response);

					foreach (var item in tipoproductos)
					{
						if (idTProdEntry.Text == item.nombre_tipo_producto)
						{
							IdTipProd = item.id_tipoproducto;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
        }
        private async void BtnEditarProd_Clicked(object sender, EventArgs e)
        {
			if (!string.IsNullOrWhiteSpace(nombreProdEntry.Text) || (!string.IsNullOrEmpty(nombreProdEntry.Text)))
			{
				if (!string.IsNullOrWhiteSpace(idTProdEntry.Text) || (!string.IsNullOrEmpty(idTProdEntry.Text)))
				{
					if (!string.IsNullOrWhiteSpace(stockEntry.Text) || (!string.IsNullOrEmpty(stockEntry.Text)))
					{
						if (!string.IsNullOrWhiteSpace(stockValoradoEntry.Text) || (!string.IsNullOrEmpty(stockValoradoEntry.Text)))
						{
							if (!string.IsNullOrWhiteSpace(promedioEntry.Text) || (!string.IsNullOrEmpty(promedioEntry.Text)))
							{
								if (!string.IsNullOrWhiteSpace(precioventaEntry.Text) || (!string.IsNullOrEmpty(precioventaEntry.Text)))
								{
									if (!string.IsNullOrWhiteSpace(alertaEntry.Text) || (!string.IsNullOrEmpty(alertaEntry.Text)))
									{
										try
										{
											Models.Producto producto = new Models.Producto()
											{
												id_producto = IdProd,
												nombre_producto = nombreProdEntry.Text,
												id_tipo_producto = IdTipProd,
												stock = Convert.ToInt32(stockEntry.Text),
												stock_valorado = Convert.ToDecimal(stockValoradoEntry.Text),
												promedio = Convert.ToDecimal(promedioEntry.Text),
												precio_venta = Convert.ToDecimal(precioventaEntry.Text),
												producto_alerta = Convert.ToDecimal(alertaEntry.Text)
											};

											var json = JsonConvert.SerializeObject(producto);
											var content = new StringContent(json, Encoding.UTF8, "application/json");
											HttpClient client = new HttpClient();
											var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/editarProducto2.php", content);

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
										catch (Exception error)
										{
											await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
										}
									}
									else
									{
										await DisplayAlert("Campo vacio", "El campo de Alerta esta vacio", "Ok");
									}
								}
								else
								{
									await DisplayAlert("Campo vacio", "El campo de Precio de venta esta vacio", "Ok");
								}
							}
							else
							{
								await DisplayAlert("Campo vacio", "El campo de Promedio esta vacio", "Ok");
							}
						}
						else
						{
							await DisplayAlert("Campo vacio", "El campo de Stock Valorado esta vacio", "Ok");
						}
					}
					else
					{
						await DisplayAlert("Campo vacio", "El campo de Stock esta vacio", "Ok");
					}
				}
				else
				{
					await DisplayAlert("Campo vacio", "El campo de Tipo de venta esta vacio", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Campo vacio", "El campo de Nombre esta vacio", "Ok");
			}
        }
    }
}