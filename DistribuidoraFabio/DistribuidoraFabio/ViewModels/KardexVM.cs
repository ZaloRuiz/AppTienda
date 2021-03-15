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
	public class KardexVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		private bool _isRefreshing;
		private int id_del_producto;
		private string NombreProdCompleto;
		ObservableCollection<Models.InventarioNombre> inventario_list = new ObservableCollection<Models.InventarioNombre>();
		public ObservableCollection<Models.InventarioNombre> InventariosKardex
		{
			get { return inventario_list; }
			set
			{
				if (inventario_list != value)
				{
					inventario_list = value;
					OnPropertyChanged("InventariosKardex");
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
		private async void CmdRefresh()
		{
			IsRefreshing = true;
			await Task.Delay(2000);
			IsRefreshing = false;
		}
		public KardexVM(int id_producto)
		{
			id_del_producto = id_producto;
			inventario_list = new ObservableCollection<Models.InventarioNombre>();
			GetKardex();
			RefreshCommand = new Command(CmdRefresh);
		}
		public async void GetKardex()
		{
			try
			{
				HttpClient client = new HttpClient();
				var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/inventarios/listaInventarioNombre.php");
				var kardex = JsonConvert.DeserializeObject<List<Models.InventarioNombre>>(response);

				foreach (var item in kardex)
				{
					if (id_del_producto == item.id_producto)
					{
						inventario_list.Add(new Models.InventarioNombre
						{
							id_inventario = item.id_inventario,
							id_producto = item.id_producto,
							nombre = item.nombre,
							nombre_tipo_producto = item.nombre_tipo_producto,
							nombre_sub_producto = item.nombre_sub_producto,
							fecha_inv = item.fecha_inv,
							numero_factura = item.numero_factura,
							detalle = item.detalle,
							precio_compra = item.precio_compra,
							unidades = item.unidades,
							entrada_fisica = item.entrada_fisica,
							salida_fisica = item.salida_fisica,
							saldo_fisica = item.saldo_fisica,
							entrada_valorado = item.entrada_valorado,
							salida_valorado = item.salida_valorado,
							saldo_valorado = item.saldo_valorado,
							promedio = item.promedio
						});
					}
				}
			}
			catch(Exception err)
			{
				Console.WriteLine("###################################################" + err.ToString());
			}
		}
	}
}
