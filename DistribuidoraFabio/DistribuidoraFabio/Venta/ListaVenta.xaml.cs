using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Venta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaVenta : ContentPage
	{
		public ListaVenta()
		{
			InitializeComponent();
		}
		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarVenta());
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			string BusyReason = "Cargando...";
			await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVentaNombre.php");
			var ventas = JsonConvert.DeserializeObject<List<VentasNombre>>(response);
			listaVenta.ItemsSource = ventas;
			await PopupNavigation.Instance.PopAsync();
		}
		private async void OnItemSelected(object sender, ItemTappedEventArgs e)
		{
			//var detalles = e.Item as Ventas;
			//await Navigation.PushAsync(new MostrarVenta(detalles.id_venta, detalles.fecha, detalles.numero_factura, detalles.id_cliente,
			//											detalles.id_vendedor, detalles.tipo_venta, detalles.saldo, detalles.total));
		}
	}
}