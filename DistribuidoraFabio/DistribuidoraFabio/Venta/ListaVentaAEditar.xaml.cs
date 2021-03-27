using DistribuidoraFabio.Models;
using Newtonsoft.Json;
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
	public partial class ListaVentaAEditar : ContentPage
	{
		private int _id_venta;
		private DateTime _fecha;
		private int _numero_factura;
		private string _cliente;
		private string _nombre_vendedor;
		private string _tipo_venta;
		private decimal _saldo;
		private decimal _total;
		private DateTime _fecha_entrega;
		private string _estado;
		private string _observacion;
		public ListaVentaAEditar()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaEditarVentaPendiente.php");
			var lista_e_v = JsonConvert.DeserializeObject<List<Editar_Venta>>(response);
			listaAEditar.ItemsSource = lista_e_v;
		}
		private async void listaAEditar_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as Editar_Venta;

			Ventas _Venta = new Ventas()
			{
				id_venta = detalles.id_venta
			};
			var json = JsonConvert.SerializeObject(_Venta);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpClient client = new HttpClient();
			var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVentaBuscar.php", content);

			var jsonR = await result.Content.ReadAsStringAsync();
			var v_lista = JsonConvert.DeserializeObject<List<VentasNombre>>(jsonR);

			foreach(var item in v_lista)
			{
				if(item.id_venta == detalles.id_venta)
				{
					_id_venta = item.id_venta;
					_fecha = item.fecha;
					_numero_factura = item.numero_factura;
					_cliente = item.nombre_cliente;
					_nombre_vendedor = item.nombre_vendedor;
					_tipo_venta = item.tipo_venta;
					_saldo = item.saldo;
					_total = item.total;
					_fecha_entrega = item.fecha_entrega;
					_estado = item.estado;
					_observacion = item.observacion;
				}
			}
			await Navigation.PushAsync(new EditarBorrarVenta(_id_venta, _fecha, _numero_factura, _cliente, _nombre_vendedor, _tipo_venta, _saldo,
				_total, _fecha_entrega, _estado, _observacion));
		}
	}
}