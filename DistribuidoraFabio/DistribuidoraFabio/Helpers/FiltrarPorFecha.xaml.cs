using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FiltrarPorFecha : PopupPage
	{
		public FiltrarPorFecha()
		{
			InitializeComponent();
		}
		protected override void OnDisappearing()
		{

		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
		protected override bool OnBackgroundClicked()
		{
			return false;
		}

		private async void btnCancelar_Clicked(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}

		private async void btnFiltrar_Clicked(object sender, EventArgs e)
		{
			try
			{
				App._fechaInicioFiltro = entryF_inicio.Date;
				App._fechaFinalFiltro = entryF_final.Date;
				MessagingCenter.Send<FiltrarPorFecha>(this, "Hi");
				
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", err.ToString(), "OK");
			}
			await PopupNavigation.Instance.PopAllAsync();
		}
	}
}