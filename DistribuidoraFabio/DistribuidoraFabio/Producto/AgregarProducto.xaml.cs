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
	public partial class AgregarProducto : ContentPage
	{
        List<Tipo_producto> TP_prods = new List<Tipo_producto>();
        public AgregarProducto()
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
                    var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
                    var tipoproductos = JsonConvert.DeserializeObject<List<Models.Tipo_producto>>(response);

                    foreach (var item in tipoproductos)
                    {
                        TP_prods.Add(item);
                    }
                    tpPicker.ItemsSource = TP_prods;
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
        private string pickTP;
        private int pickedID_TP;
        private async void TpPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                pickTP = picker.Items[selectedIndex];
            }
            try
			{
                foreach(var item in TP_prods)
				{
                    if(pickTP == item.nombre_tipo_producto)
					{
                        pickedID_TP = item.id_tipoproducto;
					}
				}
			}
            catch(Exception err)
			{
                await DisplayAlert("Error", err.ToString(), "OK");
			}
        }
        private void stockProductoEntry_Completed(object sender, EventArgs e)
        {
            stockValoradoProductoEntry.Text = (Convert.ToDecimal(stockProductoEntry.Text)*Convert.ToDecimal(precioventaEntry.Text)).ToString();
        }
        private async void BtnGuardarPr_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(pickedID_TP.ToString()) || (!string.IsNullOrEmpty(pickedID_TP.ToString())))
                {
                    if (!string.IsNullOrWhiteSpace(nombrePEntry.Text) || (!string.IsNullOrEmpty(nombrePEntry.Text)))
                    {
                        if (!string.IsNullOrWhiteSpace(precioventaEntry.Text) || (!string.IsNullOrEmpty(precioventaEntry.Text)))
                        {
                            if (!string.IsNullOrWhiteSpace(stockProductoEntry.Text) || (!string.IsNullOrEmpty(stockProductoEntry.Text)))
                            {
                                if (!string.IsNullOrWhiteSpace(stockValoradoProductoEntry.Text) || (!string.IsNullOrEmpty(stockValoradoProductoEntry.Text)))
                                {
                                    if (!string.IsNullOrWhiteSpace(promedioProductoEntry.Text) || (!string.IsNullOrEmpty(promedioProductoEntry.Text)))
                                    {
                                        if (!string.IsNullOrWhiteSpace(alertaProductoEntry.Text) || (!string.IsNullOrEmpty(alertaProductoEntry.Text)))
                                        {
                                            try
                                            {
                                                Models.Producto producto = new Models.Producto()
                                                {
                                                    nombre_producto = nombrePEntry.Text,
                                                    id_tipo_producto = pickedID_TP,
                                                    stock = Convert.ToInt32(stockProductoEntry.Text),
                                                    stock_valorado = Convert.ToDecimal(stockValoradoProductoEntry.Text),
                                                    promedio = Convert.ToDecimal(promedioProductoEntry.Text),
                                                    precio_venta = Convert.ToDecimal(precioventaEntry.Text),
                                                    producto_alerta = Convert.ToDecimal(alertaProductoEntry.Text)
                                                };

                                                var json = JsonConvert.SerializeObject(producto);
                                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                                HttpClient client = new HttpClient();
                                                var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/agregarProducto.php", content);

                                                if (result.StatusCode == HttpStatusCode.OK)
                                                {
                                                    await DisplayAlert("Guardado", "Se agrego correctamente", "OK");
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
                            await DisplayAlert("Campo vacio", "El campo de Precio de venta esta vacio", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Campo vacio", "El campo de Nombre esta vacio", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Campo vacio", "El campo de Tipo de producto esta vacio", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
            }
        }
	}
}