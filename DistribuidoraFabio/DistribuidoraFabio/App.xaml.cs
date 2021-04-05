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
		}
		public static ObservableCollection<DetalleVenta_previo> _detalleVData = new ObservableCollection<DetalleVenta_previo>();
		public static ObservableCollection<DetalleVenta_previo> _DetalleVentaData { get { return _detalleVData; } }
		public static ObservableCollection<DetalleCompra_previo> _detalleCData = new ObservableCollection<DetalleCompra_previo>();
		public static ObservableCollection<DetalleCompra_previo> _DetalleCompraData { get { return _detalleCData; } }
		public static ObservableCollection<DetalleVentaNombre> _detalleventaguardar = new ObservableCollection<DetalleVentaNombre>();
		public static ObservableCollection<DetalleVentaNombre> _DetalleVentaGuardar { get { return _detalleventaguardar; } }
		public static DateTime _fechaInicioFiltro = DateTime.Today.AddYears(-5);
		public static DateTime _fechaFinalFiltro = DateTime.Now;
		public static int _idDVenvase;
		public static int _envasesDeuda;
		public static int _idVsaldo;
		public static decimal _saldoDeuda;
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
