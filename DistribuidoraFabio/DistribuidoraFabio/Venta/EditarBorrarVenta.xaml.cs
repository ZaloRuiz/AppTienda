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

namespace DistribuidoraFabio.Venta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarBorrarVenta : ContentPage
    {
        private int _id_venta_edit = 0;
        private DateTime _fecha_edit;
        private int _numero_factura_edit = 0;
        private string _cliente_edit;
        private string _vendedor_edit;
        private decimal _saldo_edit = 0;
        private string _tipo_venta_edit;
        private decimal _total_edit = 0;
        private string _estado_edit;
        private DateTime _fecha_entrega_edit;
        private string _observacion_edit;
        private int _detalle_venta;
        private int IdVenta;
        int numProd = 0;
        public EditarBorrarVenta(int _id_venta, DateTime _fecha, int _numero_factura, string _cliente, string _nombre_vendedor, string _tipo_venta,
            decimal _saldo, decimal _total, DateTime _fecha_entrega, string _estado, string _observacion)
        {
            InitializeComponent();
            _id_venta_edit = _id_venta;
            _fecha_edit = _fecha;
            _numero_factura_edit = _numero_factura;
            _cliente_edit = _cliente;
            _vendedor_edit = _nombre_vendedor;
            _tipo_venta_edit = _tipo_venta;
            _saldo_edit = _saldo;
            _total_edit = _total;
            _estado_edit = _estado;
            _fecha_entrega_edit = _fecha_entrega;
            _observacion_edit = _observacion;
            EditarVenta();
        }
        private async void EditarVenta()
        {
            int numProd = 0;
            try
            {
                // ************* FACTURA***************
                StackLayout stk1 = new StackLayout();
                stk1.Orientation = StackOrientation.Horizontal;
                stkDatos.Children.Add(stk1);
                Entry entFac = new Entry();
                entFac.Text = _numero_factura_edit.ToString();
                entFac.Placeholder = "FACTURA";
                entFac.IsEnabled = false;
                entFac.FontSize = 18;
                entFac.TextColor = Color.FromHex("#000000");
                entFac.HorizontalOptions = LayoutOptions.FillAndExpand;
                stk1.Children.Add(entFac);
                // ************* FIN FACTURA***************
                Entry entryFecha = new Entry();
                entryFecha.Placeholder = "Fecha";
                entryFecha.Text = _fecha_edit.ToString("d");
                entryFecha.FontSize = 18;
                entryFecha.TextColor = Color.FromHex("#000000");
                entryFecha.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryFecha.Completed += entryFecha_completed;
                stk1.Children.Add(entryFecha);
                void entryFecha_completed(object sender, EventArgs e)
                {
                    _fecha_edit = Convert.ToDateTime(entryFecha.Text);
                }
                // *** fin fecha **********                
                StackLayout stk3 = new StackLayout();
                stk3.Orientation = StackOrientation.Horizontal;
                stkDatos.Children.Add(stk3);
                Entry entryCliente = new Entry();
                entryCliente.Text = _cliente_edit;
                entryCliente.Placeholder = "Cliente";
                entryCliente.IsEnabled = false;
                entryCliente.FontSize = 18;
                entryCliente.TextColor = Color.FromHex("#000000");
                entryCliente.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryCliente.WidthRequest = 200;
                stk3.Children.Add(entryCliente);
                //**** FIN CLIENTE*****

                Entry entryVendedor = new Entry();
                entryVendedor.Placeholder = "Vendedor";
                entryVendedor.Text = _vendedor_edit;
                entryVendedor.IsEnabled = false;
                entryVendedor.FontSize = 18;
                entryVendedor.TextColor = Color.FromHex("#000000");
                entryVendedor.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryVendedor.WidthRequest = 200;
                stk3.Children.Add(entryVendedor);
                //Fin Vendedor
                
                DetalleVenta _detaVenta = new DetalleVenta()
                {
                    factura = _numero_factura_edit
                };
                var json = JsonConvert.SerializeObject(_detaVenta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/listaDetalleVentaNombre.php", content);

                var jsonR = await result.Content.ReadAsStringAsync();
                var dv_lista = JsonConvert.DeserializeObject<List<DetalleVentaNombre>>(jsonR);

                await Task.Delay(1000);
                foreach (var item in dv_lista)
                {
                    BoxView boxViewI = new BoxView();
                    boxViewI.HeightRequest = 1;
                    boxViewI.BackgroundColor = Color.FromHex("#95B0B7");
                    stkPrd.Children.Add(boxViewI);

                    numProd = numProd + 1;
                    StackLayout stkP1 = new StackLayout();
                    stkP1.Orientation = StackOrientation.Horizontal;
                    stkPrd.Children.Add(stkP1);

                    Entry entryProducto = new Entry();
                    entryProducto.Text = item.display_text_nombre;
                    entryProducto.Placeholder = "Producto";
                    entryProducto.IsEnabled = false;
                    entryProducto.FontSize = 18;
                    entryProducto.TextColor = Color.FromHex("#000000");
                    entryProducto.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP1.Children.Add(entryProducto);

                    _detalle_venta = item.id_dv;
                    App._detalleventaguardar.Add(new DetalleVentaNombre
                    {
                        id_dv = _detalle_venta,
                        cantidad = item.cantidad,
                        nombre_producto = item.nombre_producto,
                        nombre_sub_producto = item.nombre_sub_producto,
                        precio_producto = item.precio_producto,
                        descuento = item.descuento,
                        sub_total = item.sub_total,
                        envases = item.envases,
                        factura = item.factura,
                    });
                }
                //total
                await Task.Delay(1000);
                BoxView boxfinal = new BoxView();
                boxfinal.HeightRequest = 1;
                boxfinal.BackgroundColor = Color.FromHex("#95B0B7");
                stkFinal.Children.Add(boxfinal);

                StackLayout stack1 = new StackLayout();
                stack1.Orientation = StackOrientation.Horizontal;
                stkFinal.Children.Add(stack1);

                Entry entryTpVenta = new Entry();
                entryTpVenta.Placeholder = "Saldo";
                entryTpVenta.Text = _saldo_edit.ToString() + " Bs.";
                entryTpVenta.FontSize = 18;
                entryTpVenta.TextColor = Color.FromHex("#000000");
                entryTpVenta.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryTpVenta.Completed += entryTpVenta_completed;
                stack1.Children.Add(entryTpVenta);
                void entryTpVenta_completed(object sender, EventArgs e)
                {
                    _saldo_edit = Convert.ToDecimal(entryTpVenta.Text);
                }

                Entry entryEstado = new Entry();
                entryEstado.Placeholder = "Estado";
                entryEstado.Text = _estado_edit;
                entryEstado.IsEnabled = false;
                entryEstado.FontSize = 18;
                entryEstado.TextColor = Color.FromHex("#000000");
                entryEstado.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryEstado.Completed += entryEstado_completed;
                stack1.Children.Add(entryEstado);
                void entryEstado_completed(object sender, EventArgs e)
                {
                    _estado_edit = entryEstado.Text;
                }

                StackLayout stack3 = new StackLayout();
                stack3.Orientation = StackOrientation.Horizontal;
                stkFinal.Children.Add(stack3);
                Entry entryFechaE = new Entry();
                entryFechaE.Text = _fecha_entrega_edit.ToString("d");
                entryFechaE.Placeholder = "Fecha de Entrega";
                entryFechaE.TextColor = Color.FromHex("#000000");
                entryFechaE.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryFechaE.Completed += entryFechaE_completed;
                stack3.Children.Add(entryFechaE);
                void entryFechaE_completed(object sender, EventArgs e)
                {
                    _fecha_entrega_edit = Convert.ToDateTime(entryFechaE.Text);
                }
                StackLayout stack4 = new StackLayout();
                stack4.Orientation = StackOrientation.Horizontal;
                stkFinal.Children.Add(stack4);
                Entry entryObser = new Entry();
                entryObser.Placeholder = "Observaciones";
                entryObser.Text = _observacion_edit;
                entryObser.FontSize = 18;
                entryObser.TextColor = Color.FromHex("#000000");
                entryObser.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryObser.Completed += entryObser_completed;
                stack4.Children.Add(entryObser);
                void entryObser_completed(object sender, EventArgs e)
                {
                    _observacion_edit = entryObser.Text;
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo", "OK");
            }
        }
        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    foreach (var item in App._detalleventaguardar)
                    {
                        DetalleVenta detalleVenta = new DetalleVenta()
                        {
                            id_dv = item.id_dv,
                            cantidad = item.cantidad,
                            precio_producto = item.precio_producto,
                            descuento = item.descuento,
                            envases = item.envases,
                        };

                        var json1 = JsonConvert.SerializeObject(detalleVenta);
                        var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                        HttpClient client1 = new HttpClient();
                        var result1 = await client1.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarDetalleVenta.php", content1);
                    }
                    Ventas ventas = new Ventas()
                    {
                        id_venta = _id_venta_edit,
                        fecha = _fecha_edit,
                        numero_factura = _numero_factura_edit,
                        fecha_entrega = _fecha_entrega_edit,
                        estado = _estado_edit,
                        saldo = _saldo_edit,
                        observacion = _observacion_edit,
                    };

                    var json = JsonConvert.SerializeObject(ventas);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient client = new HttpClient();
                    var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarVenta.php", content);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("OK", "Se Agrego correctamente", "OK");
                        App._detalleventaguardar.Clear();
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