using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Microcharts;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BalanceMensual : ContentPage
	{
		private string _fecha_inicial;
		private string _fecha_final;
		private int _mesQuery;
		private int _yearQuery;
		private decimal _totalC_Fijo = 0;
		private decimal _totalC_Variable = 0;
		private decimal _totalCompras = 0;
		private decimal _totalVentas = 0;
		string BusyReason = "Cargando...";
		public BalanceMensual()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				DateTime fechaMesAct = DateTime.Today;
				_mesQuery = Convert.ToInt32(fechaMesAct.ToString("MM"));
				_yearQuery = Convert.ToInt32(fechaMesAct.ToString("yyyy"));
				txtTitulo.Text = DateTime.Now.ToString("MMMM yyyy").ToUpper();
				DateTime primerDiaMesAct = new DateTime(fechaMesAct.Year, fechaMesAct.Month, 1);
				DateTime ultimoDiaMesAct = primerDiaMesAct.AddMonths(1).AddDays(-1);
				_fecha_inicial = primerDiaMesAct.ToString("yyyy-MM-dd");
				_fecha_final = ultimoDiaMesAct.ToString("yyyy-MM-dd");
				try
				{
					GetIngEgre();
					GetTopVendedores();
					GetTopClientes();
					GetTopProductos();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetIngEgre()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
				try
				{
					Costo_fijo _costoFijo = new Costo_fijo()
					{
						mes_cf = _mesQuery,
						gestion_cf = _yearQuery
					};
					var json = JsonConvert.SerializeObject(_costoFijo);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoFijoQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataCostoFijo = JsonConvert.DeserializeObject<List<Costo_fijo>>(jsonR);
					if (dataCostoFijo != null)
					{
						foreach (var item in dataCostoFijo)
						{
							_totalC_Fijo = _totalC_Fijo + item.monto_cf;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(150);
				try
				{
					Costo_variable _costoVariable = new Costo_variable()
					{
						mes_cv = _mesQuery,
						gestion_cv = _yearQuery
					};
					var json = JsonConvert.SerializeObject(_costoVariable);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoVariableQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataCostoVar = JsonConvert.DeserializeObject<List<Costo_variable>>(jsonR);
					if (dataCostoVar != null)
					{
						foreach (var item in dataCostoVar)
						{
							_totalC_Variable = _totalC_Variable + item.monto_cv;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(150);
				try
				{
					TotalCompra _totCompra = new TotalCompra()
					{
						fecha_inicio = Convert.ToDateTime(_fecha_inicial),
						fecha_final = Convert.ToDateTime(_fecha_final)
					};
					var json = JsonConvert.SerializeObject(_totCompra);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/listaCompraTotalQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataCompras = JsonConvert.DeserializeObject<List<TotalCompra>>(jsonR);
					if (dataCompras != null)
					{
						foreach (var item in dataCompras)
						{
							_totalCompras = _totalCompras + item.total;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(150);
				try
				{
					TotalVenta _totVenta = new TotalVenta()
					{
						fecha_inicio = Convert.ToDateTime(_fecha_inicial),
						fecha_final = Convert.ToDateTime(_fecha_final)
					};
					var json = JsonConvert.SerializeObject(_totVenta);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVentaTotalQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataVentas = JsonConvert.DeserializeObject<List<TotalVenta>>(jsonR);
					if (dataVentas != null)
					{
						foreach (var item in dataVentas)
						{
							_totalVentas = _totalVentas + item.total;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				txtTotalIngresos.Text = _totalVentas.ToString();
				txtTotalEgresos.Text = (_totalCompras + _totalC_Fijo + _totalC_Variable).ToString();
				try
				{
					//Grafico ingresos/egresos
					float _C_Fijo = (float)_totalC_Fijo;
					float _C_Variable = (float)_totalC_Variable;
					float _C_Compras = (float)_totalCompras;
					float _C_Ingresos = (float)_totalVentas;
					txtCostosF.Text = "Costos fijos: " + _totalC_Fijo.ToString() + " Bs.";
					txtCostosV.Text = "Costos variables: " + _totalC_Variable.ToString() + " Bs.";
					txtCompras.Text = "Compras: " + _totalCompras.ToString() + " Bs. ";
					txtIngresos.Text = "Ingresos: " + _totalVentas.ToString() + " Bs.";
					var entries = new[]
					{
						new ChartEntry(_C_Fijo)
						{
							Color = SKColor.Parse("#000FFF"),
						},
						new ChartEntry(_C_Variable)
						{
							Color = SKColor.Parse("#FF0000"),
						},
						new ChartEntry(_C_Compras)
						{
							Color = SKColor.Parse("#f0d400"),
						},
						new ChartEntry(_C_Ingresos)
						{
							Color = SKColor.Parse("#059B00"),
						},
					};
					grafico1.Chart = new DonutChart() { Entries = entries, BackgroundColor = SKColor.Parse("#40616B"), GraphPosition = GraphPosition.AutoFill };
					//grafico costos
					await Task.Delay(500);
					try
					{
						txtCostosF2.Text = "Costos fijos: " + _totalC_Fijo.ToString() + " Bs.";
						txtCostosV2.Text = "Costos variables: " + _totalC_Variable.ToString() + " Bs.";
						var entries2 = new[]
						{
							new ChartEntry(_C_Fijo)
							{
								Color = SKColor.Parse("#000FFF"),
							},
							new ChartEntry(_C_Variable)
							{
								Color = SKColor.Parse("#FF0000"),
							},
						};
						grafico2.Chart = new DonutChart() { Entries = entries2, BackgroundColor = SKColor.Parse("#40616B"), GraphPosition = GraphPosition.AutoFill };
					}
					catch (Exception err)
					{
						await DisplayAlert("Error", err.ToString(), "OK");
					}
					//Tabla mes
					await Task.Delay(300);
					txtTablaVenta.Text = _totalVentas.ToString();
					txtTablaCompra.Text = "- " + _totalCompras.ToString();
					txtTotalVC.Text = (_totalVentas - _totalCompras).ToString();
					txtTablaGF.Text = _totalC_Fijo.ToString();
					txtTablaGV.Text = _totalC_Variable.ToString();
					txtTotalGastos.Text = (_totalC_Fijo + _totalC_Variable).ToString();
					txtTablaTotal.Text = ((_totalVentas - _totalCompras) - (_totalC_Fijo + _totalC_Variable)).ToString();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetTopVendedores()
		{
			int contTop = 0;
			int limiteTop = 3;
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(300);
				try
				{
					VentasPorVendedor _ventaXvend = new VentasPorVendedor()
					{
						fecha_inicio = Convert.ToDateTime(_fecha_inicial),
						fecha_final = Convert.ToDateTime(_fecha_final)
					};
					var json = JsonConvert.SerializeObject(_ventaXvend);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaVentasPorVendedor.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataVentXvende = JsonConvert.DeserializeObject<List<VentasPorVendedor>>(jsonR);

					if (dataVentXvende != null)
					{
						var ordenXped = dataVentXvende.OrderByDescending(x => x.vendedor_count);
						StkVendXCant.Children.Clear();
						foreach (var item in ordenXped)
						{
							contTop = contTop + 1;
							StackLayout _stkCant = new StackLayout();
							_stkCant.Orientation = StackOrientation.Horizontal;
							StkVendXCant.Children.Add(_stkCant);

							Label _lblNombreCant = new Label();
							_lblNombreCant.Text = item.nombre_vendedor;
							_lblNombreCant.FontSize = 16;
							_lblNombreCant.TextColor = Color.White;
							_lblNombreCant.HorizontalTextAlignment = TextAlignment.Start;
							_stkCant.Children.Add(_lblNombreCant);

							Label _lblCant = new Label();
							_lblCant.Text = item.vendedor_count.ToString();
							_lblCant.FontSize = 16;
							_lblCant.TextColor = Color.White;
							_lblCant.HorizontalTextAlignment = TextAlignment.End;
							_lblCant.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkCant.Children.Add(_lblCant);

							if (contTop == limiteTop)
							{
								break;
							}
						}
						var ordenXmont = dataVentXvende.OrderByDescending(x => x.monto_vend);
						StkMontXCant.Children.Clear();
						foreach (var item in ordenXmont)
						{
							contTop = contTop + 1;
							StackLayout _stkMont = new StackLayout();
							_stkMont.Orientation = StackOrientation.Horizontal;
							StkMontXCant.Children.Add(_stkMont);

							Label _lblNombreMont = new Label();
							_lblNombreMont.Text = item.nombre_vendedor;
							_lblNombreMont.FontSize = 16;
							_lblNombreMont.TextColor = Color.White;
							_lblNombreMont.HorizontalTextAlignment = TextAlignment.Start;
							_stkMont.Children.Add(_lblNombreMont);

							Label _lblMonto = new Label();
							_lblMonto.Text = item.monto_vend.ToString() + " Bs.";
							_lblMonto.FontSize = 16;
							_lblMonto.TextColor = Color.White;
							_lblMonto.HorizontalTextAlignment = TextAlignment.End;
							_lblMonto.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkMont.Children.Add(_lblMonto);

							if (contTop == limiteTop)
							{
								break;
							}
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetTopClientes()
		{
			int contTop = 0;
			int limiteTop = 3;
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(400);
				try
				{
					VentasPorCliente _ventaXclien = new VentasPorCliente()
					{
						fecha_inicio = Convert.ToDateTime(_fecha_inicial),
						fecha_final = Convert.ToDateTime(_fecha_final)
					};
					var json = JsonConvert.SerializeObject(_ventaXclien);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaVentasPorCliente.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataVentXclient = JsonConvert.DeserializeObject<List<VentasPorCliente>>(jsonR);
					if (dataVentXclient != null)
					{
						StkPedidXCant.Children.Clear();
						var orderXped = dataVentXclient.OrderByDescending(x => x.cliente_count);
						foreach (var item in orderXped)
						{
							contTop = contTop + 1;
							StackLayout _stkCant = new StackLayout();
							_stkCant.Orientation = StackOrientation.Horizontal;
							StkPedidXCant.Children.Add(_stkCant);

							Label _lblNombreCant = new Label();
							_lblNombreCant.Text = item.nombre_cliente;
							_lblNombreCant.FontSize = 16;
							_lblNombreCant.TextColor = Color.White;
							_lblNombreCant.HorizontalTextAlignment = TextAlignment.Start;
							_stkCant.Children.Add(_lblNombreCant);

							Label _lblCant = new Label();
							_lblCant.Text = item.cliente_count.ToString();
							_lblCant.FontSize = 16;
							_lblCant.TextColor = Color.White;
							_lblCant.HorizontalTextAlignment = TextAlignment.End;
							_lblCant.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkCant.Children.Add(_lblCant);

							if (contTop == limiteTop)
							{
								break;
							}
						}
						var orderXmont = dataVentXclient.OrderByDescending(x => x.monto_vend);
						StkMontXPedid.Children.Clear();
						foreach (var item in orderXmont)
						{
							contTop = contTop + 1;
							StackLayout _stkMont = new StackLayout();
							_stkMont.Orientation = StackOrientation.Horizontal;
							StkMontXPedid.Children.Add(_stkMont);

							Label _lblNombreMont = new Label();
							_lblNombreMont.Text = item.nombre_cliente;
							_lblNombreMont.FontSize = 16;
							_lblNombreMont.TextColor = Color.White;
							_lblNombreMont.HorizontalTextAlignment = TextAlignment.Start;
							_stkMont.Children.Add(_lblNombreMont);

							Label _lblMonto = new Label();
							_lblMonto.Text = item.monto_vend.ToString() + " Bs.";
							_lblMonto.FontSize = 16;
							_lblMonto.TextColor = Color.White;
							_lblMonto.HorizontalTextAlignment = TextAlignment.End;
							_lblMonto.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkMont.Children.Add(_lblMonto);

							if (contTop == limiteTop)
							{
								break;
							}
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetTopProductos()
		{
			int contTop = 0;
			int limiteTop = 5;
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(300);
				try
				{
					VentasPorProducto _ventaXprod = new VentasPorProducto()
					{
						fecha_inicio = Convert.ToDateTime(_fecha_inicial),
						fecha_final = Convert.ToDateTime(_fecha_final)
					};
					var json = JsonConvert.SerializeObject(_ventaXprod);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaVentasPorProducto.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataVentXprod = JsonConvert.DeserializeObject<List<VentasPorProducto>>(jsonR);
					if (dataVentXprod != null)
					{
						StkPedidXCant.Children.Clear();
						var orderXped = dataVentXprod.OrderByDescending(x => x.producto_count);
						foreach (var item in orderXped)
						{
							contTop = contTop + 1;
							StackLayout _stkCant = new StackLayout();
							_stkCant.Orientation = StackOrientation.Horizontal;
							StkPedidXProd.Children.Add(_stkCant);

							Label _lblNombreCant = new Label();
							_lblNombreCant.Text = item.nombre_producto;
							_lblNombreCant.FontSize = 16;
							_lblNombreCant.TextColor = Color.White;
							_lblNombreCant.HorizontalTextAlignment = TextAlignment.Start;
							_stkCant.Children.Add(_lblNombreCant);

							Label _lblCant = new Label();
							_lblCant.Text = item.producto_count.ToString();
							_lblCant.FontSize = 16;
							_lblCant.TextColor = Color.White;
							_lblCant.HorizontalTextAlignment = TextAlignment.End;
							_lblCant.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkCant.Children.Add(_lblCant);

							if (contTop == limiteTop)
							{
								break;
							}
						}
						var orderXmont = dataVentXprod.OrderByDescending(x => x.monto_vend);
						StkMontXPedid.Children.Clear();
						foreach (var item in orderXmont)
						{
							contTop = contTop + 1;
							StackLayout _stkMont = new StackLayout();
							_stkMont.Orientation = StackOrientation.Horizontal;
							StkMontXProd.Children.Add(_stkMont);

							Label _lblNombreMont = new Label();
							_lblNombreMont.Text = item.nombre_producto;
							_lblNombreMont.FontSize = 16;
							_lblNombreMont.TextColor = Color.White;
							_lblNombreMont.HorizontalTextAlignment = TextAlignment.Start;
							_stkMont.Children.Add(_lblNombreMont);

							Label _lblMonto = new Label();
							_lblMonto.Text = item.monto_vend.ToString() + " Bs.";
							_lblMonto.FontSize = 16;
							_lblMonto.TextColor = Color.White;
							_lblMonto.HorizontalTextAlignment = TextAlignment.End;
							_lblMonto.HorizontalOptions = LayoutOptions.EndAndExpand;
							_stkMont.Children.Add(_lblMonto);

							if (contTop == limiteTop)
							{
								break;
							}
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await PopupNavigation.Instance.PopAsync();
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}