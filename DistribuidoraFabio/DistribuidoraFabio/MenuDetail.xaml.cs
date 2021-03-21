using DistribuidoraFabio.Models;
using Newtonsoft.Json;
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
			try
			{
				
			}
			catch(Exception err)
			{
				await DisplayAlert("AVISO", err.ToString(), "OK");
			}
		}
	}
}