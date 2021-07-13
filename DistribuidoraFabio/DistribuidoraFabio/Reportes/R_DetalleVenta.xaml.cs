using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using DistribuidoraFabio.Service;
using DistribuidoraFabio.ViewModels;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Reportes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class R_DetalleVenta : ContentPage
	{
		DateTime _fechaInicio;
		DateTime _fechaFinal;
		ObservableCollection<_RDetalleVenta> _reporteDV = new ObservableCollection<_RDetalleVenta>();
		public R_DetalleVenta()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				_fechaInicio = DateTime.Today.AddDays(-1);
				_fechaFinal = DateTime.Today;
				try
				{
					GetDatos();
					BindingContext = new R_DetalleVentaVM();
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
		private async void GetDatos()
		{
			try
			{
				string BusyReason = "Cargando...";
				await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
				_RDetalleVenta _R_detalleVenta = new _RDetalleVenta()
				{
					fecha_inicio = _fechaInicio,
					fecha_final = _fechaFinal
				};
				var json = JsonConvert.SerializeObject(_R_detalleVenta);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpClient client = new HttpClient();
				var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteDetalleVenta.php", content);

				var jsonR = await result.Content.ReadAsStringAsync();
				var _dataRDV = JsonConvert.DeserializeObject<List<_RDetalleVenta>>(jsonR);
				listData.ItemsSource = null;
				listData.ItemsSource = _dataRDV;
				await PopupNavigation.Instance.PopAsync();
			}
			catch (Exception err)
			{
				await DisplayAlert("Error", err.ToString(), "OK");
			}
		}
		private void toolbarFiltro_Clicked(object sender, EventArgs e)
		{
			//await PopupNavigation.Instance.PushAsync(new FiltrarPorFecha());
			if(stkFiltro.IsVisible == true)
			{
				stkFiltro.IsVisible = false;
			}
			if (stkFiltro.IsVisible == false)
			{
				stkFiltro.IsVisible = true;
			}
		}
		private void btnFiltro_Clicked(object sender, EventArgs e)
		{
			_fechaInicio = pickFechaInicio.Date.Date;
			_fechaFinal = pickFechaFinal.Date.Date;
			stkFiltro.IsVisible = false;
			GetDatos();
		}
		private async void toolbarExcel_Clicked(object sender, EventArgs e)
		{
			App._fechaInicioFiltro = _fechaInicio;
			App._fechaFinalFiltro = _fechaFinal;
			string BusyReason = "Generando Excel...";
			await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
			await Task.Delay(3500);
			await PopupNavigation.Instance.PopAsync();
		}
	}
}