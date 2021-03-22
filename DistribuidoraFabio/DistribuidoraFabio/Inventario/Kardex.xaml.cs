using DistribuidoraFabio.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Inventario
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Kardex : ContentPage
	{
		int Id_producto;
		public Kardex(int id_producto)
		{
			InitializeComponent();
			Id_producto = id_producto;
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					BindingContext = new KardexVM(Id_producto);
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}