using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaR_MutiProductos : PopupPage
	{
		public ListaR_MutiProductos ()
		{
			InitializeComponent ();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			GetProductos();
		}
		private async void GetProductos()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var producto_lista = JsonConvert.DeserializeObject<List<Models.ProductoNombre>>(response);
					if (producto_lista != null)
					{
						listProductos.ItemsSource = producto_lista;
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
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
			await PopupNavigation.Instance.PopAllAsync();
		}
	}
}