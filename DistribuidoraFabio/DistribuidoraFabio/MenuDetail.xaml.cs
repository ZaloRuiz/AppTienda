using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuDetail : ContentPage
	{
		public MenuDetail()
		{
			InitializeComponent();

		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{

				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}