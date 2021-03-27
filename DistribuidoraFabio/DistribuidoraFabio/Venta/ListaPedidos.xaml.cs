using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Venta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaPedidos : TabbedPage
	{
		List<Ventas> _Entregado = new List<Ventas>();
		List<Ventas> _Pendiente = new List<Ventas>();
		List<Ventas> _Cancelado = new List<Ventas>();
		public ListaPedidos()
		{
			InitializeComponent();
			AvisoModificar();
		}
        protected async override void OnAppearing()
        {
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					this.BindingContext = new ListaPedidosVM();
					await PopupNavigation.Instance.PopAsync();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void AvisoModificar()
		{
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaEditarVentaPendiente.php");
			var lista_e_v = JsonConvert.DeserializeObject<List<Editar_Venta>>(response);
			if(lista_e_v.Count != 0)
			{
				ToolbarItem _item_aviso = new ToolbarItem
				{
					Text = "Editar Venta",
					IconImageSource = ImageSource.FromFile("icon_advertencia.png"),
					Order = ToolbarItemOrder.Primary,
					Priority = 0
				};
				this.ToolbarItems.Add(_item_aviso);
				_item_aviso.Clicked += OnItemAvisoClicked;
			}
			async void OnItemAvisoClicked(object sender, EventArgs e)
			{
				ToolbarItem item = (ToolbarItem)sender;
				var actionSheet = await DisplayActionSheet("Aviso", "Ir a modificar", "Cancelar", "Existen ventas por modificar");

				switch (actionSheet)
				{
					case "Ir a modificar":

						// Do Something when 'Cancel' Button is pressed
						await Navigation.PushAsync(new ListaVentaAEditar());
						break;

					case "Cancelar":

						// Do Something when 'Destruction' Button is pressed
						
						break;

					case "Existen ventas por modificar":

						// Do Something when 'Button1' Button is pressed
						await Navigation.PushAsync(new ListaVentaAEditar());
						break;
				}
			}
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarVenta());
		}
		private void ToolbarItemP_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarVenta());
		}
		private void ToolbarItemC_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarVenta());
		}
		private async void OnItemSelectedE(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as VentasNombre;
			await Navigation.PushAsync(new MostrarVenta(detalles.id_venta, detalles.fecha, detalles.numero_factura, detalles.nombre_cliente,
														detalles.nombre_vendedor, detalles.tipo_venta, detalles.saldo, detalles.total, detalles.fecha_entrega,
														detalles.estado, detalles.observacion));
		}
		private async void OnItemSelectedP(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as VentasNombre;
			await Navigation.PushAsync(new MostrarVentaPendiente(detalles.id_venta, detalles.fecha, detalles.numero_factura, detalles.nombre_cliente,
														detalles.nombre_vendedor, detalles.tipo_venta, detalles.saldo, detalles.total, detalles.fecha_entrega,
														detalles.estado, detalles.observacion));
		}
		private async void OnItemSelectedC(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as VentasNombre;
			await Navigation.PushAsync(new MostrarVenta(detalles.id_venta, detalles.fecha, detalles.numero_factura, detalles.nombre_cliente,
														detalles.nombre_vendedor, detalles.tipo_venta, detalles.saldo, detalles.total, detalles.fecha_entrega,
														detalles.estado, detalles.observacion));
		}
	}
}