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

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistorialCliente : ContentPage
	{
		private int Id_Cliente;
		List<Ventas> Items;
		public HistorialCliente(int IdCliente)
		{
			InitializeComponent();
            Id_Cliente = IdCliente;
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Items = new List<Ventas>();
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVenta.php");
                var venta = JsonConvert.DeserializeObject<List<Ventas>>(response);
                foreach (var item in venta)
                {
                    if (item.id_cliente == Id_Cliente)
                    {
                        Items.Add(new Ventas
                        {
                            id_venta = item.id_venta,
                            fecha = item.fecha,
                            numero_factura = item.numero_factura,
                            id_cliente = item.id_cliente,
                            id_vendedor = item.id_vendedor,
                            tipo_venta = item.tipo_venta,
                            saldo = item.saldo,
                            total = item.total
                        });
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
            listaClienteH.ItemsSource = Items;
        }

        private async void ListaClienteH_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Ventas;
            //await Navigation.PushAsync(new Venta.MostrarVenta(detalles.id_venta, detalles.fecha, detalles.hora, detalles.numero_factura, detalles.cliente,
            //                                            detalles.vendedor, detalles.tipo_venta, detalles.descuento, detalles.saldo, detalles.total));
        }
    }
}