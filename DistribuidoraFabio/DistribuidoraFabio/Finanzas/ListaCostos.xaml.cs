using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaCostos : ContentPage
	{
		private decimal _totalCF = 0;
		private decimal _totalCV = 0;
		private string _fecha_inicial;
		private string _fecha_final;
		private string _mesElegido;
		private int _mesQuery;
		private string _yearElegido;
		private int _yearQuery;
		private string _mesDefault;
		int _medidaStkFijo = 0;
		int _medidaStkVariable = 0;
		ObservableCollection<Costo_fijo> _listaCostoFijo = new ObservableCollection<Costo_fijo>();
		ObservableCollection<Costo_variable> _listaCostoVariable = new ObservableCollection<Costo_variable>();
		public ListaCostos()
		{
			InitializeComponent();
			pickerMes.ItemsSource = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Noviembre", "Diciembre" }; 
			pickerYear.ItemsSource = new List<string> { "2021", "2022", "2023", "2024", "2025" };
			txtTitulo.Text = DateTime.Now.ToString("MMMM yyyy").ToUpper();
			DateTime fechaMesAct = DateTime.Today;
			_mesQuery = Convert.ToInt32(fechaMesAct.ToString("MM"));
			_yearQuery = Convert.ToInt32(fechaMesAct.ToString("yyyy"));
			GetData();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					stkCV.Children.Clear();
					stkCF.Children.Clear();
					txtTotalCF.Text = "0";
					txtTotalCV.Text = "0";
					_totalCF = 0;
					_totalCV = 0;
					GetAltura();
					GetCostoVariable();
					await Task.Delay(400);
					GetCostoFijo();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetAltura()
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

					if (dataCostoFijo != null)
					{
						foreach (var item in dataCostoFijo)
						{
							_listaCostoFijo.Add(item);
						}
						_medidaStkFijo = dataCostoFijo.Count;
						await Task.Delay(200);
					}
					txtTotalCF.Text = _totalCF.ToString();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
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
						foreach (var item in dataCostoVar)
						{
							_listaCostoVariable.Add(item);
						}
						_medidaStkVariable = dataCostoVar.Count;
						await Task.Delay(200);
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
				}
				if(_medidaStkFijo > _medidaStkVariable)
				{
					_medidaStkFijo = _medidaStkFijo * 60;
					stkCF.HeightRequest = _medidaStkFijo;
					stkCV.HeightRequest = _medidaStkVariable;
				}
				else if(_medidaStkVariable > _medidaStkFijo)
				{
					_medidaStkVariable = _medidaStkVariable * 60;
					stkCF.HeightRequest = _medidaStkFijo;
					stkCV.HeightRequest = _medidaStkVariable;
				}
				await Task.Delay(400);
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private void GetData()
		{
			DateTime fechaMesAct = DateTime.Today;
			DateTime primerDiaMesAct = new DateTime(fechaMesAct.Year, fechaMesAct.Month, 1);
			DateTime ultimoDiaMesAct = primerDiaMesAct.AddMonths(1).AddDays(-1);
			_fecha_inicial = primerDiaMesAct.ToString("yyyy-MM-dd");
			_fecha_final = ultimoDiaMesAct.ToString("yyyy-MM-dd");
			_mesDefault = fechaMesAct.ToString("MM");
		}
		private async void GetCostoFijo()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(400);
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
							StackLayout _stk1CF = new StackLayout();
							stkCF.Children.Add(_stk1CF);

							Label _labelNombreCF = new Label();
							_labelNombreCF.Text = "Nombre: " + item.nombre_cf;
							_labelNombreCF.TextColor = Color.Black;
							_labelNombreCF.FontSize = 14;
							_labelNombreCF.HorizontalTextAlignment = TextAlignment.Start;
							_stk1CF.Children.Add(_labelNombreCF);

							Label _labelMontoCF = new Label();
							_labelMontoCF.Text = "Monto: " + item.monto_cf;
							_labelMontoCF.TextColor = Color.Black;
							_labelMontoCF.FontSize = 14;
							_labelMontoCF.HorizontalTextAlignment = TextAlignment.Start;
							_stk1CF.Children.Add(_labelMontoCF);

							BoxView _bx1CF = new BoxView();
							_bx1CF.HeightRequest = 1;
							_bx1CF.Color = Color.FromHex("#465B70");
							_bx1CF.HorizontalOptions = LayoutOptions.FillAndExpand;
							_stk1CF.Children.Add(_bx1CF);

							_totalCF = _totalCF + item.monto_cf;
						}
					}
					txtTotalCF.Text = _totalCF.ToString();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetCostoVariable()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Task.Delay(400);
				try
				{
					_listaCostoVariable.Clear();
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
							StackLayout _stk1CV = new StackLayout();
							stkCV.Children.Add(_stk1CV);

							Label _labelNombreCV = new Label();
							_labelNombreCV.Text = "Nombre: " + item.nombre_cv;
							_labelNombreCV.TextColor = Color.Black;
							_labelNombreCV.FontSize = 14;
							_labelNombreCV.HorizontalTextAlignment = TextAlignment.Start;
							_stk1CV.Children.Add(_labelNombreCV);

							Label _labelMontoCV = new Label();
							_labelMontoCV.Text = "Monto: " + item.monto_cv;
							_labelMontoCV.TextColor = Color.Black;
							_labelMontoCV.FontSize = 14;
							_labelMontoCV.HorizontalTextAlignment = TextAlignment.Start;
							_stk1CV.Children.Add(_labelMontoCV);

							BoxView _bx1CV = new BoxView();
							_bx1CV.HeightRequest = 1;
							_bx1CV.Color = Color.FromHex("#465B70");
							_bx1CV.HorizontalOptions = LayoutOptions.FillAndExpand;
							_stk1CV.Children.Add(_bx1CV);

							_totalCV = _totalCV + item.monto_cv;
						}
					}
					txtTotalCV.Text = _totalCV.ToString();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void toolbarCF_Clicked(object sender, EventArgs e)
		{
			await Shell.Current.Navigation.PushAsync(new ListaCostoFijo(), true);
		}
		private async void toolbarCV_Clicked(object sender, EventArgs e)
		{
			await Shell.Current.Navigation.PushAsync(new ListaCostoVariable(), true);
		}
		private void pickerMes_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;
			if (selectedIndex != -1)
			{
				_mesElegido = picker.Items[selectedIndex];
			}
			string str = _mesElegido;
			switch (str)
			{
				case "Enero":
					_mesQuery = 1;
					break;
				case "Febrero":
					_mesQuery = 2;
					break;
				case "Marzo":
					_mesQuery = 3;
					break;
				case "Abril":
					_mesQuery = 4;
					break;
				case "Mayo":
					_mesQuery = 5;
					break;
				case "Junio":
					_mesQuery = 6;
					break;
				case "Julio":
					_mesQuery = 7;
					break;
				case "Agosto":
					_mesQuery = 8;
					break;
				case "Septiembre":
					_mesQuery = 9;
					break;
				case "Octubre":
					_mesQuery = 10;
					break;
				case "Noviembre":
					_mesQuery = 11;
					break;
				case "Diciembre":
					_mesQuery = 12;
					break;
				default:
					_mesQuery = Convert.ToInt32(_mesDefault);
					break;
			}
		}
		private void pickerYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;
			if (selectedIndex != -1)
			{
				_yearElegido = picker.Items[selectedIndex];
			}
			_yearQuery = Convert.ToInt32(_yearElegido);
		}
		private async void btnBuscar_Clicked(object sender, EventArgs e)
		{
			stkCV.Children.Clear();
			stkCF.Children.Clear();
			txtTotalCF.Text = "0";
			txtTotalCV.Text = "0";
			_totalCF = 0;
			_totalCV = 0;
			GetAltura();
			GetCostoVariable();
			await Task.Delay(400);
			GetCostoFijo();
		}
	}
}