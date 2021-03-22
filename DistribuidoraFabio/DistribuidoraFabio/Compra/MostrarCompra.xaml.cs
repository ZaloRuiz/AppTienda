using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Compra
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MostrarCompra : ContentPage
	{
		ObservableCollection<DetalleCompraNombre> detalleCompra_lista = new ObservableCollection<DetalleCompraNombre>();
		public ObservableCollection<DetalleCompraNombre> DetallesCompras { get { return detalleCompra_lista; } }
       
		private int _id_compra = 0;
		private DateTime _fecha;
		private int _numero_factura = 0;
		private string _proveedor;
		private decimal _saldo = 0;
		private decimal _total = 0;
        string BusyReason = "Cargando...";
        public MostrarCompra(int id_compra, DateTime fecha_compra, int numero_factura, string nombre_proveedor, decimal saldo, decimal total)
		{
			InitializeComponent();
            PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));       
			_numero_factura = numero_factura;           
			_id_compra = id_compra;
			_fecha = fecha_compra;
			_proveedor = nombre_proveedor;
			_saldo = saldo;
			_total = total;
            MostrarDetalleCompra();
            PopupNavigation.Instance.PopAsync();
        }
		private async void MostrarDetalleCompra()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					StackLayout stk1 = new StackLayout();
					stk1.Orientation = StackOrientation.Horizontal;
					stkDatos.Children.Add(stk1);

					Label label1 = new Label();
					label1.Text = "Factura: ";
					label1.FontSize = 23;
					label1.TextColor = Color.FromHex("#4DCCE8");
					label1.WidthRequest = 200;
					stk1.Children.Add(label1);
					Label entfactura = new Label();
					entfactura.Text = _numero_factura.ToString();
					entfactura.FontSize = 23;
					entfactura.TextColor = Color.FromHex("#95B0B7");
					entfactura.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk1.Children.Add(entfactura);

					StackLayout stk2 = new StackLayout();
					stk2.Orientation = StackOrientation.Horizontal;
					stkDatos.Children.Add(stk2);

					Label label2 = new Label();
					label2.Text = "Fecha: ";
					label2.FontSize = 23;
					label2.TextColor = Color.FromHex("#4DCCE8");
					label2.WidthRequest = 200;
					stk2.Children.Add(label2);
					Label entfecha = new Label();
					entfecha.Text = _fecha.ToString("dd/MM/yyyy");
					entfecha.FontSize = 23;
					entfecha.TextColor = Color.FromHex("#95B0B7");
					entfecha.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk2.Children.Add(entfecha);

					StackLayout stk3 = new StackLayout();
					stk3.Orientation = StackOrientation.Horizontal;
					stkDatos.Children.Add(stk3);

					Label label3 = new Label();
					label3.Text = "Proveedor: ";
					label3.FontSize = 23;
					label3.TextColor = Color.FromHex("#4DCCE8");
					label3.WidthRequest = 200;
					stk3.Children.Add(label3);
					Label entcliente = new Label();
					entcliente.Text = _proveedor;
					entcliente.FontSize = 23;
					entcliente.TextColor = Color.FromHex("#95B0B7");
					entcliente.HorizontalOptions = LayoutOptions.FillAndExpand;
					stk3.Children.Add(entcliente);


					////
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", err.ToString(), "OK");
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
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/listaDetalleCompraNombre.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dv_lista = JsonConvert.DeserializeObject<List<DetalleCompraNombre>>(jsonR);
					int numProd = 0;

					foreach (var item in dv_lista)
					{
						BoxView boxViewI = new BoxView();
						boxViewI.HeightRequest = 1;
						boxViewI.BackgroundColor = Color.FromHex("#95B0B7");
						stkPrd.Children.Add(boxViewI);

						numProd = numProd + 1;
						StackLayout stkP1 = new StackLayout();
						stkP1.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP1);

						Label label1 = new Label();
						label1.Text = "Producto " + numProd.ToString() + ":";
						label1.FontSize = 23;
						label1.TextColor = Color.FromHex("#4DCCE8");
						label1.WidthRequest = 200;
						stkP1.Children.Add(label1);
						Label entNomProd = new Label();
						entNomProd.Text = item.display_text_nombre;
						entNomProd.FontSize = 23;
						entNomProd.TextColor = Color.FromHex("#95B0B7");
						entNomProd.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP1.Children.Add(entNomProd);

						StackLayout stkP2 = new StackLayout();
						stkP2.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP2);

						Label label2 = new Label();
						label2.Text = "Cantidad:";
						label2.FontSize = 23;
						label2.TextColor = Color.FromHex("#4DCCE8");
						label2.WidthRequest = 200;
						stkP2.Children.Add(label2);
						Label entCant = new Label();
						entCant.Text = item.cantidad_compra.ToString();
						entCant.FontSize = 23;
						entCant.TextColor = Color.FromHex("#95B0B7");
						entCant.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP2.Children.Add(entCant);

						StackLayout stkP3 = new StackLayout();
						stkP3.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP3);

						Label label3 = new Label();
						label3.Text = "Precio:";
						label3.FontSize = 23;
						label3.TextColor = Color.FromHex("#4DCCE8");
						label3.WidthRequest = 200;
						stkP3.Children.Add(label3);
						Label entPrec = new Label();
						entPrec.Text = item.precio_producto.ToString("#.##") + " Bs.";
						entPrec.FontSize = 23;
						entPrec.TextColor = Color.FromHex("#95B0B7");
						entPrec.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP3.Children.Add(entPrec);

						StackLayout stkP4 = new StackLayout();
						stkP4.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP4);

						Label label4 = new Label();
						label4.Text = "Descuento:";
						label4.FontSize = 23;
						label4.TextColor = Color.FromHex("#4DCCE8");
						label4.WidthRequest = 200;
						stkP4.Children.Add(label4);
						Label entdesc = new Label();
						entdesc.Text = item.descuento_producto.ToString("#.##") + " Bs.";
						entdesc.FontSize = 23;
						entdesc.TextColor = Color.FromHex("#95B0B7");
						entdesc.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP4.Children.Add(entdesc);
						if (item.descuento_producto == 0)
						{
							entdesc.Text = "0 Bs.";
						}
						StackLayout stkP5 = new StackLayout();
						stkP5.Orientation = StackOrientation.Horizontal;
						stkPrd.Children.Add(stkP5);

						Label label5 = new Label();
						label5.Text = "Subtotal:";
						label5.FontSize = 23;
						label5.TextColor = Color.FromHex("#4DCCE8");
						label5.WidthRequest = 200;
						stkP5.Children.Add(label5);
						Label entenv = new Label();
						entenv.Text = item.sub_total.ToString("#.##") + " Bs.";
						entenv.FontSize = 23;
						entenv.TextColor = Color.FromHex("#95B0B7");
						entenv.HorizontalOptions = LayoutOptions.FillAndExpand;
						stkP5.Children.Add(entenv);
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
					boxViewI.BackgroundColor = Color.FromHex("#95B0B7");
					stkFinal.Children.Add(boxViewI);

					StackLayout stack1 = new StackLayout();
					stack1.Orientation = StackOrientation.Horizontal;
					stkFinal.Children.Add(stack1);

					Label labelF1 = new Label();
					labelF1.Text = "Saldo: ";
					labelF1.FontSize = 23;
					labelF1.TextColor = Color.FromHex("#4DCCE8");
					labelF1.WidthRequest = 200;
					stack1.Children.Add(labelF1);
					Label enttipv = new Label();
					enttipv.Text = _saldo.ToString("#.##") + " Bs.";
					enttipv.FontSize = 23;
					enttipv.TextColor = Color.FromHex("#95B0B7");
					enttipv.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack1.Children.Add(enttipv);
					if (_saldo == 0)
					{
						enttipv.Text = "0 Bs.";
					}
					StackLayout stack2 = new StackLayout();
					stack2.Orientation = StackOrientation.Horizontal;
					stkFinal.Children.Add(stack2);

					Label labelF2 = new Label();
					labelF2.Text = "Total: ";
					labelF2.FontSize = 23;
					labelF2.TextColor = Color.FromHex("#4DCCE8");
					labelF2.WidthRequest = 200;
					stack2.Children.Add(labelF2);
					Label entest = new Label();
					entest.Text = _total.ToString("#.##") + " Bs.";
					entest.FontSize = 23;
					entest.TextColor = Color.FromHex("#95B0B7");
					entest.HorizontalOptions = LayoutOptions.FillAndExpand;
					stack2.Children.Add(entest);
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
	}
}