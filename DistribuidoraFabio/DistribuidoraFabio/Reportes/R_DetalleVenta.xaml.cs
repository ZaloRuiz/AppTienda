using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Reportes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class R_DetalleVenta : ContentPage
	{
		DateTime _fechaInicio;
		DateTime _fechaFinal;
		DateTime _fecha_inicioVM;
		DateTime _fecha_finalVM;
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
					//string BusyReason = "Cargando...";
					//await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
					//_fechaInicio = App._fechaInicioFiltro;
					//_fechaFinal = App._fechaFinalFiltro;
					//BindingContext = new R_DetalleVentaVM(); 
					//MessagingCenter.Subscribe<FiltrarPorFecha>(this, "Hi", (sender) =>
					//{
					//	Navigation.PopAsync();
					//	_fechaInicio = App._fechaInicioFiltro;
					//	_fechaFinal = App._fechaFinalFiltro;
					//	BindingContext = new R_DetalleVentaVM(_fechaInicio, _fechaFinal);
					//});

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
				Console.WriteLine("###################################################" + err.ToString());
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

		private void toolbarExcel_Clicked(object sender, EventArgs e)
		{
			//_fechaInicio = App._fechaInicioFiltro;
			//_fechaFinal = App._fechaFinalFiltro;
			//BindingContext = new R_DetalleVentaVM();
		}
	}
}