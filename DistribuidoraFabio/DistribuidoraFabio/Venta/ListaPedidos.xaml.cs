using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
	public partial class ListaPedidos : TabbedPage
	{
		ObservableCollection<VentasNombre> _listaPedidosEnt = new ObservableCollection<VentasNombre>();
		ObservableCollection<VentasNombre> _listaPedidosPen = new ObservableCollection<VentasNombre>();
		ObservableCollection<VentasNombre> _listaPedidosCanc = new ObservableCollection<VentasNombre>();
		List<string> list_C_P = new List<string>();
		List<string> list_C_E = new List<string>();
		List<string> list_C_C = new List<string>();
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
				string BusyReason = "Cargando...";
				await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
				_listaPedidosEnt.Clear();
				_listaPedidosPen.Clear();
				_listaPedidosCanc.Clear();
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVentaNombre.php");
					var lista_ventas = JsonConvert.DeserializeObject<List<VentasNombre>>(response);

					var listVent = lista_ventas.OrderByDescending(x => x.id_venta);

					foreach (var item in listVent)
					{
						if (item.estado == "Entregado")
						{
							_listaPedidosEnt.Add(item);
						}
						else if (item.estado == "Pendiente")
						{
							_listaPedidosPen.Add(item);
						}
						else if (item.estado == "Cancelado")
						{
							_listaPedidosCanc.Add(item);
						}
					}
					listaEntregados.ItemsSource = _listaPedidosEnt;
					listaPendientes.ItemsSource = _listaPedidosPen;
					listaCancelados.ItemsSource = _listaPedidosCanc;
					await PopupNavigation.Instance.PopAsync();
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
		private async void toolbarBuscar_Clicked(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in _listaPedidosPen)
				{
					list_C_P.Add(item.nombre_cliente);
				}
				IEnumerable<string> array_C = list_C_P.Distinct<string>();
				string _c_elegido = await DisplayActionSheet("Elija un cliente", null, null, array_C.ToArray());
				if (_c_elegido != null)
				{
					listaPendientes.ItemsSource = _listaPedidosPen.Where(x => x.nombre_cliente.ToLower().Contains(_c_elegido.ToLower()));
				}
				else
				{
					listaPendientes.ItemsSource = _listaPedidosPen;
				}
			}
			catch (Exception err)
			{

			}
		}
		private async void toolbarBuscarEnt_Clicked(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in _listaPedidosEnt)
				{
					list_C_E.Add(item.nombre_cliente);
				}
				IEnumerable<string> array_C = list_C_E.Distinct<string>();
				string _c_elegido = await DisplayActionSheet("Elija un cliente", null, null, array_C.ToArray());
				if (_c_elegido != null)
				{
					listaEntregados.ItemsSource = _listaPedidosEnt.Where(x => x.nombre_cliente.ToLower().Contains(_c_elegido.ToLower()));
				}
				else
				{
					listaEntregados.ItemsSource = _listaPedidosEnt;
				}
			}
			catch (Exception err)
			{

			}
		}
		private async void toolbarBuscarCanc_Clicked(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in _listaPedidosCanc)
				{
					list_C_C.Add(item.nombre_cliente);
				}
				IEnumerable<string> array_C = list_C_C.Distinct<string>();
				string _c_elegido = await DisplayActionSheet("Elija un cliente", null, null, array_C.ToArray());
				if (_c_elegido != null)
				{
					listaCancelados.ItemsSource = _listaPedidosCanc.Where(x => x.nombre_cliente.ToLower().Contains(_c_elegido.ToLower()));
				}
				else
				{
					listaCancelados.ItemsSource = _listaPedidosCanc;
				}
			}
			catch (Exception err)
			{

			}
		}
	}
}