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
	public partial class BalanceAnual : ContentPage
	{
		private decimal _v_enero = 0;
		private decimal _v_febrero = 0;
		private decimal _v_marzo = 0;
		private decimal _v_abril = 0;
		private decimal _v_mayo = 0;
		private decimal _v_junio = 0;
		private decimal _v_julio = 0;
		private decimal _v_agosto = 0;
		private decimal _v_septiembre = 0;
		private decimal _v_octubre = 0;
		private decimal _v_noviembre = 0;
		private decimal _v_diciembre = 0;
		private decimal _c_enero = 0;
		private decimal _c_febrero = 0;
		private decimal _c_marzo = 0;
		private decimal _c_abril = 0;
		private decimal _c_mayo = 0;
		private decimal _c_junio = 0;
		private decimal _c_julio = 0;
		private decimal _c_agosto = 0;
		private decimal _c_septiembre = 0;
		private decimal _c_octubre = 0;
		private decimal _c_noviembre = 0;
		private decimal _c_diciembre = 0;
		private decimal _gf_enero = 0;
		private decimal _gf_febrero = 0;
		private decimal _gf_marzo = 0;
		private decimal _gf_abril = 0;
		private decimal _gf_mayo = 0;
		private decimal _gf_junio = 0;
		private decimal _gf_julio = 0;
		private decimal _gf_agosto = 0;
		private decimal _gf_septiembre = 0;
		private decimal _gf_octubre = 0;
		private decimal _gf_noviembre = 0;
		private decimal _gf_diciembre = 0;
		private decimal _gv_enero = 0;
		private decimal _gv_febrero = 0;
		private decimal _gv_marzo = 0;
		private decimal _gv_abril = 0;
		private decimal _gv_mayo = 0;
		private decimal _gv_junio = 0;
		private decimal _gv_julio = 0;
		private decimal _gv_agosto = 0;
		private decimal _gv_septiembre = 0;
		private decimal _gv_octubre = 0;
		private decimal _gv_noviembre = 0;
		private decimal _gv_diciembre = 0;
		private decimal _total_enero = 0;
		private decimal _total_febrero = 0;
		private decimal _total_marzo = 0;
		private decimal _total_abril = 0;
		private decimal _total_mayo = 0;
		private decimal _total_junio = 0;
		private decimal _total_julio = 0;
		private decimal _total_agosto = 0;
		private decimal _total_septiembre = 0;
		private decimal _total_octubre = 0;
		private decimal _total_noviembre = 0;
		private decimal _total_diciembre = 0;
		private string _yearElegido;
		string BusyReason = "Cargando...";
		public BalanceAnual()
		{
			InitializeComponent();
			pickerYear.ItemsSource = new List<string> { "2021", "2022", "2023", "2024", "2025", "2026" };
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				_v_enero = 0;
				_v_febrero = 0;
				_v_marzo = 0;
				_v_abril = 0;
				_v_mayo = 0;
				_v_junio = 0;
				_v_julio = 0;
				_v_agosto = 0;
				_v_septiembre = 0;
				_v_octubre = 0;
				_v_noviembre = 0;
				_v_diciembre = 0;

				_c_enero = 0;
				_c_febrero = 0;
				_c_marzo = 0;
				_c_abril = 0;
				_c_mayo = 0;
				_c_junio = 0;
				_c_julio = 0;
				_c_agosto = 0;
				_c_septiembre = 0;
				_c_octubre = 0;
				_c_noviembre = 0;
				_c_diciembre = 0;

				_gf_enero = 0;
				_gf_febrero = 0;
				_gf_marzo = 0;
				_gf_abril = 0;
				_gf_mayo = 0;
				_gf_junio = 0;
				_gf_julio = 0;
				_gf_agosto = 0;
				_gf_septiembre = 0;
				_gf_octubre = 0;
				_gf_noviembre = 0;
				_gf_diciembre = 0;

				_gv_enero = 0;
				_gv_febrero = 0;
				_gv_marzo = 0;
				_gv_abril = 0;
				_gv_mayo = 0;
				_gv_junio = 0;
				_gv_julio = 0;
				_gv_agosto = 0;
				_gv_septiembre = 0;
				_gv_octubre = 0;
				_gv_noviembre = 0;
				_gv_diciembre = 0;

				_total_enero = 0;
				_total_febrero = 0;
				_total_marzo = 0;
				_total_abril = 0;
				_total_mayo = 0;
				_total_junio = 0;
				_total_julio = 0;
				_total_agosto = 0;
				_total_septiembre = 0;
				_total_octubre = 0;
				_total_noviembre = 0;
				_total_diciembre = 0;
				_yearElegido = "2021";
				await Task.Delay(200);
				GetDataVentas();
				GetDataCompras();
				GetDataGastosFijos();
				GetDataGastosVariables();
				GetDataGanancias();
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		public async void GetDataVentas()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
				try
				{
					TotalVentasAnual _totalAnual = new TotalVentasAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalVentasPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalVentasAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_v_enero = item.Enero;
						_v_febrero = item.Febrero;
						_v_marzo = item.Marzo;
						_v_abril = item.Abril;
						_v_mayo = item.Mayo;
						_v_junio = item.Junio;
						_v_julio = item.Julio;
						_v_agosto = item.Agosto;
						_v_septiembre = item.Septiembre;
						_v_octubre = item.Octubre;
						_v_noviembre = item.Noviembre;
						_v_noviembre = item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					float _enero = (float)_v_enero;
					float _febrero = (float)_v_febrero;
					float _marzo = (float)_v_marzo;
					float _abril = (float)_v_abril;
					float _mayo = (float)_v_mayo;
					float _junio = (float)_v_junio;
					float _julio = (float)_v_julio;
					float _agosto = (float)_v_agosto;
					float _septiembre = (float)_v_septiembre;
					float _octubre = (float)_v_octubre;
					float _noviembre = (float)_v_noviembre;
					float _diciembre = (float)_v_diciembre;

					var entries = new[]
					{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
					graficoVentasBs.Chart = new BarChart()
					{
						Entries = entries,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelColor = SKColors.White,
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
					};
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
		}
		public async void GetDataCompras()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(250);
				try
				{
					TotalComprasAnual _totalAnual = new TotalComprasAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalComprasPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalComprasAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_c_enero = item.Enero;
						_c_febrero = item.Febrero;
						_c_marzo = item.Marzo;
						_c_abril = item.Abril;
						_c_mayo = item.Mayo;
						_c_junio = item.Junio;
						_c_julio = item.Julio;
						_c_agosto = item.Agosto;
						_c_septiembre = item.Septiembre;
						_c_octubre = item.Octubre;
						_c_noviembre = item.Noviembre;
						_c_noviembre = item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					float _enero = (float)_c_enero;
					float _febrero = (float)_c_febrero;
					float _marzo = (float)_c_marzo;
					float _abril = (float)_c_abril;
					float _mayo = (float)_c_mayo;
					float _junio = (float)_c_junio;
					float _julio = (float)_c_julio;
					float _agosto = (float)_c_agosto;
					float _septiembre = (float)_c_septiembre;
					float _octubre = (float)_c_octubre;
					float _noviembre = (float)_c_noviembre;
					float _diciembre = (float)_c_diciembre;

					var entries = new[]
					{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
					graficoComprasBs.Chart = new BarChart()
					{
						Entries = entries,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelColor = SKColors.White,
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
					};
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
		}
		public async void GetDataGastosFijos()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(250);
				try
				{
					TotalGastosFijosAnual _totalAnual = new TotalGastosFijosAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalGastosFPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalGastosFijosAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_gf_enero = item.Enero;
						_gf_febrero = item.Febrero;
						_gf_marzo = item.Marzo;
						_gf_abril = item.Abril;
						_gf_mayo = item.Mayo;
						_gf_junio = item.Junio;
						_gf_julio = item.Julio;
						_gf_agosto = item.Agosto;
						_gf_septiembre = item.Septiembre;
						_gf_octubre = item.Octubre;
						_gf_noviembre = item.Noviembre;
						_gf_noviembre = item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					float _enero = (float)_gf_enero;
					float _febrero = (float)_gf_febrero;
					float _marzo = (float)_gf_marzo;
					float _abril = (float)_gf_abril;
					float _mayo = (float)_gf_mayo;
					float _junio = (float)_gf_junio;
					float _julio = (float)_gf_julio;
					float _agosto = (float)_gf_agosto;
					float _septiembre = (float)_gf_septiembre;
					float _octubre = (float)_gf_octubre;
					float _noviembre = (float)_gf_noviembre;
					float _diciembre = (float)_gf_diciembre;

					var entries = new[]
					{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
					graficoGastosFBs.Chart = new BarChart()
					{
						Entries = entries,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelColor = SKColors.White,
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
					};
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
		}
		public async void GetDataGastosVariables()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(250);
				try
				{
					TotalGastosVariablesAnual _totalAnual = new TotalGastosVariablesAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalGastosVPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalGastosVariablesAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_gv_enero = item.Enero;
						_gv_febrero = item.Febrero;
						_gv_marzo = item.Marzo;
						_gv_abril = item.Abril;
						_gv_mayo = item.Mayo;
						_gv_junio = item.Junio;
						_gv_julio = item.Julio;
						_gv_agosto = item.Agosto;
						_gv_septiembre = item.Septiembre;
						_gv_octubre = item.Octubre;
						_gv_noviembre = item.Noviembre;
						_gv_noviembre = item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					float _enero = (float)_gv_enero;
					float _febrero = (float)_gv_febrero;
					float _marzo = (float)_gv_marzo;
					float _abril = (float)_gv_abril;
					float _mayo = (float)_gv_mayo;
					float _junio = (float)_gv_junio;
					float _julio = (float)_gv_julio;
					float _agosto = (float)_gv_agosto;
					float _septiembre = (float)_gv_septiembre;
					float _octubre = (float)_gv_octubre;
					float _noviembre = (float)_gv_noviembre;
					float _diciembre = (float)_gv_diciembre;

					var entries = new[]
					{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
					graficoGastosVBs.Chart = new BarChart()
					{
						Entries = entries,
						BackgroundColor = SKColor.Parse("#40616B"),
						LabelColor = SKColors.White,
						LabelTextSize = 30,
						LabelOrientation = Orientation.Horizontal,
						ValueLabelOrientation = Orientation.Horizontal,
					};
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
		}
		public async void GetDataGanancias()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(200);
				try
				{
					TotalVentasAnual _totalAnual = new TotalVentasAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalVentasPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalVentasAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_total_enero = _total_enero + item.Enero;
						_total_febrero = _total_febrero + item.Febrero;
						_total_marzo = _total_marzo + item.Marzo;
						_total_abril = _total_abril + item.Abril;
						_total_mayo = _total_mayo + item.Mayo;
						_total_junio = _total_junio + item.Junio;
						_total_julio = _total_julio + item.Julio;
						_total_agosto = _total_agosto + item.Agosto;
						_total_septiembre = _total_septiembre + item.Septiembre;
						_total_octubre = _total_octubre + item.Octubre;
						_total_noviembre = _total_noviembre + item.Noviembre;
						_total_diciembre = _total_diciembre + item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					TotalComprasAnual _totalAnual = new TotalComprasAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalComprasPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalComprasAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_total_enero = _total_enero - item.Enero;
						_total_febrero = _total_febrero - item.Febrero;
						_total_marzo = _total_marzo - item.Marzo;
						_total_abril = _total_abril - item.Abril;
						_total_mayo = _total_mayo - item.Mayo;
						_total_junio = _total_junio - item.Junio;
						_total_julio = _total_julio - item.Julio;
						_total_agosto = _total_agosto - item.Agosto;
						_total_septiembre = _total_septiembre - item.Septiembre;
						_total_octubre = _total_octubre - item.Octubre;
						_total_noviembre = _total_noviembre - item.Noviembre;
						_total_diciembre = _total_diciembre - item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					TotalGastosFijosAnual _totalAnual = new TotalGastosFijosAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalGastosFPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalGastosFijosAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_total_enero = _total_enero - item.Enero;
						_total_febrero = _total_febrero - item.Febrero;
						_total_marzo = _total_marzo - item.Marzo;
						_total_abril = _total_abril - item.Abril;
						_total_mayo = _total_mayo - item.Mayo;
						_total_junio = _total_junio - item.Junio;
						_total_julio = _total_julio - item.Julio;
						_total_agosto = _total_agosto - item.Agosto;
						_total_septiembre = _total_septiembre - item.Septiembre;
						_total_octubre = _total_octubre - item.Octubre;
						_total_noviembre = _total_noviembre - item.Noviembre;
						_total_diciembre = _total_diciembre - item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(200);
				try
				{
					TotalGastosVariablesAnual _totalAnual = new TotalGastosVariablesAnual()
					{
						fecha_year = Convert.ToInt32(_yearElegido)
					};
					var json = JsonConvert.SerializeObject(_totalAnual);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaTotalGastosVPorMes.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataAnual = JsonConvert.DeserializeObject<List<TotalGastosVariablesAnual>>(jsonR);
					foreach (var item in dataAnual)
					{
						_total_enero = _total_enero - item.Enero;
						_total_febrero = _total_febrero - item.Febrero;
						_total_marzo = _total_marzo - item.Marzo;
						_total_abril = _total_abril - item.Abril;
						_total_mayo = _total_mayo - item.Mayo;
						_total_junio = _total_junio - item.Junio;
						_total_julio = _total_julio - item.Julio;
						_total_agosto = _total_agosto - item.Agosto;
						_total_septiembre = _total_septiembre - item.Septiembre;
						_total_octubre = _total_octubre - item.Octubre;
						_total_noviembre = _total_noviembre - item.Noviembre;
						_total_diciembre = _total_diciembre - item.Diciembre;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
				await Task.Delay(400);
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
			try
			{
				float _enero = (float)_total_enero;
				float _febrero = (float)_total_febrero;
				float _marzo = (float)_total_marzo;
				float _abril = (float)_total_abril;
				float _mayo = (float)_total_mayo;
				float _junio = (float)_total_junio;
				float _julio = (float)_total_julio;
				float _agosto = (float)_total_agosto;
				float _septiembre = (float)_total_septiembre;
				float _octubre = (float)_total_octubre;
				float _noviembre = (float)_total_noviembre;
				float _diciembre = (float)_total_diciembre;

				var entries = new[]
				{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
				graficoGananciasBs.Chart = new BarChart()
				{
					Entries = entries,
					BackgroundColor = SKColor.Parse("#40616B"),
					LabelColor = SKColors.White,
					LabelTextSize = 30,
					LabelOrientation = Orientation.Horizontal,
					ValueLabelOrientation = Orientation.Horizontal,
				};
			}
			catch (Exception err)
			{
				await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
			}
			await PopupNavigation.Instance.PopAsync();
		}
		private void pickerYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;
			if (selectedIndex != -1)
			{
				_yearElegido = picker.Items[selectedIndex];
			}
		}
		private void btnBuscar_Clicked(object sender, EventArgs e)
		{
			_v_enero = 0;
			_v_febrero = 0;
			_v_marzo = 0;
			_v_abril = 0;
			_v_mayo = 0;
			_v_junio = 0;
			_v_julio = 0;
			_v_agosto = 0;
			_v_septiembre = 0;
			_v_octubre = 0;
			_v_noviembre = 0;
			_v_diciembre = 0;

			_c_enero = 0;
			_c_febrero = 0;
			_c_marzo = 0;
			_c_abril = 0;
			_c_mayo = 0;
			_c_junio = 0;
			_c_julio = 0;
			_c_agosto = 0;
			_c_septiembre = 0;
			_c_octubre = 0;
			_c_noviembre = 0;
			_c_diciembre = 0;

			_gf_enero = 0;
			_gf_febrero = 0;
			_gf_marzo = 0;
			_gf_abril = 0;
			_gf_mayo = 0;
			_gf_junio = 0;
			_gf_julio = 0;
			_gf_agosto = 0;
			_gf_septiembre = 0;
			_gf_octubre = 0;
			_gf_noviembre = 0;
			_gf_diciembre = 0;

			_gv_enero = 0;
			_gv_febrero = 0;
			_gv_marzo = 0;
			_gv_abril = 0;
			_gv_mayo = 0;
			_gv_junio = 0;
			_gv_julio = 0;
			_gv_agosto = 0;
			_gv_septiembre = 0;
			_gv_octubre = 0;
			_gv_noviembre = 0;
			_gv_diciembre = 0;

			_total_enero = 0;
			_total_febrero = 0;
			_total_marzo = 0;
			_total_abril = 0;
			_total_mayo = 0;
			_total_junio = 0;
			_total_julio = 0;
			_total_agosto = 0;
			_total_septiembre = 0;
			_total_octubre = 0;
			_total_noviembre = 0;
			_total_diciembre = 0;
			GetDataVentas();
			GetDataCompras();
			GetDataGastosFijos();
			GetDataGastosVariables();
			GetDataGanancias();
		}
	}
}