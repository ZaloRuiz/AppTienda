using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Producto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaProducto : ContentPage
	{
		public ListaProducto()
		{
			InitializeComponent();
		}
		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarProducto());
		}
		private void ToolbarItemTP_Clicked(object sender, EventArgs e)
		{
			 Navigation.PushAsync(new ListaTipoProducto());
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
			var productos = JsonConvert.DeserializeObject<List<Models.ProductoNombre>>(response);

			listaProd.ItemsSource = productos;
		}
		private async void OnItemSelected(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as Models.ProductoNombre;
			await Navigation.PushAsync(new EditarBorrarProducto(detalles.id_producto, detalles.nombre_producto, detalles.nombre_tipo_producto, detalles.stock,
				detalles.stock_valorado, detalles.promedio, detalles.precio_venta, detalles.producto_alerta));
		}
	}
}