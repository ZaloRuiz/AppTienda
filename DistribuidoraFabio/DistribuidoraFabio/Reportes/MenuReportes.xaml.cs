using DistribuidoraFabio.Helpers;
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
	public partial class MenuReportes : ContentPage
	{
		public MenuReportes()
		{
			InitializeComponent();
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new R_VentaDiaria());
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PushAsync(new ListaR_InventarioDia());
		}

		private async void Button_Clicked_2(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new R_DetalleVenta());
		}

		private async void Button_Clicked_3(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new GraficoVentas());
		}
	}
}