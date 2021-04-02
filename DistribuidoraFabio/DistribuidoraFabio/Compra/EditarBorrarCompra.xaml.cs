using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Compra
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarCompra : ContentPage
	{
		private int _id_compra_edit = 0;
		private DateTime _fecha_edit;
		private int _numero_factura_edit = 0;
		private string _proveedor_edit;
		private decimal _saldo_edit = 0;
		private decimal _total_edit = 0;
		int numProd = 0;
		string BusyReason = "Cargando...";
		public EditarBorrarCompra(int _id_compra, DateTime _fecha, int _numero_factura, string _proveedor, decimal _saldo, decimal _total)
		{
			InitializeComponent();
			_id_compra_edit = _id_compra;
			_fecha_edit = _fecha;
			_numero_factura_edit = _numero_factura;
			_proveedor_edit = _proveedor;
			_saldo_edit = _saldo;
			_total_edit = _total;
			EditarCompra();
		}
		private async void EditarCompra()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					StackLayout stk1 = new StackLayout();
					stk1.Orientation = StackOrientation.Horizontal;
					stkDatos.Children.Add(stk1);

					Entry entfactura = new Entry();
					entfactura.Placeholder = "Factura";
					entfactura.Text = _numero_factura_edit.ToString();
					entfactura.FontSize = 18;
					entfactura.TextColor = Color.FromHex("#465B70");
					entfactura.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk1.Children.Add(entfactura);

					Entry entfecha = new Entry();
					entfecha.Placeholder = "Fecha";
					entfecha.Text = _fecha_edit.ToString("dd/MM/yyyy");
					entfecha.FontSize = 18;
					entfecha.TextColor = Color.FromHex("#4DCCE8");
					entfecha.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk1.Children.Add(entfecha);

					Entry entProv = new Entry();
					entProv.Placeholder = "Proveedor";
					entProv.Text = _proveedor_edit;
					entProv.FontSize = 18;
					entProv.TextColor = Color.FromHex("#465B70");
					entProv.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk1.Children.Add(entProv);
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", err.ToString(), "OK");
				}
				try
				{
					DetalleCompraNombre _detaVenta = new DetalleCompraNombre()
					{
						numero_factura = _numero_factura_edit
					};
					var json = JsonConvert.SerializeObject(_detaVenta);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/listaDetalleCompraNombre.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dc_lista = JsonConvert.DeserializeObject<List<DetalleCompraNombre>>(jsonR);
					int numProd = 0;
					await Task.Delay(1000);
					foreach (var item in dc_lista)
					{
						BoxView boxViewI = new BoxView();
						boxViewI.HeightRequest = 1;
						boxViewI.BackgroundColor = Color.FromHex("#465B70");
						stkPrd.Children.Add(boxViewI);

						numProd = numProd + 1;
						StackLayout stkP1 = new StackLayout();
						stkP1.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP1);

						Entry entNomProd = new Entry();
						entNomProd.Placeholder = "Producto " + numProd.ToString();
						entNomProd.Text = item.nombre_producto;
						entNomProd.FontSize = 18;
						entNomProd.TextColor = Color.FromHex("#465B70");
						entNomProd.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP1.Children.Add(entNomProd);

						Entry entCant = new Entry();
						entCant.Placeholder = "Cantidad";
						entCant.Text = item.cantidad_compra.ToString();
						entCant.FontSize = 18;
						entCant.TextColor = Color.FromHex("#465B70");
						entCant.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP1.Children.Add(entCant);

						StackLayout stkP3 = new StackLayout();
						stkP3.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP3);
						
						Entry entPrec = new Entry();
						entPrec.Placeholder = "Precio";
						entPrec.Text = item.precio_producto.ToString("#.##") + " Bs.";
						entPrec.FontSize = 18;
						entPrec.TextColor = Color.FromHex("#465B70");
						entPrec.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP3.Children.Add(entPrec);

						Entry entdesc = new Entry();
						entdesc.Placeholder = "Descuento";
						entdesc.Text = item.descuento_producto.ToString("#.##") + " Bs.";
						entdesc.FontSize = 18;
						entdesc.TextColor = Color.FromHex("#465B70");
						entdesc.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP3.Children.Add(entdesc);
						if (item.descuento_producto == 0)
						{
							entdesc.Text = "0 Bs.";
						}
						
						Entry entSubT = new Entry();
						entSubT.Placeholder = "Sub Total";
						entSubT.Text = item.sub_total.ToString("#.##") + " Bs.";
						entSubT.FontSize = 18;
						entSubT.TextColor = Color.FromHex("#465B70");
						entSubT.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP3.Children.Add(entSubT);
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", err.ToString(), "OK");
				}
				try
				{
					await Task.Delay(1000);
					////Datos finales
					BoxView boxViewI = new BoxView();
					boxViewI.HeightRequest = 1;
					boxViewI.BackgroundColor = Color.FromHex("#465B70");
					stkFinal.Children.Add(boxViewI);

					StackLayout stack1 = new StackLayout();
					stack1.Orientation = StackOrientation.Horizontal;
					stkFinal.Children.Add(stack1);

					Entry entSaldo = new Entry();
					entSaldo.Placeholder = "Saldo";
					entSaldo.Text = _saldo_edit.ToString("#.##") + " Bs.";
					entSaldo.FontSize = 18;
					entSaldo.TextColor = Color.FromHex("#465B70");
					entSaldo.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack1.Children.Add(entSaldo);
					if (_saldo_edit == 0)
					{
						entSaldo.Text = "0 Bs.";
					}

					Entry entTotal = new Entry();
					entTotal.Placeholder = "Total";
					entTotal.Text = _total_edit.ToString("#.##") + " Bs.";
					entTotal.FontSize = 18;
					entTotal.TextColor = Color.FromHex("#465B70");
					entTotal.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack1.Children.Add(entTotal);
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", err.ToString(), "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}