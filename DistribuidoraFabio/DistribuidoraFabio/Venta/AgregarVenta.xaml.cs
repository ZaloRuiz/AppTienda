using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Models;
using dotMorten.Xamarin.Forms;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Venta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarVenta : ContentPage
    {
        List<ProductoNombre> prods = new List<ProductoNombre>();
        List<Vendedores> vendedorList = new List<Vendedores>();
        List<Models.Cliente> clienteList = new List<Models.Cliente>();
        private int idProductoSelected = 0;
        private decimal MontoTotal = 0;
        private int idClienteSelected = 0;
        private int idVendedorSelected = 0;
        private DateTime _fechaHoy = DateTime.Now;
        public AgregarVenta()
        {
            InitializeComponent();
            App._detalleVData.Clear();
            tipoVentaEntry.ItemsSource = new List<string> { "Contado", "Credito" };
            estadoEntry.ItemsSource = new List<string> { "Entregado", "Pendiente"};
            GetDataCliente();
            GetDataVendedor();
            GetProductos();
        }
        protected override void OnAppearing()
        {
            listProductos.ItemsSource = App._detalleVData;
            base.OnAppearing();
        }
        private async void GetDataCliente()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/clientes/listaCliente.php");
                var clientes = JsonConvert.DeserializeObject<List<Models.Cliente>>(response).ToList();
                clientePicker.ItemsSource = clientes;
                foreach (var item in clientes)
                {
                    clienteList.Add(item);
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Erro", error.ToString(), "OK");
            }
        }
        private async void GetDataVendedor()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/vendedores/listaVendedores.php");
                var vendedores = JsonConvert.DeserializeObject<List<Vendedores>>(response).ToList();
                vendedorPicker.ItemsSource = vendedores;
                foreach (var item in vendedores)
                {
                    vendedorList.Add(item);
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Error", error.ToString(), "OK");
            }
        }
        public async void GetProductos()
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
                await DisplayAlert("Error", error.ToString(), "OK");
            }
        }
        List<string> _listSuggestion = null;
        private async void entryNombreProd_TextChanged_1(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            try
            {
                if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    List<string> _nameList = new List<string>();
                    foreach (var item in prods.Distinct())
                    {
                        _nameList.Add(item.nombre_producto);
                    }
                    _listSuggestion = _nameList.Where(x => x.ToLower().Contains(sender.Text.ToLower())).ToList();
                    sender.ItemsSource = _listSuggestion;
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        string pickedProducto;
        private async void entryNombreProd_SuggestionChosen_1(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            try
            {
                var selectedItem = e.SelectedItem.ToString();
                sender.Text = selectedItem;
                if(sender.Text != string.Empty)
				{
                    pickedProducto = selectedItem;
                    try
                    {
                        foreach (var item in prods)
                        {
                            if (pickedProducto == item.nombre_producto)
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
                        await DisplayAlert("ERROR", err.ToString(), "OK");
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        private async void entryNombreProd_QuerySubmitted_1(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                if (e.ChosenSuggestion != null)
                {
                    entryNombreProd.Text = e.ChosenSuggestion.ToString();
                }
                else
                {
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        private string vendedorPick;
        private async void VendedorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex != -1)
                {
                    vendedorPick = picker.Items[selectedIndex];
                    try
                    {
                        foreach (var item in vendedorList)
                        {
                            if (vendedorPick == item.nombre)
                            {
                                idVendedorSelected = item.id_vendedor;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        await DisplayAlert("ERROR", err.ToString(), "OK");
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        private string clientePick;
        private async void ClientePicker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectIndex = picker.SelectedIndex;
            if (selectIndex != -1)
            {
                clientePick = picker.Items[selectIndex];
                try
                {
                    foreach (var item in clienteList)
                    {
                        if (clientePick == item.nombre_cliente)
                        {
                            idClienteSelected = item.id_cliente;
                        }
                    }
                }
                catch (Exception err)
                {
                    await DisplayAlert("ERROR", err.ToString(), "OK");
                }
            }
        }
        private string estadoPick;
        private void estadoEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                estadoPick = picker.Items[selectedIndex];
            }
        }
        private string tipoVentaPick;
        private void TipoVentaEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                tipoVentaPick = picker.Items[selectedIndex];
                if (selectedIndex == 1)
                {
                    saldo_VentaEntry.IsVisible = true;
                }

                if (selectedIndex == 0)
                {
                    saldo_VentaEntry.IsVisible = false;
                }
            }
        }
        decimal precioSelected = 0;
        int cantidaSelected = 0;
        decimal descuentoSelected = 0;
        decimal precioFinalSelected = 0;
        decimal subTotalSelected = 0;
        decimal stockSelected = 0;
        decimal stockValoradoSelected = 0;
        decimal promedioSelected = 0;
        int envaseSelected = 0;
        private async void txtDescuento_Completed(object sender, EventArgs e)
        {
            try
            {
                precioSelected = Convert.ToDecimal(txtPrecio.Text);
                //cantidaSelected = Convert.ToInt32(txtCantidad.Text);
                descuentoSelected = Convert.ToDecimal(txtDescuento.Text);
                stockSelected = Convert.ToDecimal(txtStock.Text);
                stockValoradoSelected = Convert.ToDecimal(txtStockValorado.Text);
                promedioSelected = Convert.ToDecimal(txtPromedio.Text);
                precioFinalSelected = precioSelected - descuentoSelected;
                subTotalSelected = precioFinalSelected * cantidaSelected;
                txtSubTotal.Text = subTotalSelected.ToString();
                envaseSelected = Convert.ToInt32(txtEnvases.Text);
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        private async void txtCantidad_Completed(object sender, EventArgs e)
        {
            try
            {
                precioSelected = Convert.ToDecimal(txtPrecio.Text);
                cantidaSelected = Convert.ToInt32(txtCantidad.Text);
                descuentoSelected = Convert.ToDecimal(txtDescuento.Text);
                stockSelected = Convert.ToDecimal(txtStock.Text);
                stockValoradoSelected = Convert.ToDecimal(txtStockValorado.Text);
                promedioSelected = Convert.ToDecimal(txtPromedio.Text);
                precioFinalSelected = precioSelected - descuentoSelected;
                subTotalSelected = precioFinalSelected * cantidaSelected;
                txtSubTotal.Text = subTotalSelected.ToString();
                envaseSelected = Convert.ToInt32(txtEnvases.Text);
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
        private async void agregarAlista_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDescuento.Text) || (!string.IsNullOrEmpty(txtDescuento.Text)))
            {
                if (!string.IsNullOrWhiteSpace(txtCantidad.Text) || (!string.IsNullOrEmpty(txtCantidad.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(txtEnvases.Text) || (!string.IsNullOrEmpty(txtEnvases.Text)))
                    {
                        try
                        {
                            envaseSelected = Convert.ToInt32(txtEnvases.Text);
                            App._detalleVData.Add(new DetalleVenta_previo
                            {
                                cantidad = cantidaSelected,
                                id_producto = idProductoSelected,
                                nombre_producto = pickedProducto,
                                precio_producto = precioSelected,
                                descuento = descuentoSelected,
                                sub_total = subTotalSelected,
                                envases = envaseSelected,
                                stock = stockSelected,
                                stock_valorado = stockValoradoSelected,
                                promedio = promedioSelected,
                                factura = Convert.ToInt32(numero_facturaVentaEntry.Text)
                            }); ;
                            //picker_Producto.SelectedIndex = -1;
                            //picker_Producto.Items.Clear();
                            entryNombreProd.Text = string.Empty;
                            txtPrecio.Text = string.Empty;
                            txtCantidad.Text = string.Empty;
                            txtDescuento.Text = string.Empty;
                            txtSubTotal.Text = string.Empty;
                            txtStock.Text = string.Empty;
                            txtStockValorado.Text = string.Empty;
                            txtPromedio.Text = string.Empty;
                            txtEnvases.Text = "0";
                            MontoTotal = 0;
                            foreach (var item in App._detalleVData)
                            {
                                MontoTotal = MontoTotal + item.sub_total;
                            }
                            totalVentaEntry.Text = MontoTotal.ToString();
                        }
                        catch (Exception err)
                        {
                            await DisplayAlert("Error", err.ToString(), "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de envases no puede estar vacio", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El campo de cantidad no puede estar vacio", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "El campo de descuento no puede estar vacio", "Ok");
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
                        var detalles = e.Item as DetalleVenta_previo;
                        App._detalleVData.Remove(detalles);
                        MontoTotal = 0;
                        foreach (var item in App._detalleVData)
                        {
                            MontoTotal = MontoTotal + item.sub_total;
                        }
                        totalVentaEntry.Text = MontoTotal.ToString();
                    }
                    catch (Exception err)
                    {
                        await DisplayAlert("Error", err.ToString(), "OK");
                    }
                    break;
                case "NO":
                    break;
            }
        }
        private async void BtnVentaGuardar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(numero_facturaVentaEntry.Text) || (!string.IsNullOrEmpty(numero_facturaVentaEntry.Text)))
            {
                if (!string.IsNullOrWhiteSpace(saldo_VentaEntry.Text) || (!string.IsNullOrEmpty(saldo_VentaEntry.Text)))
                {
                    if (!string.IsNullOrWhiteSpace(totalVentaEntry.Text) || (!string.IsNullOrEmpty(totalVentaEntry.Text)))
                    {
                        if (!string.IsNullOrWhiteSpace(entryObs.Text) || (!string.IsNullOrEmpty(entryObs.Text)))
                        {
                            if (App._detalleVData.Count() > 0)
                            {
                                string BusyReason = "Cargando...";
                                try
                                {
                                    await PopupNavigation.Instance.PushAsync(new BusyPopup(BusyReason));
                                    if(estadoPick == "Entregado")
									{
                                        foreach (var item in App._detalleVData)
                                        {
                                            DetalleVenta detalleVenta = new DetalleVenta()
                                            {
                                                cantidad = item.cantidad,
                                                id_producto = item.id_producto,
                                                precio_producto = item.precio_producto,
                                                descuento = item.descuento,
                                                sub_total = item.sub_total,
                                                envases = item.envases,
                                                factura = Convert.ToInt32(numero_facturaVentaEntry.Text)
                                            };

                                            var json1 = JsonConvert.SerializeObject(detalleVenta);
                                            var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                                            HttpClient client1 = new HttpClient();
                                            var result1 = await client1.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/agregarDetalleVenta.php", content1);

                                            Models.Inventario inventario = new Models.Inventario()
                                            {
                                                id_producto = item.id_producto,
                                                fecha_inv = fechaVentaEntry.Date,
                                                numero_factura = Convert.ToInt32(numero_facturaVentaEntry.Text),
                                                detalle = "Venta",
                                                precio_compra = 0,
                                                unidades = item.cantidad,
                                                entrada_fisica = 0,
                                                salida_fisica = item.cantidad,
                                                saldo_fisica = item.stock - item.cantidad,
                                                entrada_valorado = 0,
                                                salida_valorado = item.cantidad * item.promedio,
                                                saldo_valorado = item.stock_valorado - (item.cantidad * item.promedio),
                                                promedio = item.stock_valorado / item.stock
                                            };

                                            var json2 = JsonConvert.SerializeObject(inventario);
                                            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
                                            HttpClient client2 = new HttpClient();
                                            var result2 = await client2.PostAsync("https://dmrbolivia.com/api_distribuidora/inventarios/agregarInventario.php", content2);

                                            Models.Producto producto = new Models.Producto()
                                            {
                                                id_producto = item.id_producto,
                                                stock = item.stock - item.cantidad,
                                                stock_valorado = item.stock_valorado - (item.cantidad * item.promedio),
                                                promedio = item.stock_valorado / item.stock
                                            };
                                            var json3 = JsonConvert.SerializeObject(producto);
                                            var content3 = new StringContent(json3, Encoding.UTF8, "application/json");
                                            HttpClient client3 = new HttpClient();
                                            var result3 = await client3.PostAsync("https://dmrbolivia.com/api_distribuidora/productos/editarProducto.php", content3);
                                        }
                                        Ventas ventas = new Ventas()
                                        {
                                            fecha = fechaVentaEntry.Date,
                                            numero_factura = Convert.ToInt32(numero_facturaVentaEntry.Text),
                                            id_cliente = idClienteSelected,
                                            id_vendedor = idVendedorSelected,
                                            tipo_venta = tipoVentaPick,
                                            saldo = Convert.ToDecimal(saldo_VentaEntry.Text),
                                            total = Convert.ToDecimal(totalVentaEntry.Text),
                                            fecha_entrega = _fechaHoy,
                                            estado = estadoPick,
                                            observacion = entryObs.Text
                                        };

                                        var json = JsonConvert.SerializeObject(ventas);
                                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                                        HttpClient client = new HttpClient();
                                        var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/agregarVenta.php", content);
                                        if (result.StatusCode == HttpStatusCode.OK)
                                        {
                                            await PopupNavigation.Instance.PopAsync();
                                            await DisplayAlert("OK", "Se agrego correctamente", "OK");
                                            App._detalleVData.Clear();
                                            await Navigation.PopAsync();
                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PopAsync();
                                            await DisplayAlert("Error", result.StatusCode.ToString(), "OK");
                                            await Navigation.PopAsync();
                                        }
                                    }
                                    else if(estadoPick == "Pendiente")
									{
                                        foreach (var item in App._detalleVData)
                                        {
                                            DetalleVenta detalleVenta = new DetalleVenta()
                                            {
                                                cantidad = item.cantidad,
                                                id_producto = item.id_producto,
                                                precio_producto = item.precio_producto,
                                                descuento = item.descuento,
                                                sub_total = item.sub_total,
                                                envases = item.envases,
                                                factura = Convert.ToInt32(numero_facturaVentaEntry.Text)
                                            };

                                            var json1 = JsonConvert.SerializeObject(detalleVenta);
                                            var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                                            HttpClient client1 = new HttpClient();
                                            var result1 = await client1.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/agregarDetalleVenta.php", content1);
                                        }
                                        Ventas ventas5 = new Ventas()
                                        {
                                            fecha = fechaVentaEntry.Date,
                                            numero_factura = Convert.ToInt32(numero_facturaVentaEntry.Text),
                                            id_cliente = idClienteSelected,
                                            id_vendedor = idVendedorSelected,
                                            tipo_venta = tipoVentaPick,
                                            saldo = Convert.ToDecimal(saldo_VentaEntry.Text),
                                            total = Convert.ToDecimal(totalVentaEntry.Text),
                                            fecha_entrega = _fechaHoy,
                                            estado = "Pendiente",
                                            observacion = entryObs.Text
                                        };

                                        var json5 = JsonConvert.SerializeObject(ventas5);
                                        var content5 = new StringContent(json5, Encoding.UTF8, "application/json");
                                        HttpClient client5 = new HttpClient();
                                        var result5 = await client5.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/agregarVenta.php", content5);
                                        if (result5.StatusCode == HttpStatusCode.OK)
                                        {
                                            await PopupNavigation.Instance.PopAsync();
                                            await DisplayAlert("OK", "Se agrego correctamente", "OK");
                                            App._detalleVData.Clear();
                                            await Navigation.PopAsync();
                                        }
                                        else
                                        {
                                            await PopupNavigation.Instance.PopAsync();
                                            await DisplayAlert("Error", result5.StatusCode.ToString(), "OK");
                                            await Navigation.PopAsync();
                                        }
                                    }
                                }
                                catch (Exception error)
                                {
                                    await PopupNavigation.Instance.PopAsync();
                                    await DisplayAlert("Error", error.ToString(), "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Error", "Agregue un producto a la lista", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Campo vacio", "El campo de Observacion esta vacio", "Ok");
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
	}
}