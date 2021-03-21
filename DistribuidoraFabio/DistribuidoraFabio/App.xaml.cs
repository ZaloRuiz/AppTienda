using DistribuidoraFabio.Models;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio
{
	public partial class App : Application
	{
		public static NavigationPage NavigationPage { get; private set; }
		public App()
		{
			InitializeComponent();
			MainPage = new Menu();
			//MainPage = new NavigationPage(new Menu());
			var seconds = TimeSpan.FromSeconds(1);
			Xamarin.Forms.Device.StartTimer(seconds,
				() =>
				{
					bool result = false;
					return result;  
					CheckConnection();
					
				});
		}
		public static ObservableCollection<DetalleVenta_previo> _detalleVData = new ObservableCollection<DetalleVenta_previo>();
		public static ObservableCollection<DetalleVenta_previo> _DetalleVentaData { get { return _detalleVData; } }
		public static ObservableCollection<DetalleCompra_previo> _detalleCData = new ObservableCollection<DetalleCompra_previo>();
		public static ObservableCollection<DetalleCompra_previo> _DetalleCompraData { get { return _detalleCData; } }
		public static DateTime _fechaInicioFiltro = DateTime.Today.AddYears(-5);
		public static DateTime _fechaFinalFiltro = DateTime.Now;
		private async void CheckConnection()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{

				await App.Current.MainPage.DisplayAlert("AVISO", "No esta conectado a internet", "OK");
				//await new NavigationPage().PushAsync(new MenuDetail());
			}
			else
			{
				return;
			}
		}
		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
