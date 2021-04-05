using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Venta
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MostrarVentaPendiente : ContentPage
	{
		ObservableCollection<DetalleVentaNombre> detalleVenta_lista = new ObservableCollection<DetalleVentaNombre>();
		ObservableCollection<DetalleVenta> _detalleVentaComp = new ObservableCollection<DetalleVenta>();
		public ObservableCollection<DetalleVentaNombre> DetallesVentas { get { return detalleVenta_lista; } }
		private int facturacomp = 0;
		private int _id_venta = 0;
		private DateTime _fecha;
		private int _numero_factura = 0;
		private string _cliente;
		private string _vendedor;
		private string _tipo_venta;
		private decimal _total = 0;
		private string _estado;
		private DateTime _fecha_entrega;
		private string _observacion;
		private decimal _saldo;
		private DateTime _fechaHoy;
		public MostrarVentaPendiente(int id_venta, DateTime fecha, int numero_factura, string nombre_cliente, string nombre_vendedor, string tipo_venta,
			decimal saldo, decimal total, DateTime fecha_entrega, string estado, string observacion)
		{
			InitializeComponent();
			facturacomp = numero_factura;
			_id_venta = id_venta;
			_fecha = fecha;
			_numero_factura = numero_factura;
			_cliente = nombre_cliente;
			_vendedor = nombre_vendedor;
			_tipo_venta = tipo_venta;
			_total = total;
			_estado = estado;
			_fecha_entrega = fecha_entrega;
			_observacion = observacion;
			_fechaHoy = DateTime.Now;
			_saldo = saldo;
			if (_tipo_venta == "Credito")
			{
				_tipo_venta = tipo_venta + " Saldo: " + saldo.ToString("#.##") + " Bs.";
			}
			else if (_tipo_venta == "Contado")
			{
				_tipo_venta = tipo_venta;
			}
			MostraDatosVentaInicial();
		}
		private async void MostraDatosVentaInicial()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					StackLayout stk1 = new StackLayout();
					stk1.Orientation = StackOrientation.Horizontal;
					stkPrimero.Children.Add(stk1);

					Label label1 = new Label();
					label1.Text = "Factura: ";
					label1.FontSize = 18;
					label1.TextColor = Color.FromHex("#465B70");
					label1.WidthRequest = 200;
					stk1.Children.Add(label1);
					Label entfactura = new Label();
					entfactura.Text = _numero_factura.ToString();
					entfactura.FontSize = 18;
					entfactura.TextColor = Color.FromHex("#000000");
					entfactura.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk1.Children.Add(entfactura);

					StackLayout stk2 = new StackLayout();
					stk2.Orientation = StackOrientation.Horizontal;
					stkPrimero.Children.Add(stk2);

					Label label2 = new Label();
					label2.Text = "Fecha: ";
					label2.FontSize = 18;
					label2.TextColor = Color.FromHex("#465B70");
					label2.WidthRequest = 200;
					stk2.Children.Add(label2);
					Label entfecha = new Label();
					entfecha.Text = _fecha.ToString("dd/MM/yyyy");
					entfecha.FontSize = 18;
					entfecha.TextColor = Color.FromHex("#000000");
					entfecha.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk2.Children.Add(entfecha);

					StackLayout stk3 = new StackLayout();
					stk3.Orientation = StackOrientation.Horizontal;
					stkPrimero.Children.Add(stk3);

					Label label3 = new Label();
					label3.Text = "Cliente: ";
					label3.FontSize = 18;
					label3.TextColor = Color.FromHex("#465B70");
					label3.WidthRequest = 200;
					stk3.Children.Add(label3);
					Label entcliente = new Label();
					entcliente.Text = _cliente;
					entcliente.FontSize = 18;
					entcliente.TextColor = Color.FromHex("#000000");
					entcliente.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk3.Children.Add(entcliente);

					StackLayout stk4 = new StackLayout();
					stk4.Orientation = StackOrientation.Horizontal;
					stkPrimero.Children.Add(stk4);

					Label label4 = new Label();
					label4.Text = "Vendedor: ";
					label4.FontSize = 18;
					label4.TextColor = Color.FromHex("#465B70");
					label4.WidthRequest = 200;
					stk4.Children.Add(label4);
					Label entVend = new Label();
					entVend.Text = _vendedor;
					entVend.FontSize = 18;
					entVend.TextColor = Color.FromHex("#000000");
					entVend.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk4.Children.Add(entVend);
					////
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", "Necesitas estar conectado a internet", "OK");
				}
				try
				{
					DetalleVenta _detaVenta = new DetalleVenta()
					{
						factura = _numero_factura
					};
					var json = JsonConvert.SerializeObject(_detaVenta);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaDetalleVentaNombre.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dv_lista = JsonConvert.DeserializeObject<List<DetalleVentaNombre>>(jsonR);
					int numProd = 0;
					await Task.Delay(1000);
					foreach (var item in dv_lista)
					{
						BoxView boxViewI = new BoxView();
						boxViewI.HeightRequest = 1;
						boxViewI.BackgroundColor = Color.FromHex("#465B70");
						stkSegundo.Children.Add(boxViewI);

						numProd = numProd + 1;
						StackLayout stkP1 = new StackLayout();
						stkP1.Orientation = StackOrientation.Horizontal;
						stkSegundo.Children.Add(stkP1);

						Label label1 = new Label();
						label1.Text = "Producto " + numProd.ToString() + ":";
						label1.FontSize = 18;
						label1.TextColor = Color.FromHex("#465B70");
						label1.WidthRequest = 200;
						stkP1.Children.Add(label1);
						Label entNomProd = new Label();
						entNomProd.Text = item.display_text_nombre;
						entNomProd.FontSize = 18;
						entNomProd.TextColor = Color.FromHex("#000000");
						entNomProd.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP1.Children.Add(entNomProd);

						StackLayout stkP2 = new StackLayout();
						stkP2.Orientation = StackOrientation.Horizontal;
						stkSegundo.Children.Add(stkP2);

						Label label2 = new Label();
						label2.Text = "Cantidad:";
						label2.FontSize = 18;
						label2.TextColor = Color.FromHex("#465B70");
						label2.WidthRequest = 200;
						stkP2.Children.Add(label2);
						Label entCant = new Label();
						entCant.Text = item.cantidad.ToString();
						entCant.FontSize = 18;
						entCant.TextColor = Color.FromHex("#000000");
						entCant.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP2.Children.Add(entCant);

						StackLayout stkP3 = new StackLayout();
						stkP3.Orientation = StackOrientation.Horizontal;
						stkSegundo.Children.Add(stkP3);

						Label label3 = new Label();
						label3.Text = "Precio:";
						label3.FontSize = 18;
						label3.TextColor = Color.FromHex("#465B70");
						label3.WidthRequest = 200;
						stkP3.Children.Add(label3);
						Label entPrec = new Label();
						entPrec.Text = item.precio_producto.ToString("#.##") + " Bs.";
						entPrec.FontSize = 18;
						entPrec.TextColor = Color.FromHex("#000000");
						entPrec.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP3.Children.Add(entPrec);

						StackLayout stkP4 = new StackLayout();
						stkP4.Orientation = StackOrientation.Horizontal;
						stkSegundo.Children.Add(stkP4);

						Label label4 = new Label();
						label4.Text = "Subtotal:";
						label4.FontSize = 18;
						label4.TextColor = Color.FromHex("#465B70");
						label4.WidthRequest = 200;
						stkP4.Children.Add(label4);
						Label entdesc = new Label();
						entdesc.Text = item.sub_total.ToString("#.##") + " Bs.";
						entdesc.FontSize = 18;
						entdesc.TextColor = Color.FromHex("#000000");
						entdesc.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP4.Children.Add(entdesc);

						StackLayout stkP5 = new StackLayout();
						stkP5.Orientation = StackOrientation.Horizontal;
						stkSegundo.Children.Add(stkP5);

						Label label5 = new Label();
						label5.Text = "Envases:";
						label5.FontSize = 18;
						label5.TextColor = Color.FromHex("#465B70");
						label5.WidthRequest = 200;
						stkP5.Children.Add(label5);
						Label entenv = new Label();
						entenv.Text = item.envases.ToString();
						entenv.FontSize = 18;
						entenv.TextColor = Color.FromHex("#000000");
						entenv.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP5.Children.Add(entenv);
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", "Necesitas estar conectado a internet", "OK");
				}
				try
				{
					await Task.Delay(1000);
					////Datos finales
					BoxView boxViewI = new BoxView();
					boxViewI.HeightRequest = 1;
					boxViewI.BackgroundColor = Color.FromHex("#465B70");
					stkTercero.Children.Add(boxViewI);

					StackLayout stack1 = new StackLayout();
					stack1.Orientation = StackOrientation.Horizontal;
					stkTercero.Children.Add(stack1);

					Label labelF1 = new Label();
					labelF1.Text = "Tipo de Venta: ";
					labelF1.FontSize = 18;
					labelF1.TextColor = Color.FromHex("#465B70");
					labelF1.WidthRequest = 200;
					stack1.Children.Add(labelF1);
					Label enttipv = new Label();
					enttipv.Text = _tipo_venta;
					enttipv.FontSize = 18;
					enttipv.TextColor = Color.FromHex("#000000");
					enttipv.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack1.Children.Add(enttipv);

					StackLayout stack2 = new StackLayout();
					stack2.Orientation = StackOrientation.Horizontal;
					stkTercero.Children.Add(stack2);

					Label labelF2 = new Label();
					labelF2.Text = "Estado: ";
					labelF2.FontSize = 18;
					labelF2.TextColor = Color.FromHex("#465B70");
					labelF2.WidthRequest = 200;
					stack2.Children.Add(labelF2);
					Label entest = new Label();
					entest.Text = _estado;
					entest.FontSize = 18;
					entest.TextColor = Color.FromHex("#000000");
					entest.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack2.Children.Add(entest);

					StackLayout stack3 = new StackLayout();
					stack3.Orientation = StackOrientation.Horizontal;
					stkTercero.Children.Add(stack3);

					Label labelF3 = new Label();
					labelF3.Text = "Fecha de Entrega: ";
					labelF3.FontSize = 18;
					labelF3.TextColor = Color.FromHex("#465B70");
					labelF3.WidthRequest = 200;
					stack3.Children.Add(labelF3);
					Label entfecent = new Label();
					entfecent.Text = _fecha_entrega.ToString("dd/MM/yyyy");
					entfecent.FontSize = 18;
					entfecent.TextColor = Color.FromHex("#000000");
					entfecent.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack3.Children.Add(entfecent);

					StackLayout stack4 = new StackLayout();
					stack4.Orientation = StackOrientation.Horizontal;
					stkTercero.Children.Add(stack4);

					Label labelF4 = new Label();
					labelF4.Text = "Observaciones: ";
					labelF4.FontSize = 18;
					labelF4.TextColor = Color.FromHex("#465B70");
					labelF4.WidthRequest = 200;
					stack4.Children.Add(labelF4);
					Label entobs = new Label();
					entobs.Text = _observacion;
					entobs.FontSize = 18;
					entobs.TextColor = Color.FromHex("#000000");
					entobs.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack4.Children.Add(entobs);

					StackLayout stack5 = new StackLayout();
					stack5.Orientation = StackOrientation.Horizontal;
					stkTercero.Children.Add(stack5);

					Label labelF5 = new Label();
					labelF5.Text = "Total: ";
					labelF5.FontSize = 18;
					labelF5.TextColor = Color.FromHex("#465B70");
					labelF5.WidthRequest = 200;
					stack5.Children.Add(labelF5);
					Label enttotal = new Label();
					enttotal.Text = _total.ToString("#.##") + " Bs.";
					enttotal.FontSize = 18;
					enttotal.TextColor = Color.FromHex("#000000");
					enttotal.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack5.Children.Add(enttotal);
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", "Necesitas estar conectado a internet", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void ToolbarItemComp_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				string _resultObs = await DisplayPromptAsync("Pedido entregado", "Comentarios:");
				try
				{
					DetalleVenta _detaVenta = new DetalleVenta()
					{
						factura = _numero_factura
					};
					var jsonDV = JsonConvert.SerializeObject(_detaVenta);
					var contentDV = new StringContent(jsonDV, Encoding.UTF8, "application/json");
					HttpClient clientDV = new HttpClient();
					var resultDV = await clientDV.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaDetalleVentaPrevio.php", contentDV);

					var jsonR = await resultDV.Content.ReadAsStringAsync();
					var dv_lista = JsonConvert.DeserializeObject<List<DetalleVenta_previo>>(jsonR);
					foreach (var item in dv_lista)
					{
						Models.Producto producto = new Models.Producto()
						{
							id_producto = item.id_producto,
							stock = item.stock - item.cantidad,
							stock_valorado = item.stock_valorado - (item.cantidad * item.promedio),
							promedio = item.stock_valorado / item.stock
						};
						var json3 = JsonConvert.SerializeObject(producto);
						var content3 = new StringContent(json3, Encoding.UTF8, "application/json");
						HttpClient client3 = new HttpClient();
						var result3 = await client3.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/editarProducto.php", content3);
					}

					Ventas ventas = new Ventas()
					{
						id_venta = _id_venta,
						numero_factura = _numero_factura,
						fecha_entrega = _fechaHoy,
						estado = "Entregado",
						observacion = _resultObs
					};

					var json = JsonConvert.SerializeObject(ventas);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarEstadoVenta.php", content);
					if (result.StatusCode == HttpStatusCode.OK)
					{
						await DisplayAlert("OK", "Se agrego correctamente", "OK");
						await Navigation.PopAsync();
					}
					else
					{
						await PopupNavigation.Instance.PopAsync();
						await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
						await Navigation.PopAsync();
					}
				}
				catch (Exception error)
				{
					await PopupNavigation.Instance.PopAsync();
					await DisplayAlert("ERROR", "Necesitas estar conectado a internet", "OK");
					await Navigation.PopAsync();
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void toolbarCanc_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				string _resultObs = await DisplayPromptAsync("Pedido cancelado", "Comentarios:");
				try
				{
					Ventas ventas = new Ventas()
					{
						id_venta = _id_venta,
						numero_factura = _numero_factura,
						fecha_entrega = _fechaHoy,
						estado = "Cancelado",
						observacion = _resultObs
					};

					var json = JsonConvert.SerializeObject(ventas);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarEstadoVenta.php", content);
					if (result.StatusCode == HttpStatusCode.OK)
					{
						await DisplayAlert("OK", "Se cancelo el producto", "OK");
						await Navigation.PopAsync();
					}
					else
					{
						await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
						await Navigation.PopAsync();
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("ERROR", "Necesitas estar conectado a internet", "OK");
					await Navigation.PopAsync();
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EditarBorrarVenta(_id_venta, _fecha, _numero_factura, _cliente, _vendedor, _tipo_venta, _saldo, _total,
				_fecha_entrega, _estado, _observacion));
		}
	}
}