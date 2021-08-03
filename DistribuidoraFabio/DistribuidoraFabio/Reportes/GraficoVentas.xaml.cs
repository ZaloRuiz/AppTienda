using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Microcharts;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Reportes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraficoVentas : ContentPage
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
		private int _diasPrimerSemana = 0;
		private int _diasSegundaSemana = 0;
		private int _diasTerceraSemana = 0;
		private int _diasCuartaSemana = 0;
		private int _diasQuintaSemana = 0;
		private DateTime PrimerDiaMes;
		DateTime _inicioSem1;
		DateTime _finSem1;
		DateTime _inicioSem2;
		DateTime _finSem2;
		DateTime _inicioSem3;
		DateTime _finSem3;
		DateTime _inicioSem4;
		DateTime _finSem4;
		DateTime _inicioSem5;
		DateTime _finSem5;
		DateTime _inicioSem6;
		DateTime _finSem6;
		private DateTime _InicioSemana = DateTime.Today;
		private DateTime _FinalSemana = DateTime.Today.AddDays(-7);
		private DateTime _InicioYear;
		private DateTime _FinalYear;
		private DateTime _InicioMes;
		private DateTime _FinMes;
		private int MesSelected = 0;
		int _diasSextaSemana = 0;
		List<Vendedores> vendedorList = new List<Vendedores>();
		List<ProductoNombre> productosList = new List<ProductoNombre>();
		List<Tipo_producto> tipoProductosList = new List<Tipo_producto>();
		private string pickedTP;
		private string pickedProducto;
		private int _idProducto = 0;
		private int _idTipoProducto = 0;
		private string _filtroProd = "Todos";
		private string _tipoResultado;
		List<string> ListSemanas = new List<string>();
		private int idVendedorSelected = 0;
		ObservableCollection<GraficoVentaDiaria> _listaVentasDia = new ObservableCollection<GraficoVentaDiaria>();
		ObservableCollection<GraficoVentaDiariaPorProducto> _listaVentaDiaXProducto = new ObservableCollection<GraficoVentaDiariaPorProducto>();
		ObservableCollection<GraficoVentaDiaria> _listaVentasDiaComparar = new ObservableCollection<GraficoVentaDiaria>();
		ObservableCollection<GraficoVentaDiariaPorProducto> _listaVentaDiaXProductoComparar = new ObservableCollection<GraficoVentaDiariaPorProducto>();
		ObservableCollection<VentasNombre> _listaVentasSemana = new ObservableCollection<VentasNombre>();
		ObservableCollection<VentasNombre> _listaVentasYear = new ObservableCollection<VentasNombre>();
		private string DiaDeLaSemana;
		private DateTime DiasSemana;
		string BusyReason = "Cargando...";
		private PlotView _opv = new PlotView();
		public GraficoVentas()
		{
			InitializeComponent();
			pickSemMes.ItemsSource = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
			pickSemYear.ItemsSource = new List<string> { "2021", "2022", "2023", "2024", "2025" };
			pickYear.ItemsSource = new List<string> { "2021", "2022", "2023", "2024", "2025" };
			pickMes.ItemsSource = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
			pickMesYear.ItemsSource = new List<string> { "2021", "2022", "2023", "2024", "2025" };
			GetDataVendedor();
			GetTipoProducto();
			GetProductos();
		}
		private async void GetDataVendedor()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/vendedores/listaVendedores.php");
					var vendedores = JsonConvert.DeserializeObject<List<Vendedores>>(response).ToList();
					pickerVendedor.ItemsSource = vendedores;
					foreach (var item in vendedores)
					{
						vendedorList.Add(item);
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetTipoProducto()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tp_productos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response).ToList();
					
					foreach(var item in tp_productos)
					{
						tipoProductosList.Add(item);
					}
					pickerTipoProducto.Items.Add("Todos");
					foreach (var item in tipoProductosList)
					{
						pickerTipoProducto.Items.Add(item.nombre_tipo_producto);
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("Error", error.ToString(), "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		public async void GetProductos()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var prodsList = JsonConvert.DeserializeObject<List<ProductoNombre>>(response).ToList();
					foreach (var item in prodsList)
					{
						productosList.Add(item);
					}
					pickerProducto.Items.Add("Todos");
					foreach(var item in productosList)
					{
						pickerProducto.Items.Add(item.nombre_producto);
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("Error", error.ToString(), "OK");
				}
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
						fecha_year = 2021
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
					//graficoVentas.Chart = new LineChart()
					//{
					//	Entries = entries,
					//	BackgroundColor = SKColor.Parse("#40616B"),
					//	LabelColor = SKColors.White,
					//	LabelTextSize = 30,
					//	LabelOrientation = Orientation.Horizontal,
					//	ValueLabelOrientation = Orientation.Horizontal,
					//	LineMode = LineMode.Straight
					//};
					await PopupNavigation.Instance.PopAsync();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
		}
		public void GetGrafico()
		{

			var Points = new List<DataPoint>
		{
            //DateTimeAxis.ToDouble(new DateTime(1989, 10, 3)), 8)
            new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 1)), 8000),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 2)), 7420),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 3)), 6541),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 4)), 7887),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 5)), 9671),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 6)), 11215),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 7)), 7582),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 8)), 6014),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 9)), 4772)
		};
			var Points2 = new List<DataPoint>
		{
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 1)), 4200),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 2)), 2963),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 3)), 3500),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 4)), 8455),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 5)), 7602),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 6)), 4117),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 7)), 3668),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 8)), 9412),
			new DataPoint(DateTimeAxis.ToDouble(new DateTime(2021, 6, 9)), 6471)
		};

			var m = new PlotModel();
			m.PlotType = PlotType.XY;
			m.InvalidatePlot(false);

			m.Title = "Ventas";


			var startDate = DateTime.Now.AddMonths(-3);
			var endDate = DateTime.Now;

			var minValue = DateTimeAxis.ToDouble(startDate);
			var maxValue = DateTimeAxis.ToDouble(endDate);
			m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "MMM/yyyy" });
			m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 2800, Maximum = 12000 });
			m.ResetAllAxes();

			var ls1 = new LineSeries();
			var ls2 = new LineSeries();
			//var ls3 = new LineSeries();
			//var ls4 = new LineSeries();
			//MarkerType = OxyPlot.MarkerType.Circle,
			ls1.MarkerType = OxyPlot.MarkerType.Circle;
			ls2.MarkerType = OxyPlot.MarkerType.Circle;
			//ls3.MarkerType = OxyPlot.MarkerType.Circle;
			//ls4.MarkerType = OxyPlot.MarkerType.Circle;
			ls1.ItemsSource = Points;
			ls2.ItemsSource = Points2;
			//ls3.ItemsSource = Points3;
			//ls4.ItemsSource = Points4;

			m.Series.Add(ls1);
			m.Series.Add(ls2);
			//m.Series.Add(ls3);
			//m.Series.Add(ls4);
			_opv = new PlotView
			{
				WidthRequest = 300,
				HeightRequest = 300,
				BackgroundColor = Color.White,

			};
			_opv.Model = m;
			//Content = _opv;
			stkGrafico.Children.Add(_opv);
		}
		private void RB_Dia_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_Dia.IsChecked)
			{
				stkDia.IsVisible = true;
				stkSemana.IsVisible = false;
				stkMes.IsVisible = false;
				stkYear.IsVisible = false;
			}
		}
		private void RB_Semana_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_Semana.IsChecked)
			{
				stkDia.IsVisible = false;
				stkSemana.IsVisible = true;
				stkMes.IsVisible = false;
				stkYear.IsVisible = false;
			}
		}
		private void RB_Mes_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if(RB_Mes.IsChecked)
			{
				stkDia.IsVisible = false;
				stkSemana.IsVisible = false;
				stkMes.IsVisible = true;
				stkYear.IsVisible = false;
			}
		}
		private void RB_Year_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_Year.IsChecked)
			{
				stkDia.IsVisible = false;
				stkSemana.IsVisible = false;
				stkMes.IsVisible = false;
				stkYear.IsVisible = true;
			}
		}
		private string vendedorPick;
		private async void pickerVendedor_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if (selectedIndex != -1)
				{
					vendedorPick = picker.Items[selectedIndex];
					try
					{
						foreach (var item in vendedorList)
						{
							if (vendedorPick == item.nombre)
							{
								idVendedorSelected = item.id_vendedor;
							}
						}
					}
					catch (Exception err)
					{
						await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
					}
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
			}
		}
		private async void pickSemMes_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				List<DateTime> _fechaElegida = new List<DateTime>();
				CultureInfo ci = Thread.CurrentThread.CurrentCulture;
				int _numeroMes = pickSemMes.SelectedIndex + 1;
				string _nombreMes = ci.DateTimeFormat.GetMonthName(_numeroMes);
				int _numeroYear = Convert.ToInt32(pickSemYear.SelectedItem);
				PrimerDiaMes = new DateTime(_numeroYear, _numeroMes, 1);
				DiaDeLaSemana = ci.DateTimeFormat.GetDayName(PrimerDiaMes.DayOfWeek).ToString();
				DiasSemana = PrimerDiaMes;
				DateTime _DiasSemanaPorMes = PrimerDiaMes;
				int _diasPorMes = DateTime.DaysInMonth(_numeroYear, _numeroMes);

				//agregar fechas del mes elegido a lista
				for (int d = 0; d < _diasPorMes; d++)
				{
					_fechaElegida.Add(_DiasSemanaPorMes);
					_DiasSemanaPorMes.AddDays(1);
				}
				int contadorDias = 0;
				_diasPrimerSemana = 0;
				_diasSegundaSemana = 0;
				_diasTerceraSemana = 0;
				_diasCuartaSemana = 0;
				_diasQuintaSemana = 0;
				_diasSextaSemana = 0;

				//obtener la cantidad de dias por semana
				for (int d = 0; d < _diasPorMes; d++)
				{
					if (DiaDeLaSemana.ToLower() == "domingo")
					{
						if (_diasPrimerSemana == 0)
						{
							_diasPrimerSemana = contadorDias + 1;
							contadorDias = 0;
							DiasSemana = DiasSemana.AddDays(1);
							DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
						}
						else if (_diasSegundaSemana == 0)
						{
							_diasSegundaSemana = contadorDias + 1;
							contadorDias = 0;
							DiasSemana = DiasSemana.AddDays(1);
							DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
						}
						else if (_diasTerceraSemana == 0)
						{
							_diasTerceraSemana = contadorDias + 1;
							contadorDias = 0;
							DiasSemana = DiasSemana.AddDays(1);
							DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
						}
						else if (_diasCuartaSemana == 0)
						{
							_diasCuartaSemana = contadorDias + 1;
							contadorDias = 0;
							DiasSemana = DiasSemana.AddDays(1);
							DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
						}
						else if (_diasQuintaSemana == 0)
						{
							_diasQuintaSemana = contadorDias + 1;
							contadorDias = 0;
							DiasSemana = DiasSemana.AddDays(1);
							DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
						}
					}
					else
					{
						contadorDias = contadorDias + 1;
						DiasSemana = DiasSemana.AddDays(1);
						DiaDeLaSemana = ci.DateTimeFormat.GetDayName(DiasSemana.DayOfWeek).ToString();
					}
				}
				if (_diasQuintaSemana == 0)
				{
					_diasQuintaSemana = contadorDias;
				}
				if (_diasQuintaSemana == 7)
				{
					_diasSextaSemana = contadorDias;
				}

				//Ahora toca calcular el 1er y ultimo dia de cada semana en base a la cantidad de dias por semana y al primer dia del mes
				_inicioSem1 = PrimerDiaMes;
				_finSem1 = _inicioSem1.AddDays(_diasPrimerSemana - 1);
				_inicioSem2 = _finSem1.AddDays(1);
				_finSem2 = _inicioSem2.AddDays(_diasSegundaSemana - 1);
				_inicioSem3 = _finSem2.AddDays(1);
				_finSem3 = _inicioSem3.AddDays(_diasTerceraSemana - 1);
				_inicioSem4 = _finSem3.AddDays(1);
				_finSem4 = _inicioSem4.AddDays(_diasCuartaSemana - 1);
				if (_diasQuintaSemana > 0)
				{
					_inicioSem5 = _finSem4.AddDays(1);
					_finSem5 = _inicioSem5.AddDays(_diasQuintaSemana - 1);
					pickSemSemana.ItemsSource = new List<string> { "Semana 1", "Semana 2", "Semana 3", "Semana 4", "Semana 5" };
					if (_diasSextaSemana > 0)
					{
						_inicioSem6 = _finSem5.AddDays(1);
						_finSem6 = _inicioSem6.AddDays(_diasSextaSemana - 1);
						pickSemSemana.ItemsSource = new List<string> { "Semana 1", "Semana 2", "Semana 3", "Semana 4", "Semana 5", "Semana 6" };
					}
				}
				else
				{
					pickSemSemana.ItemsSource = new List<string> { "Semana 1", "Semana 2", "Semana 3", "Semana 4" };
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private async void pickSemSemana_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if (selectedIndex != -1)
				{
					if (picker.Items[selectedIndex] == "Semana 1")
					{
						_InicioSemana = _inicioSem1;
						_FinalSemana = _finSem1;
					}
					else if (picker.Items[selectedIndex] == "Semana 2")
					{
						_InicioSemana = _inicioSem2;
						_FinalSemana = _finSem2;
					}
					else if (picker.Items[selectedIndex] == "Semana 3")
					{
						_InicioSemana = _inicioSem3;
						_FinalSemana = _finSem3;
					}
					else if (picker.Items[selectedIndex] == "Semana 4")
					{
						_InicioSemana = _inicioSem4;
						_FinalSemana = _finSem4;
					}
					else if (picker.Items[selectedIndex] == "Semana 5")
					{
						_InicioSemana = _inicioSem5;
						_FinalSemana = _finSem5;
					}
					else if (picker.Items[selectedIndex] == "Semana 6")
					{
						_InicioSemana = _inicioSem6;
						_FinalSemana = _finSem6;
					}
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private async void pickMes_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if(selectedIndex != -1)
				{
					MesSelected = selectedIndex + 1;
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private async void pickMesYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			int YearMesSelected = 0;
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if (selectedIndex != -1)
				{
					YearMesSelected = Convert.ToInt32(picker.Items[selectedIndex]);
				}
				_InicioMes = new DateTime(YearMesSelected, MesSelected, 1);
				_FinMes = _InicioMes.AddMonths(1).AddDays(-1);
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private async void pickYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			int YearSelected = 0;
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if (selectedIndex != -1)
				{
					YearSelected = Convert.ToInt32(picker.Items[selectedIndex]);
				}
				_InicioYear = new DateTime(YearSelected, 1, 1);
				_FinalYear = new DateTime(YearSelected, 12, 31);
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private async void pickerTipoProducto_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;

				if (selectedIndex != -1)
				{
					pickedTP = picker.Items[selectedIndex];
					foreach (var item in tipoProductosList)
					{
						if (item.nombre_tipo_producto == pickedTP)
						{
							_idTipoProducto = item.id_tipoproducto;
						}
					}
				}
			}
			catch (Exception error)
			{
				await DisplayAlert("Error", error.ToString(), "OK");
			}
		}
		private async void pickerProducto_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;

				if (selectedIndex != -1)
				{
					pickedProducto = picker.Items[selectedIndex];
					foreach (var item in productosList)
					{
						if (item.nombre_producto == pickedProducto)
						{
							_idProducto = item.id_producto;
						}
					}
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
		}
		private void RB_TipoProducto_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_TipoProducto.IsChecked)
			{
				pickerProducto.IsVisible = false;
				pickerTipoProducto.IsVisible = true;
				_filtroProd = "TipoProducto";
			}
		}
		private void RB_Producto_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_Producto.IsChecked)
			{
				pickerProducto.IsVisible = true;
				pickerTipoProducto.IsVisible = false;
				_filtroProd = "Producto";
				//await PopupNavigation.Instance.PushAsync(new ListaR_MutiProductos());
			}
		}
		private void RB_Todos_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (RB_Todos.IsChecked)
			{
				pickerProducto.IsVisible = false;
				pickerTipoProducto.IsVisible = false;
				_filtroProd = "Todos";
			}
		}
		private async void btnFiltrar_Clicked(object sender, EventArgs e)
		{
			stkGrafico.Children.Clear();
			_listaVentaDiaXProducto.Clear();
			_listaVentasDia.Clear();
			decimal _montoMaximo = 0;
			decimal _montoMinimo = 0;
			btnComparar.IsVisible = true;
			//filtrado por dia
			if (RB_Dia.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							
							//await DisplayAlert("Valores", _listaVentaDiaXProducto.Count.ToString(), "OK");
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if(RB_Cajas.IsChecked)
							{
								if(_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}

									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if(RB_Bolivianos.IsChecked)
							{
								if(_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}

									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							
							await DisplayAlert("Valores", _listaVentaDiaXProducto.Count.ToString(), "OK");
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			//filtrado por semana
			else if (RB_Semana.IsChecked)
			{
				//Consulta al servidor
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioSemana,
								fecha_final = _FinalSemana
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
							
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioSemana.Date.AddDays(-2);
									var endDate = _FinalSemana.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			//filtrado por mes
			else if(RB_Mes.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			//filtrado de por año
			else if (RB_Year.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							Points.Clear();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							Points.Clear();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							Points.Clear();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentasDia)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentasDia)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}

									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioYear.Date.AddDays(-2);
									var endDate = _FinalYear.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = Convert.ToDouble(_montoMinimo) - 50;
									double _maximum = Convert.ToDouble(_montoMaximo) + 100;
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;

									m.Series.Add(ls1);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
		}
		private async void btnComparar_Clicked(object sender, EventArgs e)
		{
			stkGrafico.Children.Clear();
			_listaVentaDiaXProducto.Clear();
			_listaVentasDia.Clear();
			decimal _montoMaximo = 0;
			decimal _montoMinimo = 0;
			decimal _montoMaximoComp = 0;
			decimal _montoMinimoComp = 0;
			//filtrado por dia
			if (RB_Dia.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if(_filtroProd == "Producto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date.AddYears(-1),
								fecha_final = fechaFinalDiaria.Date.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if(_filtroProd == "TipoProducto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date.AddYears(-1),
								fecha_final = fechaFinalDiaria.Date.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if(_filtroProd == "Todos")
					{
						//Datos de la busqueda
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = fechaInicioDiaria.Date.AddYears(-1),
								fecha_final = fechaFinalDiaria.Date.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas_comparar = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							//listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas_comparar)
							{
								_listaVentasDiaComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " a " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

								var startDate = fechaInicioDiaria.Date.AddDays(-2);
								var endDate = fechaFinalDiaria.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 50;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " a " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

								var startDate = fechaInicioDiaria.Date.AddDays(-2);
								var endDate = fechaFinalDiaria.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 50;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
					}
					else
					{
						await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
					}
				}
			}
			//filtrado por semana
			else if (RB_Semana.IsChecked)
			{
				//Consulta al servidor
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = fechaInicioDiaria.Date.AddYears(-1),
								fecha_final = fechaFinalDiaria.Date.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.cantidad < _montoMinimo)
									{
										_montoMinimo = item.cantidad;
									}
									if (item.cantidad > _montoMaximo)
									{
										_montoMaximo = item.cantidad;
									}
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									if (item.cantidad < _montoMinimoComp)
									{
										_montoMinimoComp = item.cantidad;
									}
									if (item.cantidad > _montoMaximoComp)
									{
										_montoMaximoComp = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioSemana.Date.AddDays(-2);
								var endDate = _FinalSemana.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioSemana.Date.AddDays(-2);
								var endDate = _FinalSemana.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						
					}
					else if (_filtroProd == "TipoProducto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date,
								fecha_final = fechaFinalDiaria.Date
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = fechaInicioDiaria.Date.AddYears(-1),
								fecha_final = fechaFinalDiaria.Date.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.cantidad < _montoMinimo)
									{
										_montoMinimo = item.cantidad;
									}
									if (item.cantidad > _montoMaximo)
									{
										_montoMaximo = item.cantidad;
									}
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									if (item.cantidad < _montoMinimoComp)
									{
										_montoMinimoComp = item.cantidad;
									}
									if (item.cantidad > _montoMaximoComp)
									{
										_montoMaximoComp = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioSemana.Date.AddDays(-2);
								var endDate = _FinalSemana.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentaDiaXProductoComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioSemana.Date.ToString("dd/MM/yyyy") + " a " + _FinalSemana.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioSemana.Date.AddDays(-2);
								var endDate = _FinalSemana.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						
					}
					else if (_filtroProd == "Todos")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioYear.AddYears(1),
								fecha_final = _FinalYear.AddYears(1)
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDiaComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
						
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.cantidad < _montoMinimo)
									{
										_montoMinimo = item.cantidad;
									}
									if (item.cantidad > _montoMaximo)
									{
										_montoMaximo = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.cantidad < _montoMinimoComp)
									{
										_montoMinimoComp = item.cantidad;
									}
									if (item.cantidad > _montoMaximoComp)
									{
										_montoMaximoComp = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioYear.Date.AddDays(-2);
								var endDate = _FinalYear.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioYear.Date.AddDays(-2);
								var endDate = _FinalYear.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			//filtrado por mes
			else if(RB_Mes.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioMes.AddYears(-1),
								fecha_final = _FinMes.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioMes.AddYears(-1),
								fecha_final = _FinMes.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " al " + _FinMes.Date.ToString("dd/MM/yyyy");

									var startDate = _InicioMes.Date.AddDays(-2);
									var endDate = _FinMes.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						//Datos de la busqueda
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioMes,
								fecha_final = _FinMes
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioMes.AddYears(-1),
								fecha_final = _FinMes.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas_comparar = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							//listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas_comparar)
							{
								_listaVentasDiaComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
						}
						
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.cantidad < _montoMinimo)
									{
										_montoMinimo = item.cantidad;
									}
									if (item.cantidad > _montoMaximo)
									{
										_montoMaximo = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.cantidad < _montoMinimoComp)
									{
										_montoMinimoComp = item.cantidad;
									}
									if (item.cantidad > _montoMaximoComp)
									{
										_montoMaximoComp = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " a " + _FinMes.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioMes.Date.AddDays(-2);
								var endDate = _FinMes.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 50;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}

								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioMes.Date.ToString("dd/MM/yyyy") + " a " + _FinMes.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioMes.Date.AddDays(-2);
								var endDate = _FinMes.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 50;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
					}
					else
					{
						await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
					}
				}
			}
			//filtrado por año
			else if (RB_Year.IsChecked)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					string BusyReason = "Cargando...";
					await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					if (_filtroProd == "Producto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_producto = _idProducto,
								fecha_inicio = _InicioYear.AddYears(-1),
								fecha_final = _FinalYear.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "TipoProducto")
					{
						//Datos fecha de busqueda
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProducto.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiariaPorProducto _VentasXTipoProd = new GraficoVentaDiariaPorProducto()
							{
								id_vendedor = idVendedorSelected,
								id_tipo_producto = _idTipoProducto,
								fecha_inicio = _InicioYear.AddYears(-1),
								fecha_final = _FinalYear.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_VentasXTipoProd);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiariasPorTipoProducto.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiariaPorProducto>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentaDiaXProductoComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
						try
						{
							//Crear grafico
							stkGrafico.Children.Clear();
							List<DataPoint> Points = new List<DataPoint>();
							List<DataPoint> PointsComp = new List<DataPoint>();
							if (RB_Cajas.IsChecked)
							{
								if (_tipoResultado == "Cajas")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.cantidad < _montoMinimo)
										{
											_montoMinimo = item.cantidad;
										}
										if (item.cantidad > _montoMaximo)
										{
											_montoMaximo = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.cantidad < _montoMinimoComp)
										{
											_montoMinimoComp = item.cantidad;
										}
										if (item.cantidad > _montoMaximoComp)
										{
											_montoMaximoComp = item.cantidad;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
							else if (RB_Bolivianos.IsChecked)
							{
								if (_tipoResultado == "Bolivianos")
								{
									foreach (var item in _listaVentaDiaXProducto)
									{
										if (item.total < _montoMinimo)
										{
											_montoMinimo = item.total;
										}
										if (item.total > _montoMaximo)
										{
											_montoMaximo = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										if (item.total < _montoMinimoComp)
										{
											_montoMinimoComp = item.total;
										}
										if (item.total > _montoMaximoComp)
										{
											_montoMaximoComp = item.total;
										}
									}
									foreach (var item in _listaVentaDiaXProducto)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									foreach (var item in _listaVentaDiaXProductoComparar)
									{
										Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
									}
									var m = new PlotModel();
									m.PlotType = PlotType.XY;
									m.InvalidatePlot(false);

									m.Title = "Ventas de " + vendedorPick + " de fechas " + fechaInicioDiaria.Date.ToString("dd/MM/yyyy") + " al " + fechaFinalDiaria.Date.ToString("dd/MM/yyyy");

									var startDate = fechaInicioDiaria.Date.AddDays(-2);
									var endDate = fechaFinalDiaria.Date.AddDays(2);

									var minValue = DateTimeAxis.ToDouble(startDate);
									var maxValue = DateTimeAxis.ToDouble(endDate);
									m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
									double _minimum = 0;
									double _maximum = 0;
									if (_montoMinimo > _montoMinimoComp)
									{
										_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
									}
									else
									{
										_minimum = Convert.ToDouble(_montoMinimo) - 50;
									}
									if (_montoMaximo > _montoMaximoComp)
									{
										_maximum = Convert.ToDouble(_montoMaximo) + 50;
									}
									else
									{
										_maximum = Convert.ToDouble(_montoMaximoComp) + 50;
									}
									m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
									m.ResetAllAxes();

									var ls1 = new LineSeries();
									var ls2 = new LineSeries();
									ls1.MarkerType = OxyPlot.MarkerType.Circle;
									ls2.MarkerType = OxyPlot.MarkerType.Circle;
									ls1.ItemsSource = Points;
									ls2.ItemsSource = PointsComp;

									m.Series.Add(ls1);
									m.Series.Add(ls2);
									_opv = new PlotView
									{
										WidthRequest = 300,
										HeightRequest = 340,
										BackgroundColor = Color.White,
									};
									_opv.Model = m;
									stkGrafico.Children.Add(_opv);
									await PopupNavigation.Instance.PopAsync();
								}
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "Ok");
						}
					}
					else if (_filtroProd == "Todos")
					{
						//Datos de la busqueda
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioYear,
								fecha_final = _FinalYear
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDia.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
						//Datos año anterior
						try
						{
							GraficoVentaDiaria _Ventas = new GraficoVentaDiaria()
							{
								id_vendedor = idVendedorSelected,
								fecha_inicio = _InicioYear.AddYears(-1),
								fecha_final = _FinalYear.AddYears(-1)
							};
							var json = JsonConvert.SerializeObject(_Ventas);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteGraficoVentasDiaria.php", content);

							var jsonR = await result.Content.ReadAsStringAsync();
							var lista_ventas = JsonConvert.DeserializeObject<List<GraficoVentaDiaria>>(jsonR);
							listDataReporte.ItemsSource = lista_ventas;
							foreach (var item in lista_ventas)
							{
								_listaVentasDiaComparar.Add(item);
							}
						}
						catch (Exception err)
						{
							await DisplayAlert("Error", err.ToString(), "OK");
						}
						//Crear grafico
						stkGrafico.Children.Clear();
						List<DataPoint> Points = new List<DataPoint>();
						List<DataPoint> PointsComp = new List<DataPoint>();
						if (RB_Cajas.IsChecked)
						{
							if (_tipoResultado == "Cajas")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.cantidad < _montoMinimo)
									{
										_montoMinimo = item.cantidad;
									}
									if (item.cantidad > _montoMaximo)
									{
										_montoMaximo = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.cantidad < _montoMinimoComp)
									{
										_montoMinimoComp = item.cantidad;
									}
									if (item.cantidad > _montoMaximoComp)
									{
										_montoMaximoComp = item.cantidad;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.cantidad)));
								}
								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioYear.Date.AddDays(-2);
								var endDate = _FinalYear.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else if (RB_Bolivianos.IsChecked)
						{
							if (_tipoResultado == "Bolivianos")
							{
								foreach (var item in _listaVentasDia)
								{
									if (item.total < _montoMinimo)
									{
										_montoMinimo = item.total;
									}
									if (item.total > _montoMaximo)
									{
										_montoMaximo = item.total;
									}
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									if (item.total < _montoMinimoComp)
									{
										_montoMinimoComp = item.total;
									}
									if (item.total > _montoMaximoComp)
									{
										_montoMaximoComp = item.total;
									}
								}
								foreach (var item in _listaVentasDia)
								{
									Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								foreach (var item in _listaVentasDiaComparar)
								{
									PointsComp.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(item.fecha.AddYears(+1).Year, item.fecha.Month, item.fecha.Day)), Convert.ToDouble(item.total)));
								}
								var m = new PlotModel();
								m.PlotType = PlotType.XY;
								m.InvalidatePlot(false);

								m.Title = "Ventas de " + vendedorPick + " de fechas " + _InicioYear.Date.ToString("dd/MM/yyyy") + " a " + _FinalYear.Date.ToString("dd/MM/yyyy");

								var startDate = _InicioYear.Date.AddDays(-2);
								var endDate = _FinalYear.Date.AddDays(2);

								var minValue = DateTimeAxis.ToDouble(startDate);
								var maxValue = DateTimeAxis.ToDouble(endDate);
								m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MMM/yyyy" });
								double _minimum = 0;
								double _maximum = 0;
								if (_montoMinimo > _montoMinimoComp)
								{
									_minimum = Convert.ToDouble(_montoMinimoComp) - 50;
								}
								else
								{
									_minimum = Convert.ToDouble(_montoMinimo) - 50;
								}
								if (_montoMaximo > _montoMaximoComp)
								{
									_maximum = Convert.ToDouble(_montoMaximo) + 100;
								}
								else
								{
									_maximum = Convert.ToDouble(_montoMaximoComp) + 100;
								}
								m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = _minimum, Maximum = _maximum });
								m.ResetAllAxes();

								var ls1 = new LineSeries();
								var ls2 = new LineSeries();
								//MarkerType = OxyPlot.MarkerType.Circle,
								ls1.MarkerType = OxyPlot.MarkerType.Circle;
								ls2.MarkerType = OxyPlot.MarkerType.Circle;
								ls1.ItemsSource = Points;
								ls2.ItemsSource = PointsComp;

								m.Series.Add(ls1);
								m.Series.Add(ls2);
								_opv = new PlotView
								{
									WidthRequest = 300,
									HeightRequest = 340,
									BackgroundColor = Color.White,
								};
								_opv.Model = m;
								stkGrafico.Children.Add(_opv);
								await PopupNavigation.Instance.PopAsync();
							}
						}
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
		}
		private void RB_Cajas_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if(RB_Cajas.IsChecked)
			{
				_tipoResultado = "Cajas";
			}
		}
		private void RB_Bolivianos_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if(RB_Bolivianos.IsChecked)
			{
				_tipoResultado = "Bolivianos";
			}
		}
	}
}