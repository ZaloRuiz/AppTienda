using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarCostoVariable : ContentPage
	{
		private int _mesQuery;
		private string _mesElegido;
		private int _yearQuery;
		private string _mesDefault;
		private int _yearActual;
		private string _fechaElegida;
		public AgregarCostoVariable()
		{
			InitializeComponent();
			pickerMes.ItemsSource = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Noviembre", "Diciembre"};
		}
		protected override void OnAppearing()
		{
			DateTime fechaMesAct = DateTime.Today;
			_yearActual = Convert.ToInt32(fechaMesAct.ToString("yyyy"));
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
		private async void btnGuardar_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(entryNombre.Text) || (!string.IsNullOrEmpty(entryNombre.Text)))
				{
					if (!string.IsNullOrWhiteSpace(entrymonto.Text) || (!string.IsNullOrEmpty(entrymonto.Text)))
					{
						if (_mesQuery != 0)
						{
							if (!string.IsNullOrWhiteSpace(entryDescripcion.Text) || (!string.IsNullOrEmpty(entryDescripcion.Text)))
							{
								if (!string.IsNullOrWhiteSpace(entryTipoGasto.Text) || (!string.IsNullOrEmpty(entryTipoGasto.Text)))
								{
									_fechaElegida = pickerFecha.Date.ToString("yyyy-MM-dd");
									if (!string.IsNullOrWhiteSpace(_fechaElegida) || (!string.IsNullOrEmpty(_fechaElegida)))
									{
										string BusyReason = "Agregando...";
										await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
										try
										{
											Costo_variable _costoVariable = new Costo_variable()
											{
												nombre_cv = entryNombre.Text,
												monto_cv = Convert.ToDecimal(entrymonto.Text),
												fecha_cv = Convert.ToDateTime(_fechaElegida),
												mes_cv = _mesQuery,
												gestion_cv = _yearActual,
												descripcion_cv = entryDescripcion.Text,
												tipo_gasto_cv = entryTipoGasto.Text
											};
											var json = JsonConvert.SerializeObject(_costoVariable);
											var content = new StringContent(json, Encoding.UTF8, "application/json");
											HttpClient client = new HttpClient();
											var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/agregarCostoVariable.php", content);
											if (result.StatusCode == HttpStatusCode.OK)
											{
												await PopupNavigation.Instance.PopAsync();
												await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
												await Navigation.PopAsync();
											}
											else
											{
												await PopupNavigation.Instance.PopAsync();
												await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
												await Navigation.PopAsync();
											}
										}
										catch (Exception err)
										{
											await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
										}
									}
								}
								else
								{
									await DisplayAlert("Campo vacio", "El campo de Tipo de gasto esta vacio", "Ok");
								}
							}
							else
							{
								await DisplayAlert("Campo vacio", "El campo de Descripcion esta vacio", "Ok");
							}
						}
						else
						{
							await DisplayAlert("Campo vacio", "El campo de Mes esta vacio", "Ok");
						}
					}
					else
					{
						await DisplayAlert("Campo vacio", "El campo de Monto esta vacio", "Ok");
					}
				}
				else
				{
					await DisplayAlert("Campo vacio", "El campo de Nombre esta vacio", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}