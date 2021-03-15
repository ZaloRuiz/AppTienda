using DistribuidoraFabio.Inventario;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DistribuidoraFabio.ViewModels
{
	public class InventarioGeneralVM : INotifyPropertyChanged
	{
		private Models.ProductoNombre _selectedProducto;
		private bool _isRefreshing;
		private ObservableCollection<Models.ProductoNombre> _listaDeProducto;
		public ObservableCollection<Models.ProductoNombre> ListaDeProducto
		{
			get { return _listaDeProducto; }
			set
			{
				if (_listaDeProducto != value)
				{
					_listaDeProducto = value;
					OnPropertyChanged("ListaDeProducto");
				}
			}
		}
		public Models.ProductoNombre SelectedProducto
		{
			get
			{
				return _selectedProducto;
			}
			set
			{
				_selectedProducto = value;
				OnPropertyChanged(nameof(SelectedProducto));
				try
				{
					if (_selectedProducto != null)
					{
						MasterDetailPage masterDetail = (MasterDetailPage)Application.Current.MainPage; 
						masterDetail.Detail.Navigation.PushAsync(new Kardex( _selectedProducto.id_producto));
					}
				}
				catch (Exception err)
				{
					Console.WriteLine("###########################################" + err.ToString());
				}
			}
		}
		public bool IsRefreshing
		{
			get
			{
				return _isRefreshing;
			}
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}
		public ICommand RefreshCommand { get; set; }
		public InventarioGeneralVM()
		{
			_listaDeProducto = new ObservableCollection<Models.ProductoNombre>();
			GetProducto();
			RefreshCommand = new Command(CmdRefresh);
		}
		public async void GetProducto()
		{
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
			var producto_lista = JsonConvert.DeserializeObject<List<Models.ProductoNombre>>(response);
			foreach (var item in producto_lista)
			{
				_listaDeProducto.Add(new Models.ProductoNombre
				{
					id_producto = item.id_producto,
					nombre_producto = item.nombre_producto,
					nombre_tipo_producto = item.nombre_tipo_producto,
					precio_venta = item.precio_venta,
					producto_alerta = item.producto_alerta,
					promedio = item.promedio,
					stock = item.stock,
					stock_valorado = item.stock_valorado
				});
			}
		}
		private async void CmdRefresh()
		{
			IsRefreshing = true;
			await Task.Delay(2000);
			IsRefreshing = false;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
	}
}
