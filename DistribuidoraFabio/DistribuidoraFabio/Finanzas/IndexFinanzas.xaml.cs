using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IndexFinanzas : ContentPage
	{
		public IndexFinanzas()
		{
			InitializeComponent();
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new BalanceMensual());
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new Deudas());
		}

		private async void Button_Clicked_2(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ListaCostos());
		}

		private async void Button_Clicked_3(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new BalanceAnual());
		}
	}
}