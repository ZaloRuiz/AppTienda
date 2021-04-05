using DistribuidoraFabio.Models;
using DistribuidoraFabio.ViewModels;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Producto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaProducto : ContentPage
	{
		private int _estadoNFiltro = 0;
		private int _estadoSFiltro = 0;
		ObservableCollection<ProductoNombre> _listProdNom = new ObservableCollection<ProductoNombre>();
		public ListaProducto()
		{
			InitializeComponent();
		}
		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarProducto());
		}
		private void ToolbarItemTP_Clicked(object sender, EventArgs e)
		{
			 Navigation.PushAsync(new ListaTipoProducto());
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					_listProdNom.Clear();
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var productos = JsonConvert.DeserializeObject<List<ProductoNombre>>(response);

					foreach (var item in productos)
					{
						_listProdNom.Add(item);
					}
					listaProd.ItemsSource = _listProdNom;
					btnOrdNombre.Clicked += (sender, args) => FiltrarNombre();
					btnOrdStock.Clicked += (sender, args) => FiltrarStock();
				}
				catch (HttpRequestException err)
				{
					await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private void FiltrarNombre()
		{
			if(_estadoNFiltro == 0)
			{
				_estadoNFiltro = 1;
				listaProd.ItemsSource = _listProdNom.OrderBy(x => x.nombre_producto).ToList();
			}
			else if(_estadoNFiltro == 1)
			{
				_estadoNFiltro = 0;
				listaProd.ItemsSource = _listProdNom.OrderByDescending(x => x.nombre_producto).ToList();
			}
		}
		private void FiltrarStock()
		{
			if (_estadoSFiltro == 0)
			{
				_estadoSFiltro = 1;
				listaProd.ItemsSource = _listProdNom.OrderBy(x => x.stock).ToList();
			}
			else if (_estadoSFiltro == 1)
			{
				_estadoSFiltro = 0;
				listaProd.ItemsSource = _listProdNom.OrderByDescending(x => x.stock).ToList();
			}
		}
		private async void OnItemSelected(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as Models.ProductoNombre;
			await Navigation.PushAsync(new EditarBorrarProducto(detalles.id_producto, detalles.nombre_producto, detalles.nombre_tipo_producto, detalles.stock,
				detalles.stock_valorado, detalles.promedio, detalles.precio_venta, detalles.producto_alerta));
		}
		private void btnOrdenar_Clicked(object sender, EventArgs e)
		{
			if(btnOrdNombre.IsVisible == false)
			{
				btnOrdStock.IsVisible = true;
				btnOrdNombre.IsVisible = true;
			}
			else if(btnOrdNombre.IsVisible == true)
			{
				btnOrdNombre.IsVisible = false;
				btnOrdStock.IsVisible = false;
			}
		}
	}
}