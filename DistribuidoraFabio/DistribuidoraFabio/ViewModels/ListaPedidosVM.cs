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
	public class ListaPedidosVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		private ObservableCollection<VentasNombre> _listaPedidosEnt;
		public ObservableCollection<VentasNombre> ListaPedidosEnt
		{
			get { return _listaPedidosEnt; }
			set
			{
				if (_listaPedidosEnt != value)
				{
					_listaPedidosEnt = value;
					OnPropertyChanged("ListaPedidosEnt");
				}
			}
		}
		private ObservableCollection<VentasNombre> _listaPedidosPen;
		public ObservableCollection<VentasNombre> ListaPedidosPen
		{
			get { return _listaPedidosPen; }
			set
			{
				if (_listaPedidosPen != value)
				{
					_listaPedidosPen = value;
					OnPropertyChanged("ListaPedidosPen");
				}
			}
		}
		private ObservableCollection<VentasNombre> _listaPedidosCanc;
		public ObservableCollection<VentasNombre> ListaPedidosCanc
		{
			get { return _listaPedidosCanc; }
			set
			{
				if (_listaPedidosCanc != value)
				{
					_listaPedidosCanc = value;
					OnPropertyChanged("ListaPedidosCanc");
				}
			}
		}
		public ListaPedidosVM()
		{
			_listaPedidosEnt = new ObservableCollection<VentasNombre>();
			_listaPedidosPen = new ObservableCollection<VentasNombre>();
			_listaPedidosCanc = new ObservableCollection<VentasNombre>();
			GetPedidos();
		}
		public async void GetPedidos()
		{
			try
			{
				HttpClient client = new HttpClient();
				var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaVentaNombre.php");
				var lista_ventas = JsonConvert.DeserializeObject<List<VentasNombre>>(response);
				foreach (var item in lista_ventas)
				{
					if (item.estado == "Entregado")
					{
						_listaPedidosEnt.Add(item);
					}
					else if (item.estado == "Pendiente")
					{
						_listaPedidosPen.Add(item);
					}
					else if (item.estado == "Cancelado")
					{
						_listaPedidosCanc.Add(item);
					}
				}
			}
			catch (Exception err)
			{
				Console.WriteLine("#######################################" + err.ToString() + "#################################################");
			}
		}
	}
}
