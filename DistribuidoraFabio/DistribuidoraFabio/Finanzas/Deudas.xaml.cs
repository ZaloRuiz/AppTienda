using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
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
	public partial class Deudas : TabbedPage
	{
		ObservableCollection<VentasNombre> _listaDeudasPorCobrar = new ObservableCollection<VentasNombre>();
		ObservableCollection<ReporteEnvases> _listaDeudasEnvases = new ObservableCollection<ReporteEnvases>();
		List<string> list_DxC = new List<string>();
		List<string> list_DE = new List<string>();
		public Deudas()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					GetDeudasXCobrar();
					Task.Delay(400);
					GetDeudasEnvases();
					MessagingCenter.Subscribe<DevolverEnvases>(this, "Hi", (sender) =>
					{
						GetDeudasEnvases();
					});
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
		private async void GetDeudasXCobrar()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				_listaDeudasPorCobrar.Clear();
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaDeudasPorCobrar.php");
					var lista_duedas = JsonConvert.DeserializeObject<List<VentasNombre>>(response);
					foreach (var item in lista_duedas)
					{
						_listaDeudasPorCobrar.Add(item);
					}
					listCuentas.ItemsSource = _listaDeudasPorCobrar;
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
		private async void GetDeudasEnvases()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				_listaDeudasEnvases.Clear();
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/reportes/listaDuedasEnvases.php");
					var lista_envases = JsonConvert.DeserializeObject<List<ReporteEnvases>>(response);
					foreach (var item in lista_envases)
					{
						_listaDeudasEnvases.Add(item);
					}
					listEnvases.ItemsSource = _listaDeudasEnvases;
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
		private void listCuentas_ItemTapped(object sender, ItemTappedEventArgs e)
		{

		}
		private async void filtrarCliente_Clicked(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in _listaDeudasPorCobrar)
				{
					list_DxC.Add(item.nombre_cliente);
				}
				IEnumerable<string> array_C = list_DxC.Distinct<string>();
				string _c_elegido = await DisplayActionSheet("Elija un cliente", null, null, array_C.ToArray());
				if (_c_elegido != null)
				{
					listCuentas.ItemsSource = _listaDeudasPorCobrar.Where(x => x.nombre_cliente.ToLower().Contains(_c_elegido.ToLower()));
				}
				else
				{
					listCuentas.ItemsSource = _listaDeudasPorCobrar;
				}
			}
			catch (Exception err)
			{

			}
		}
		private async void listEnvases_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			try
			{
				var detalles = e.Item as ReporteEnvases;
				App._idDVenvase = detalles.id_dv;
				App._envasesDeuda = detalles.envases;
				await PopupNavigation.Instance.PushAsync(new DevolverEnvases());
			}
			catch (Exception err)
			{
				await DisplayAlert("Error", err.ToString(), "OK");
			}
		}
		private async void filtrarClienteEnvases_Clicked(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in _listaDeudasEnvases)
				{
					list_DE.Add(item.nombre_cliente);
				}
				IEnumerable<string> array_C = list_DE.Distinct<string>();
				string _c_elegido = await DisplayActionSheet("Elija un cliente", null, null, array_C.ToArray());
				if (_c_elegido != null)
				{
					listEnvases.ItemsSource = _listaDeudasEnvases.Where(x => x.nombre_cliente.ToLower().Contains(_c_elegido.ToLower()));
				}
				else
				{
					listEnvases.ItemsSource = _listaDeudasEnvases;
				}
			}
			catch (Exception err)
			{

			}
		}
	}
}