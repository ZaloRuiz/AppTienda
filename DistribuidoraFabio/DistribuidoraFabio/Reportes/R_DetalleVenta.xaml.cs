using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public R_DetalleVenta()
		{
			InitializeComponent();
			
		}
		protected override void OnAppearing()
		{
			_fechaInicio = App._fechaInicioFiltro;
			_fechaFinal = App._fechaFinalFiltro;
			BindingContext = new R_DetalleVentaVM(_fechaInicio, _fechaFinal);
			MessagingCenter.Subscribe<FiltrarPorFecha>(this, "Hi", (sender) =>
			{
				Navigation.PopAsync();
				_fechaInicio = App._fechaInicioFiltro;
				_fechaFinal = App._fechaFinalFiltro;
				BindingContext = new R_DetalleVentaVM(_fechaInicio, _fechaFinal);
			});
		}
		private async void toolbarFiltro_Clicked(object sender, EventArgs e)
		{
			//Rg.Plugins.Popup
			await PopupNavigation.Instance.PushAsync(new FiltrarPorFecha());
		}
	}
}