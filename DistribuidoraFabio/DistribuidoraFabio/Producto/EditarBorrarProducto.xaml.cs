using Newtonsoft.Json;
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
		public EditarBorrarProducto(int id_producto, string nombre_producto, string nombre_tipo_producto, decimal stock,
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
		}
        private async void BtnEditarProd_Clicked(object sender, EventArgs e)
        {
            Models.Producto producto = new Models.Producto()
            {
                id_producto = IdProd,
                nombre_producto = nombreProdEntry.Text,
                id_tipo_producto = Convert.ToInt32(idTProdEntry.Text),
                stock = Convert.ToDecimal(stockEntry.Text),
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
                await DisplayAlert("ERROR", result.StatusCode.ToString(), "OK");
                await Navigation.PopAsync();
            }
        }

        private async void BtnBorrarProd_Clicked(object sender, EventArgs e)
        {
            Models.Producto producto = new Models.Producto()
            {
                id_producto = IdProd,
            };

            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/borrarProducto.php", content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("ELIMINADO", "Se elimino correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("ERROR", result.StatusCode.ToString(), "OK");
                await Navigation.PopAsync();
            }
        }
    }
}