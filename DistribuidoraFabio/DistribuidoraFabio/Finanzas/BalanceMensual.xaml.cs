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
		private decimal _costo_fijo;
		private decimal _costo_variable;
		private decimal _ingresos = 1650;
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
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostos.php");
					var dataCostos = JsonConvert.DeserializeObject<List<Costos>>(response);

					foreach (var item in dataCostos)
					{
						if (item.tipo_costo == "Fijo")
						{
							_costo_fijo = _costo_fijo + item.monto;
						}
						else if (item.tipo_costo == "Variable")
						{
							_costo_variable = _costo_variable + item.monto;
						}
					}
					float _C_Fijo = (float)_costo_fijo;
					float _C_Variable = (float)_costo_variable;
					float _C_Ingresos = (float)_ingresos;
					txtCostosF.Text = "Costos fijos: " + _costo_fijo.ToString() + " Bs.";
					txtCostosV.Text = "Costos variables: " + _costo_variable.ToString() + " Bs.";
					txtIngresos.Text = "Ingresos: " + _ingresos.ToString() + " Bs.";
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
				new ChartEntry(_C_Ingresos)
					{
						Color = SKColor.Parse("#059B00"),
					},
			};
					grafico1.Chart = new DonutChart() { Entries = entries, BackgroundColor = SKColor.Parse("#40616B"), GraphPosition = GraphPosition.AutoFill };

					txtCostosF2.Text = "Costos fijos: " + _costo_fijo.ToString() + " Bs.";
					txtCostosV2.Text = "Costos variables: " + _costo_variable.ToString() + " Bs.";
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

					float _top_vendedor1 = (float)_vend1;
					float _top_vendedor2 = (float)_vend2;
					float _top_vendedor3 = (float)_vend3;
					float _top_vendedor4 = (float)_vend4;
					var entriesVendedores = new[]
					{
				new ChartEntry(_top_vendedor1)
				{
					Color = SKColors.OrangeRed,
					TextColor = SKColors.OrangeRed,
					Label = "Vendedor 1",
					ValueLabel = _top_vendedor1.ToString(),
					ValueLabelColor = SKColors.OrangeRed
				},
				new ChartEntry(_top_vendedor2)
				{
					Color = SKColors.Yellow,
					TextColor = SKColors.Yellow,
					Label = "Vendedor 2",
					ValueLabel = _top_vendedor2.ToString(),
					ValueLabelColor = SKColors.Yellow
				},
				new ChartEntry(_top_vendedor3)
				{
					Color = SKColors.DarkSeaGreen,
					TextColor = SKColors.DarkSeaGreen,
					Label = "Vendedor 3",
					ValueLabel = _top_vendedor3.ToString(),
					ValueLabelColor = SKColors.DarkSeaGreen
				},
				new ChartEntry(_top_vendedor4)
				{
					Color = SKColors.BlueViolet,
					TextColor = SKColors.BlueViolet,
					Label = "Vendedor 4",
					ValueLabel = _top_vendedor4.ToString(),
					ValueLabelColor = SKColors.BlueViolet
				},
			};

					grafVendedores.Chart = new BarChart()
					{
						Entries = entriesVendedores,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
						LabelColor = SKColors.White,
					};

					float _top_cliente1 = (float)_cliente1;
					float _top_cliente2 = (float)_cliente2;
					float _top_cliente3 = (float)_cliente3;
					float _top_cliente4 = (float)_cliente4;
					var entriesCliente = new[]
					{
				new ChartEntry(_top_cliente1)
				{
					Color = SKColors.OrangeRed,
					TextColor = SKColors.OrangeRed,
					Label = "Cliente 1",
					ValueLabel = _top_cliente1.ToString(),
					ValueLabelColor = SKColors.OrangeRed
				},
				new ChartEntry(_top_cliente2)
				{
					Color = SKColors.Yellow,
					TextColor = SKColors.Yellow,
					Label = "Cliente 2",
					ValueLabel = _top_cliente2.ToString(),
					ValueLabelColor = SKColors.Yellow
				},
				new ChartEntry(_top_cliente3)
				{
					Color = SKColors.DarkSeaGreen,
					TextColor = SKColors.DarkSeaGreen,
					Label = "Cliente 3",
					ValueLabel = _top_cliente3.ToString(),
					ValueLabelColor = SKColors.DarkSeaGreen
				},
				new ChartEntry(_top_cliente4)
				{
					Color = SKColors.BlueViolet,
					TextColor = SKColors.BlueViolet,
					Label = "Cliente 4",
					ValueLabel = _top_cliente4.ToString(),
					ValueLabelColor = SKColors.BlueViolet
				},
			};
					grafClientes.Chart = new BarChart()
					{
						Entries = entriesCliente,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
						LabelColor = SKColors.White,
					};

					float _top_producto1 = (float)_producto1;
					float _top_producto2 = (float)_producto2;
					float _top_producto3 = (float)_producto3;
					float _top_producto4 = (float)_producto4;
					var entriesProductos = new[]
					{
				new ChartEntry(_top_producto1)
				{
					Color = SKColors.DarkOrange,
					TextColor = SKColors.DarkOrange,
					Label = "Producto 1",
					ValueLabel = _top_producto1.ToString(),
					ValueLabelColor = SKColors.DarkOrange
				},
				new ChartEntry(_top_producto2)
				{
					Color = SKColors.DarkCyan,
					TextColor = SKColors.DarkCyan,
					Label = "Producto 2",
					ValueLabel = _top_producto2.ToString(),
					ValueLabelColor = SKColors.DarkCyan
				},
				new ChartEntry(_top_producto3)
				{
					Color = SKColors.MediumPurple,
					TextColor = SKColors.MediumPurple,
					Label = "Producto 3",
					ValueLabel = _top_producto3.ToString(),
					ValueLabelColor = SKColors.MediumPurple
				},
				new ChartEntry(_top_producto4)
				{
					Color = SKColors.ForestGreen,
					TextColor = SKColors.ForestGreen,
					Label = "Producto 4",
					ValueLabel = _top_producto4.ToString(),
					ValueLabelColor = SKColors.ForestGreen
				},
			};
					grafP_MasVendidos.Chart = new BarChart()
					{
						Entries = entriesProductos,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
						LabelColor = SKColors.White,
					};

					float _top_invent1 = (float)_p_inventario1;
					float _top_invent2 = (float)_p_inventario2;
					float _top_invent3 = (float)_p_inventario3;
					float _top_invent4 = (float)_p_inventario4;
					var entriesInventario = new[]
					{
				new ChartEntry(_top_invent1)
				{
					Color = SKColors.LawnGreen,
					TextColor = SKColors.LawnGreen,
					Label = "Producto 1",
					ValueLabel = _top_invent1.ToString(),
					ValueLabelColor = SKColors.LawnGreen
				},
				new ChartEntry(_top_invent2)
				{
					Color = SKColors.IndianRed,
					TextColor = SKColors.IndianRed,
					Label = "Producto 2",
					ValueLabel = _top_invent2.ToString(),
					ValueLabelColor = SKColors.IndianRed
				},
				new ChartEntry(_top_invent3)
				{
					Color = SKColors.DeepSkyBlue,
					TextColor = SKColors.DeepSkyBlue,
					Label = "Producto 3",
					ValueLabel = _top_invent3.ToString(),
					ValueLabelColor = SKColors.DeepSkyBlue
				},
				new ChartEntry(_top_invent4)
				{
					Color = SKColors.Brown,
					TextColor = SKColors.Brown,
					Label = "Producto 4",
					ValueLabel = _top_invent4.ToString(),
					ValueLabelColor = SKColors.Brown
				},
			};
					grafP_almacen.Chart = new BarChart()
					{
						Entries = entriesInventario,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
						LabelColor = SKColors.White,
					};
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