using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Compra
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarCompra : ContentPage
	{
		private int _id_compra_edit = 0;
		private DateTime _fecha_edit;
		private int _numero_factura_edit = 0;
		private string _proveedor_edit;
		private decimal _saldo_edit = 0;
		private decimal _total_edit = 0;
		private int _proveedorID = 0;
		int numProd = 0;
		string BusyReason = "Cargando...";
		private decimal MontoRetirado = 0;
		public EditarBorrarCompra(int _id_compra, DateTime _fecha, int _numero_factura, string _proveedor, decimal _saldo, decimal _total)
		{
			InitializeComponent();
			_id_compra_edit = _id_compra;
			_fecha_edit = _fecha;
			_numero_factura_edit = _numero_factura;
			_proveedor_edit = _proveedor;
			_saldo_edit = _saldo;
			_total_edit = _total;
			GetLista();
			txtFactura.Text = _numero_factura.ToString();
			txtFecha.Date = _fecha;
			txtProveedor.Text = _proveedor;
			txtSaldo.Text = _saldo.ToString();
			txtTotal.Text = _total.ToString();
			GetProveedorID();
		}
		
		private async void GetLista()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					DetalleCompraNombre _detaVenta = new DetalleCompraNombre()
					{
						numero_factura = _numero_factura_edit
					};
					var json = JsonConvert.SerializeObject(_detaVenta);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/listaDetalleCompraNombre.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dc_lista = JsonConvert.DeserializeObject<List<DetalleCompraNombre>>(jsonR);
					int cantidad = dc_lista.Count();
					stkPrd.HeightRequest = cantidad * 45;
					listProductos.ItemsSource = dc_lista;
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void btnEditar_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(txtFactura.Text) || (!string.IsNullOrEmpty(txtFactura.Text)))
			{
				if (txtFecha.Date != null)
				{
					if (!string.IsNullOrWhiteSpace(txtProveedor.Text) || (!string.IsNullOrEmpty(txtProveedor.Text)))
					{
						if (!string.IsNullOrWhiteSpace(txtSaldo.Text) || (!string.IsNullOrEmpty(txtSaldo.Text)))
						{
							if (!string.IsNullOrWhiteSpace(txtTotal.Text) || (!string.IsNullOrEmpty(txtTotal.Text)))
							{
								if (_proveedorID != 0)
								{
									if (CrossConnectivity.Current.IsConnected)
									{
										try
										{
											Compras _compras = new Compras()
											{
												id_compra = _id_compra_edit,
												numero_factura = Convert.ToInt32(txtFactura.Text),
												fecha_compra = txtFecha.Date,
												id_proveedor = _proveedorID,
												saldo = Convert.ToDecimal(txtSaldo.Text),
												total = Convert.ToDecimal(txtTotal.Text)
											};

											var json = JsonConvert.SerializeObject(_compras);
											var content = new StringContent(json, Encoding.UTF8, "application/json");
											HttpClient client = new HttpClient();
											var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/editarCompra.php", content);
											if (result.StatusCode == HttpStatusCode.OK)
											{
												await DisplayAlert("OK", "Se edito correctamente", "OK");
												await Shell.Current.Navigation.PopAsync();
											}
											else
											{
												await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
												await Shell.Current.Navigation.PopAsync();
											}
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
								else
								{
									await DisplayAlert("Error", "El campo de Proovedor esta vacio", "OK");
								}
							}
							else
							{
								await DisplayAlert("Error", "El campo de Total esta vacio", "OK");
							}
						}
						else
						{
							await DisplayAlert("Error", "El campo de Saldo esta vacio", "OK");
						}
					}
					else
					{
						await DisplayAlert("Error", "El campo de Proovedor esta vacio", "OK");
					}
				}
				else
				{
					await DisplayAlert("Error", "El campo de Fecha esta vacio", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "El campo de Factura esta vacio", "OK");
			}
		}
		private async void txtProveedor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/proveedores/listaProveedor.php");
					var proveedores = JsonConvert.DeserializeObject<List<Models.Proveedor>>(response).ToList();
					foreach (var item in proveedores)
					{
						if(txtProveedor.Text == item.nombre)
						{
							_proveedorID = item.id_proveedor;
						}
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void GetProveedorID()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/proveedores/listaProveedor.php");
					var proveedores = JsonConvert.DeserializeObject<List<Models.Proveedor>>(response).ToList();
					foreach (var item in proveedores)
					{
						if (txtProveedor.Text == item.nombre)
						{
							_proveedorID = item.id_proveedor;
						}
					}
				}
				catch (Exception error)
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