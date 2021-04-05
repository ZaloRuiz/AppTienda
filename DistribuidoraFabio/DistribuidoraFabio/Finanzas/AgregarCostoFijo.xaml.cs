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
	public partial class AgregarCostoFijo : ContentPage
	{
		private int _cantMeses = 0;
		private int _mesCF = 0;
		private int _gestionCF = 0;
		private string _fechaHoy;
		private int _yearActual;
		private int _mesActual;
		private int _yearSiguiente;
		private int _mesInicio;
		private int _mesesFaltantes;
		public AgregarCostoFijo()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			DateTime fechaMesAct = DateTime.Today;
			_fechaHoy = fechaMesAct.ToString("yyyy-MM-dd");
			_yearActual = Convert.ToInt32(fechaMesAct.ToString("yyyy"));
			_mesActual = Convert.ToInt32(fechaMesAct.ToString("MM"));
			_mesesFaltantes = 13 - _mesActual;
			_yearSiguiente = _yearActual + 1;
			_mesInicio = 1;
		}
		private async void btnGuardar_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(entryNombre.Text) || (!string.IsNullOrEmpty(entryNombre.Text)))
				{
					if (!string.IsNullOrWhiteSpace(entrymonto.Text) || (!string.IsNullOrEmpty(entrymonto.Text)))
					{
						if (!string.IsNullOrWhiteSpace(entryCantMeses.Text) || (!string.IsNullOrEmpty(entryCantMeses.Text)))
						{
							if (!string.IsNullOrWhiteSpace(entryDescripcion.Text) || (!string.IsNullOrEmpty(entryDescripcion.Text)))
							{
								if (!string.IsNullOrWhiteSpace(entryTipoGasto.Text) || (!string.IsNullOrEmpty(entryTipoGasto.Text)))
								{
									_cantMeses = Convert.ToInt32(entryCantMeses.Text);
									string BusyReason = "Agregando...";
									await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
									for (int i = 1; i <= _cantMeses; i++)
									{
										if (i <= _mesesFaltantes)
										{
											try
											{
												Costo_fijo _costoFijo = new Costo_fijo()
												{
													nombre_cf = entryNombre.Text,
													monto_cf = Convert.ToDecimal(entrymonto.Text),
													fecha_cf = Convert.ToDateTime(_fechaHoy),
													mes_cf = _mesActual,
													gestion_cf = _yearActual,
													descripcion_cf = entryDescripcion.Text,
													tipo_gasto_cf = entryTipoGasto.Text
												};
												var json = JsonConvert.SerializeObject(_costoFijo);
												var content = new StringContent(json, Encoding.UTF8, "application/json");
												HttpClient client = new HttpClient();
												var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/agregarCostoFijo.php", content);
											}
											catch (Exception err)
											{
												await DisplayAlert("Error", err.ToString(), "OK");
											}
											_mesActual = _mesActual + 1;
										}
										else if (i > _mesesFaltantes)
										{
											try
											{
												Costo_fijo _costoFijo = new Costo_fijo()
												{
													nombre_cf = entryNombre.Text,
													monto_cf = Convert.ToDecimal(entrymonto.Text),
													fecha_cf = Convert.ToDateTime(_fechaHoy),
													mes_cf = _mesInicio,
													gestion_cf = _yearSiguiente,
													descripcion_cf = entryDescripcion.Text,
													tipo_gasto_cf = entryTipoGasto.Text
												};
												var json = JsonConvert.SerializeObject(_costoFijo);
												var content = new StringContent(json, Encoding.UTF8, "application/json");
												HttpClient client = new HttpClient();
												var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/agregarCostoFijo.php", content);
											}
											catch (Exception err)
											{
												await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
											}
											_mesInicio = _mesInicio + 1;
										}
									}
									await PopupNavigation.Instance.PopAsync();
									await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
									await Navigation.PopAsync();
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
							await DisplayAlert("Campo vacio", "El campo de Cantidad de meses esta vacio", "Ok");
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