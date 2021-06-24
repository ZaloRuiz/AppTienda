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
	public partial class R_VentaDiaria : ContentPage
	{
		private DateTime _fechaElegida;
		public R_VentaDiaria()
		{
			InitializeComponent();
			_fechaElegida = DateTime.Today;
		}

		private async void btnReporte_Clicked(object sender, EventArgs e)
		{
			_fechaElegida = datePickerFecha.Date;
			await PopupNavigation.Instance.PushAsync(new ListaR_VentaDiaria(_fechaElegida));
		}
	}
}