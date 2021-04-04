using DistribuidoraFabio.Models;
using Microcharts;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
		private decimal _costo_fijo;
		private decimal _costo_variable;
		private decimal _vend1 = 2457;
		private decimal _vend2 = 4457;
		private decimal _vend3 = 6457;
		private decimal _vend4 = 8457;
		private decimal _cliente1 = 14287;
		private decimal _cliente2 = 12287;
		private decimal _cliente3 = 11287;
		private decimal _cliente4 = 10287;
		private int _producto1 = 140;
		private int _producto2 = 122;
		private int _producto3 = 104;
		private int _producto4 = 94;
		private int _p_inventario1 = 166;
		private int _p_inventario2 = 152;
		private int _p_inventario3 = 144;
		private int _p_inventario4 = 106;
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
					await DisplayAlert("Error", err.ToString(), "OK");
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
					if(dataCostoFijo != null)
					{
						foreach (var item in dataCostoFijo)
						{
							_totalC_Fijo = _totalC_Fijo + item.monto_cf;
						}
					}
				}
				catch(Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
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
						foreach(var item in dataCostoVar)
						{
							_totalC_Variable = _totalC_Variable + item.monto_cv;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
				await Task.Delay(1000);
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
					await DisplayAlert("Error", err.ToString(), "OK");
				}
				await Task.Delay(1000);
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
					await DisplayAlert("Error", err.ToString(), "OK");
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
					await Task.Delay(1000);
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
					await Task.Delay(500);
					txtTablaVenta.Text = _totalVentas.ToString();
					txtTablaCompra.Text = "- "+_totalCompras.ToString();
					txtTotalVC.Text = (_totalVentas - _totalCompras).ToString();
					txtTablaGF.Text = _totalC_Fijo.ToString();
					txtTablaGV.Text = _totalC_Variable.ToString();
					txtTotalGastos.Text = (_totalC_Fijo + _totalC_Variable).ToString();
					txtTablaTotal.Text = ((_totalVentas - _totalCompras) - (_totalC_Fijo + _totalC_Variable)).ToString();
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
		private async void GetTopVendedores()
		{
			int contTop = 0;
			int limiteTop = 3;
			if (CrossConnectivity.Current.IsConnected)
			{
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

							if(contTop == limiteTop)
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
					await DisplayAlert("Error", err.ToString(), "OK");
				}
				//Grafico Top Vendedores
				try
				{
					//float _top_vendedor1 = (float)_vend1;
					//float _top_vendedor2 = (float)_vend2;
					//float _top_vendedor3 = (float)_vend3;
					//float _top_vendedor4 = (float)_vend4;
					//var entriesVendedores = new[]
					//{
					//	new ChartEntry(_top_vendedor1)
					//	{
					//		Color = SKColors.OrangeRed,
					//		TextColor = SKColors.OrangeRed,
					//		Label = "Vendedor 1",
					//		ValueLabel = _top_vendedor1.ToString(),
					//		ValueLabelColor = SKColors.OrangeRed
					//	},
					//	new ChartEntry(_top_vendedor2)
					//	{
					//		Color = SKColors.Yellow,
					//		TextColor = SKColors.Yellow,
					//		Label = "Vendedor 2",
					//		ValueLabel = _top_vendedor2.ToString(),
					//		ValueLabelColor = SKColors.Yellow
					//	},
					//	new ChartEntry(_top_vendedor3)
					//	{
					//		Color = SKColors.DarkSeaGreen,
					//		TextColor = SKColors.DarkSeaGreen,
					//		Label = "Vendedor 3",
					//		ValueLabel = _top_vendedor3.ToString(),
					//		ValueLabelColor = SKColors.DarkSeaGreen
					//	},
					//	new ChartEntry(_top_vendedor4)
					//	{
					//		Color = SKColors.BlueViolet,
					//		TextColor = SKColors.BlueViolet,
					//		Label = "Vendedor 4",
					//		ValueLabel = _top_vendedor4.ToString(),
					//		ValueLabelColor = SKColors.BlueViolet
					//	},
					//};
					//grafVendedores.Chart = new BarChart()
					//{
					//	Entries = entriesVendedores,
					//	BackgroundColor = SKColor.Parse("#40616B"),
					//	LabelTextSize = 30,
					//	LabelOrientation = Orientation.Horizontal,
					//	ValueLabelOrientation = Orientation.Horizontal,
					//	LabelColor = SKColors.White,
					//};
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
		private async void GetTopClientes()
		{
			int contTop = 0;
			int limiteTop = 3;
			if (CrossConnectivity.Current.IsConnected)
			{
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
						foreach(var item in orderXmont)
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
					//		float _top_cliente1 = (float)_cliente1;
					//		float _top_cliente2 = (float)_cliente2;
					//		float _top_cliente3 = (float)_cliente3;
					//		float _top_cliente4 = (float)_cliente4;
					//		var entriesCliente = new[]
					//		{
					//	new ChartEntry(_top_cliente1)
					//	{
					//		Color = SKColors.OrangeRed,
					//		TextColor = SKColors.OrangeRed,
					//		Label = "Cliente 1",
					//		ValueLabel = _top_cliente1.ToString(),
					//		ValueLabelColor = SKColors.OrangeRed
					//	},
					//	new ChartEntry(_top_cliente2)
					//	{
					//		Color = SKColors.Yellow,
					//		TextColor = SKColors.Yellow,
					//		Label = "Cliente 2",
					//		ValueLabel = _top_cliente2.ToString(),
					//		ValueLabelColor = SKColors.Yellow
					//	},
					//	new ChartEntry(_top_cliente3)
					//	{
					//		Color = SKColors.DarkSeaGreen,
					//		TextColor = SKColors.DarkSeaGreen,
					//		Label = "Cliente 3",
					//		ValueLabel = _top_cliente3.ToString(),
					//		ValueLabelColor = SKColors.DarkSeaGreen
					//	},
					//	new ChartEntry(_top_cliente4)
					//	{
					//		Color = SKColors.BlueViolet,
					//		TextColor = SKColors.BlueViolet,
					//		Label = "Cliente 4",
					//		ValueLabel = _top_cliente4.ToString(),
					//		ValueLabelColor = SKColors.BlueViolet
					//	},
					//};
					//		grafClientes.Chart = new BarChart()
					//		{
					//			Entries = entriesCliente,
					//			BackgroundColor = SKColor.Parse("#40616B"),
					//			LabelTextSize = 30,
					//			LabelOrientation = Orientation.Horizontal,
					//			ValueLabelOrientation = Orientation.Horizontal,
					//			LabelColor = SKColors.White,
					//		};
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
		private async void GetTopProductos()
		{
			int contTop = 0;
			int limiteTop = 5;
			if (CrossConnectivity.Current.IsConnected)
			{
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
					//grafico productos mas vendidos
					//		float _top_producto1 = (float)_producto1;
					//		float _top_producto2 = (float)_producto2;
					//		float _top_producto3 = (float)_producto3;
					//		float _top_producto4 = (float)_producto4;
					//		var entriesProductos = new[]
					//		{
					//	new ChartEntry(_top_producto1)
					//	{
					//		Color = SKColors.DarkOrange,
					//		TextColor = SKColors.DarkOrange,
					//		Label = "Producto 1",
					//		ValueLabel = _top_producto1.ToString(),
					//		ValueLabelColor = SKColors.DarkOrange
					//	},
					//	new ChartEntry(_top_producto2)
					//	{
					//		Color = SKColors.DarkCyan,
					//		TextColor = SKColors.DarkCyan,
					//		Label = "Producto 2",
					//		ValueLabel = _top_producto2.ToString(),
					//		ValueLabelColor = SKColors.DarkCyan
					//	},
					//	new ChartEntry(_top_producto3)
					//	{
					//		Color = SKColors.MediumPurple,
					//		TextColor = SKColors.MediumPurple,
					//		Label = "Producto 3",
					//		ValueLabel = _top_producto3.ToString(),
					//		ValueLabelColor = SKColors.MediumPurple
					//	},
					//	new ChartEntry(_top_producto4)
					//	{
					//		Color = SKColors.ForestGreen,
					//		TextColor = SKColors.ForestGreen,
					//		Label = "Producto 4",
					//		ValueLabel = _top_producto4.ToString(),
					//		ValueLabelColor = SKColors.ForestGreen
					//	},
					//};
					//		grafP_MasVendidos.Chart = new BarChart()
					//		{
					//			Entries = entriesProductos,
					//			BackgroundColor = SKColor.Parse("#40616B"),
					//			LabelTextSize = 30,
					//			LabelOrientation = Orientation.Horizontal,
					//			ValueLabelOrientation = Orientation.Horizontal,
					//			LabelColor = SKColors.White,
					//		};

					//		//Grafico productos en almacen
					//		float _top_invent1 = (float)_p_inventario1;
					//		float _top_invent2 = (float)_p_inventario2;
					//		float _top_invent3 = (float)_p_inventario3;
					//		float _top_invent4 = (float)_p_inventario4;
					//		var entriesInventario = new[]
					//		{
					//	new ChartEntry(_top_invent1)
					//	{
					//		Color = SKColors.LawnGreen,
					//		TextColor = SKColors.LawnGreen,
					//		Label = "Producto 1",
					//		ValueLabel = _top_invent1.ToString(),
					//		ValueLabelColor = SKColors.LawnGreen
					//	},
					//	new ChartEntry(_top_invent2)
					//	{
					//		Color = SKColors.IndianRed,
					//		TextColor = SKColors.IndianRed,
					//		Label = "Producto 2",
					//		ValueLabel = _top_invent2.ToString(),
					//		ValueLabelColor = SKColors.IndianRed
					//	},
					//	new ChartEntry(_top_invent3)
					//	{
					//		Color = SKColors.DeepSkyBlue,
					//		TextColor = SKColors.DeepSkyBlue,
					//		Label = "Producto 3",
					//		ValueLabel = _top_invent3.ToString(),
					//		ValueLabelColor = SKColors.DeepSkyBlue
					//	},
					//	new ChartEntry(_top_invent4)
					//	{
					//		Color = SKColors.Brown,
					//		TextColor = SKColors.Brown,
					//		Label = "Producto 4",
					//		ValueLabel = _top_invent4.ToString(),
					//		ValueLabelColor = SKColors.Brown
					//	},
					//};
					//		grafP_almacen.Chart = new BarChart()
					//		{
					//			Entries = entriesInventario,
					//			BackgroundColor = SKColor.Parse("#40616B"),
					//			LabelTextSize = 30,
					//			LabelOrientation = Orientation.Horizontal,
					//			ValueLabelOrientation = Orientation.Horizontal,
					//			LabelColor = SKColors.White,
					//		};
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