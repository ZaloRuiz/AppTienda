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
	public partial class ListaR_InventarioDia : PopupPage
	{
		public ListaR_InventarioDia ()
		{
			InitializeComponent ();
			txtTitulo.Text = "Inventario de el " + DateTime.Today.ToString("dd/MM/yyyy");
			GetProductos();
		}
		private async void GetProductos()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					txtTitulo.Text = "Inventario de el " + DateTime.Today.ToString("dd/MM/yyy");
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var producto_lista = JsonConvert.DeserializeObject<List<Models.ProductoNombre>>(response);
					if (producto_lista != null)
					{
						listData.ItemsSource = producto_lista;
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
		protected override void OnAppearing()
		{
			base.OnAppearing();
			MessagingCenter.Send(this, "allowPortrait");

		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MessagingCenter.Send(this, "preventPortrait");
		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
		protected override bool OnBackgroundClicked()
		{
			return false;
		}

		private async void btnCerrar_Clicked(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAllAsync();
		}
	}
}