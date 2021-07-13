using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using DistribuidoraFabio.Service;
using DistribuidoraFabio.Models;

namespace DistribuidoraFabio.ViewModels
{
	public class R_DetalleVentaVM : INotifyPropertyChanged
	{
		public ICommand ExportToExcelCommand { private set; get; }
		private ExcelServices excelService;
		public ObservableCollection<_RDetalleVenta> rDetalleVentas;
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		private bool _isRefreshing;
		ObservableCollection<Models._RDetalleVenta> _reporteDV = new ObservableCollection<Models._RDetalleVenta>();
		public ObservableCollection<Models._RDetalleVenta> ReportesDVs
		{
			get { return _reporteDV; }
			set
			{
				if (_reporteDV != value)
				{
					_reporteDV = value;
					OnPropertyChanged("ReportesDVs");
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
			await Task.Delay(1500);
			IsRefreshing = false;
		}
		public R_DetalleVentaVM()
		{
			_reporteDV = new ObservableCollection<Models._RDetalleVenta>();
			RefreshCommand = new Command(CmdRefresh);
			ExportToExcelCommand = new Command(async () => await ExportToExcel());
			excelService = new ExcelServices();
		}
		async Task ExportToExcel()
		{
			try
			{
				_RDetalleVenta _R_detalleVenta = new _RDetalleVenta()
				{
					fecha_inicio = App._fechaInicioFiltro,
					fecha_final = App._fechaFinalFiltro
				};
				var json = JsonConvert.SerializeObject(_R_detalleVenta);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpClient client = new HttpClient();
				var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/ReporteDetalleVenta.php", content);

				var jsonR = await result.Content.ReadAsStringAsync();
				var _dataRDV = JsonConvert.DeserializeObject<List<_RDetalleVenta>>(jsonR);

				foreach (var item in _dataRDV)
				{
					_reporteDV.Add(new Models._RDetalleVenta
					{
						id_venta = item.id_venta,
						nombre = item.nombre,
						fecha = item.fecha,
						codigo_c = item.codigo_c,
						nombre_cliente = item.nombre_cliente,
						razon_social = item.razon_social,
						nit = item.nit,
						telefono = item.telefono,
						direccion_cliente = item.direccion_cliente,
						geolocalizacion = item.geolocalizacion,
						nombre_producto = item.nombre_producto,
						precio_producto = item.precio_producto,
						cantidad = item.cantidad,
						sub_total = item.sub_total,
						envases = item.envases,
						tipo_venta = item.tipo_venta,
						saldo = item.saldo,
						estado = item.estado
					});
				}
			}
			catch (Exception err)
			{
				Console.WriteLine("###################################################" + err.ToString());
			}

			await Task.Delay(1000);
			string fechahoy = DateTime.Today.ToString("dd-MM-yyyy");
			string fecha = "DetalleDeVenta_" + fechahoy + ".xlsx";
			string filePath = excelService.GenerateExcel(fecha);
			var header = new List<string>() { "ID", "Nombre", "Fecha", "Codigo Cliente", "Nombre Cliente", "Razon Social", "Nit", "Telefono", "Direccion","Geolocalizacion","Producto", "Precio", "Cantidad",
			"Sub Total", "Envases" , "Tipo Venta", "Saldo", "Estado"};
			var data = new ExcelStructure();
			data.Headers = header;
			foreach (var publication in _reporteDV)
			{
				var row = new List<string>()
				{
					publication.id_venta.ToString(),
					publication.nombre,
					publication.fecha.ToString(),
					publication.codigo_c.ToString(),
					publication.nombre_cliente,
					publication.razon_social,
					publication.nit.ToString(),
					publication.telefono.ToString(),
					publication.direccion_cliente,
					publication.geolocalizacion,
					publication.nombre_producto,
					publication.precio_producto.ToString(),
					publication.cantidad.ToString(),
					publication.sub_total.ToString(),
					publication.envases.ToString(),
					publication.tipo_venta,
					publication.saldo.ToString(),
					publication.estado,
				};
				data.Values.Add(row);
			}
			excelService.InsertDataIntoSheet(filePath, "Publications", data);
			await Launcher.OpenAsync(new OpenFileRequest()
			{
				File = new ReadOnlyFile(filePath)
			});
		}
	}
}
