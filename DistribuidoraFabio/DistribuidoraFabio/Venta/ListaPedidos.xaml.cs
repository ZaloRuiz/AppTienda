using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
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
			
		}
        protected async override void OnAppearing()
        {
			base.OnAppearing();
			this.BindingContext = new ListaPedidosVM();
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