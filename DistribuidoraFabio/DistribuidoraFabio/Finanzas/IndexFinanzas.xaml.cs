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
			await Shell.Current.Navigation.PushAsync(new BalanceMensual(), true);
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
			await Shell.Current.Navigation.PushAsync(new Deudas(), true);
		}

		private async void Button_Clicked_2(object sender, EventArgs e)
		{
			await Shell.Current.Navigation.PushAsync(new ListaCostos(), true);
		}

		private async void Button_Clicked_3(object sender, EventArgs e)
		{
			await Shell.Current.Navigation.PushAsync(new BalanceAnual(), true);
		}
	}
}