using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Compra
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarCompra : ContentPage
	{
		List<ProductoNombre> prods = new List<ProductoNombre>();
		List<Models.Proveedor> proveedorList = new List<Models.Proveedor>();
		private int idProductoSelected = 0;
		private decimal MontoTotal = 0;
		private int idProveedorSelected = 0;
		public AgregarCompra()
		{
			InitializeComponent();
			App._detalleCData.Clear();
			GetDataProveedor();
			GetTipoProducto();
			GetProductos();
		}
		protected override void OnAppearing()
		{
			listProductos.ItemsSource = App._detalleCData;
			base.OnAppearing();
		}
		private async void GetDataProveedor()
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
						proveedorList.Add(item);
					}
					proveedorPicker.ItemsSource = proveedorList;
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
		private async void GetTipoProducto()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tp_productos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response).ToList();
					picker_TP.ItemsSource = tp_productos;
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
		public async void GetProductos()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/productos/listaProductoNombres.php");
					var prodsList = JsonConvert.DeserializeObject<List<ProductoNombre>>(response).ToList();
					foreach (var item in prodsList)
					{
						prods.Add(item);
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
		private string proveedorPick;
		private async void ProveedorPicker_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectIndex = picker.SelectedIndex;
			if (selectIndex != -1)
			{
				proveedorPick = picker.Items[selectIndex];
				try
				{
					foreach (var item in proveedorList)
					{
						if (proveedorPick == item.nombre)
						{
							idProveedorSelected = item.id_proveedor;
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
				}
			}
			
		}
		string pickedTP;
		private async void picker_TP_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;

			if (selectedIndex != -1)
			{
				pickedTP = picker.Items[selectedIndex];
			}
			try
			{
				picker_Producto.Items.Clear();
				foreach (var item in prods)
				{
					if (item.nombre_tipo_producto == pickedTP)
					{
						picker_Producto.Items.Add(item.display_text_nombre);
					}
				}
			}
			catch (Exception error)
			{
				await DisplayAlert("Erro", "Algo salio mal, intentelo de nuevo", "OK");
			}
		}
		string pickedProducto;
		private async void picker_Producto_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;

				if (selectedIndex != -1)
				{
					pickedProducto = picker.Items[selectedIndex];
					try
					{
						foreach (var item in prods)
						{
							if (pickedProducto == item.display_text_nombre)
							{
								txtPrecio.Text = item.precio_venta.ToString("0.##");
								txtStock.Text = item.stock.ToString();
								txtStockValorado.Text = item.stock_valorado.ToString();
								txtPromedio.Text = item.promedio.ToString();
								idProductoSelected = item.id_producto;
							}
						}
					}
					catch (Exception err)
					{
						await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
					}
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
			}
		}
		decimal precioSelected = 0;
		int cantidaSelected = 0;
		decimal descuentoSelected = 0;
		decimal precioFinalSelected = 0;
		decimal subTotalSelected = 0;
		int stockSelected = 0;
		decimal stockValoradoSelected = 0;
		decimal promedioSelected = 0;
		private async void txtDescuento_Completed(object sender, EventArgs e)
		{
			try
			{
				precioSelected = Convert.ToDecimal(txtPrecio.Text);
				cantidaSelected = Convert.ToInt32(txtCantidad.Text);
				descuentoSelected = Convert.ToDecimal(txtDescuento.Text);
				stockSelected = Convert.ToInt32(txtStock.Text);
				stockValoradoSelected = Convert.ToDecimal(txtStockValorado.Text);
				promedioSelected = Convert.ToDecimal(txtPromedio.Text);
				precioFinalSelected = precioSelected - descuentoSelected;
				subTotalSelected = precioFinalSelected * cantidaSelected;
				txtSubTotal.Text = subTotalSelected.ToString();
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
			}
		}
		private async void agregarAlista_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (!string.IsNullOrWhiteSpace(txtPrecio.Text) || (!string.IsNullOrEmpty(txtPrecio.Text)))
				{
					if (!string.IsNullOrWhiteSpace(txtCantidad.Text) || (!string.IsNullOrEmpty(txtCantidad.Text)))
					{
						if (!string.IsNullOrWhiteSpace(txtDescuento.Text) || (!string.IsNullOrEmpty(txtDescuento.Text)))
						{
							try
							{
								App._detalleCData.Add(new DetalleCompra_previo
								{
									cantidad = cantidaSelected,
									id_producto = idProductoSelected,
									nombre_producto = pickedTP + " " + pickedProducto,
									precio_producto = precioSelected,
									descuento = descuentoSelected,
									sub_total = subTotalSelected,
									stock = stockSelected,
									stock_valorado = stockValoradoSelected,
									promedio = promedioSelected
								});
								picker_Producto.SelectedIndex = -1;
								picker_Producto.Items.Clear();
								picker_TP.SelectedIndex = -1;
								txtPrecio.Text = string.Empty;
								txtCantidad.Text = string.Empty;
								txtDescuento.Text = "0";
								txtSubTotal.Text = string.Empty;
								txtStock.Text = string.Empty;
								txtStockValorado.Text = string.Empty;
								txtPromedio.Text = string.Empty;
								MontoTotal = 0;
								foreach (var item in App._detalleCData)
								{
									MontoTotal = MontoTotal + item.sub_total;
								}
								totalCompraEntry.Text = MontoTotal.ToString();
							}
							catch (Exception err)
							{
								await DisplayAlert("Erro", "Algo salio mal, intentelo de nuevo", "OK");
							}
						}
						else
						{
							await DisplayAlert("Error", "El campo de descuento no puede estar vacio", "Ok");
						}
					}
					else
					{
						await DisplayAlert("Error", "El campo de cantidad no puede estar vacio", "Ok");
					}
				}
				else
				{
					await DisplayAlert("Error", "El campo de precio no puede estar vacio", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void listProductos_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var action = await DisplayActionSheet("BORRAR PRODUCTO DE LA LISTA?", null, null, "SI", "NO");
			switch (action)
			{
				case "SI":
					try
					{
						var detalles = e.Item as DetalleCompra_previo;
						App._detalleCData.Remove(detalles);
						MontoTotal = 0;
						foreach (var item in App._detalleCData)
						{
							MontoTotal = MontoTotal + item.sub_total;
						}
						totalCompraEntry.Text = MontoTotal.ToString();
					}
					catch (Exception err)
					{
						await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
					}
					break;
				case "NO":
					break;
			}
		}
		private async void btnCompraGuardar_Clicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				string BusyReason = "Cargando...";
				if (!string.IsNullOrWhiteSpace(numero_facturaCompraEntry.Text) || (!string.IsNullOrEmpty(numero_facturaCompraEntry.Text)))
				{
					if (!string.IsNullOrWhiteSpace(saldo_CompraEntry.Text) || (!string.IsNullOrEmpty(saldo_CompraEntry.Text)))
					{
						if (!string.IsNullOrWhiteSpace(totalCompraEntry.Text) || (!string.IsNullOrEmpty(totalCompraEntry.Text)))
						{
							try
							{
								await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
								if (App._detalleCData.Count() > 0)
								{
									foreach (var item in App._detalleCData)
									{
										DetalleCompra detalleCompra = new DetalleCompra()
										{
											cantidad_compra = item.cantidad,
											id_producto = item.id_producto,
											precio_producto = item.precio_producto,
											descuento_producto = item.descuento,
											sub_total = item.sub_total,
											numero_factura = Convert.ToInt32(numero_facturaCompraEntry.Text)
										};

										var json1 = JsonConvert.SerializeObject(detalleCompra);
										var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
										HttpClient client1 = new HttpClient();
										var result1 = await client1.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/agregarDetalleCompra.php", content1);

										Models.Inventario _inventario = new Models.Inventario()
										{
											id_producto = item.id_producto,
											fecha_inv = fechaCompraEntry.Date,
											numero_factura = Convert.ToInt32(numero_facturaCompraEntry.Text),
											detalle = "Compra",
											precio_compra = item.precio_producto - item.descuento,
											unidades = item.cantidad,
											entrada_fisica = item.cantidad,
											salida_fisica = 0,
											saldo_fisica = item.stock + item.cantidad,
											entrada_valorado = (item.precio_producto - item.descuento) * item.cantidad,
											salida_valorado = 0,
											saldo_valorado = item.stock_valorado + ((item.precio_producto - item.descuento) * item.cantidad),
											promedio = ((item.stock_valorado + (item.precio_producto - item.descuento)) * item.cantidad) / item.stock + item.cantidad
										};

										var json2 = JsonConvert.SerializeObject(_inventario);
										var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
										HttpClient client2 = new HttpClient();
										var result2 = await client2.PostAsync("https://dmrbolivia.com/api_distribuidora/inventarios/agregarInventario.php", content2);

										Models.Producto producto = new Models.Producto()
										{
											id_producto = item.id_producto,
											stock = item.stock + item.cantidad,
											stock_valorado = item.stock_valorado + (item.cantidad * item.promedio),
											promedio = (item.stock_valorado + ((item.precio_producto - item.descuento) * item.cantidad)) / (item.stock + item.cantidad)
										};
										var json3 = JsonConvert.SerializeObject(producto);
										var content3 = new StringContent(json3, Encoding.UTF8, "application/json");
										HttpClient client3 = new HttpClient();
										var result3 = await client3.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/editarProducto.php", content3);
									}
									Compras _compras = new Compras()
									{
										fecha_compra = fechaCompraEntry.Date,
										numero_factura = Convert.ToInt32(numero_facturaCompraEntry.Text),
										id_proveedor = idProveedorSelected,
										saldo = Convert.ToDecimal(saldo_CompraEntry.Text),
										total = Convert.ToDecimal(totalCompraEntry.Text)
									};

									var json = JsonConvert.SerializeObject(_compras);
									var content = new StringContent(json, Encoding.UTF8, "application/json");
									HttpClient client = new HttpClient();
									var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/compras/agregarCompra.php", content);
									if (result.StatusCode == HttpStatusCode.OK)
									{
										await PopupNavigation.Instance.PopAsync();
										await DisplayAlert("OK", "Se agrego correctamente", "OK");
										App._detalleCData.Clear();
										await Navigation.PopAsync();
									}
									else
									{
										await PopupNavigation.Instance.PopAsync();
										await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
										await Navigation.PopAsync();
									}
								}
								else
								{
									await PopupNavigation.Instance.PopAsync();
									await DisplayAlert("ERROR", "Agregue productos a la lista", "OK");
								}
							}
							catch (Exception error)
							{
								await PopupNavigation.Instance.PopAsync();
								await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
							}
						}
						else
						{
							await DisplayAlert("Campo vacio", "El campo de Total esta vacio", "Ok");
						}
					}
					else
					{
						await DisplayAlert("Campo vacio", "El campo de Saldo esta vacio", "Ok");
					}
				}
				else
				{
					await DisplayAlert("Campo vacio", "El campo de Numero de factura esta vacio", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
	}
}