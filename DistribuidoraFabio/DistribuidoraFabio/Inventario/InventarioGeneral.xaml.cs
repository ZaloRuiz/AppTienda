using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Inventario
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventarioGeneral : ContentPage
	{
		private int _sumaCantidad = 0;
		private decimal _sumaBs = 0;
		public InventarioGeneral()
		{
			InitializeComponent();
			this.BindingContext = new InventarioGeneralVM();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					BindingContext = new InventarioGeneralVM();
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var producto_lista = JsonConvert.DeserializeObject<List<Models.ProductoNombre>>(response);
					foreach (var item in producto_lista)
					{
						_sumaCantidad = _sumaCantidad + item.stock;
						_sumaBs = _sumaBs + item.stock_valorado;
					}
					txtTotalBs.Text = _sumaBs.ToString() + " Bs.";
					txtTotalCantidad.Text = _sumaCantidad.ToString();
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
	}
}