using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                StackLayout stk1 = new StackLayout();
                stk1.Orientation = StackOrientation.Horizontal;
                stkDatos.Children.Add(stk1);
                Entry entFac = new Entry();
                entFac.Text = _numero_factura_edit.ToString();
                entFac.Placeholder = "Factura";
                entFac.FontSize = 18;
                entFac.TextColor = Color.FromHex("#000000");
                entFac.HorizontalOptions = LayoutOptions.FillAndExpand;
                stk1.Children.Add(entFac);
                 
                Entry entryFecha = new Entry();
                entryFecha.Placeholder = "Fecha de venta";
                entryFecha.Text = _fecha_edit.ToString("dd/MM/yyyy");
                entryFecha.FontSize = 18;
                entryFecha.TextColor = Color.FromHex("#000000");
                entryFecha.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryFecha.WidthRequest = 200;
                stk1.Children.Add(entryFecha);
                               
                StackLayout stk3 = new StackLayout();
                stk3.Orientation = StackOrientation.Horizontal;
                stkDatos.Children.Add(stk3);
                Entry entryCliente = new Entry();
                entryCliente.Text = _cliente_edit;
                entryCliente.Placeholder = "Cliente";
                entryCliente.FontSize = 18;
                entryCliente.TextColor = Color.FromHex("#000000");
                entryCliente.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryCliente.WidthRequest = 200;
                stk3.Children.Add(entryCliente);
                 
                Entry entryVendedor = new Entry();
                entryVendedor.Placeholder = "Vendedor";
                entryVendedor.Text = _vendedor_edit;
                entryVendedor.IsEnabled = false;
                entryVendedor.FontSize = 18;
                entryVendedor.TextColor = Color.FromHex("#000000");
                entryVendedor.HorizontalOptions = LayoutOptions.FillAndExpand;
                entryVendedor.WidthRequest = 200;
                stk3.Children.Add(entryVendedor);

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
                    entryProducto.FontSize = 18;
                    entryProducto.TextColor = Color.FromHex("#000000");
                    entryProducto.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP1.Children.Add(entryProducto);

                    StackLayout stkP2 = new StackLayout();
                    stkP2.Orientation = StackOrientation.Horizontal;
                    stkPrd.Children.Add(stkP2);

                    Entry entryCantidad = new Entry();
                    entryCantidad.Placeholder = "Cantidad";
                    entryCantidad.Text = item.cantidad.ToString();
                    entryCantidad.FontSize = 18;
                    entryCantidad.TextColor = Color.FromHex("#000000");
                    entryCantidad.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP2.Children.Add(entryCantidad);

                    StackLayout stkP3 = new StackLayout();
                    stkP3.Orientation = StackOrientation.Horizontal;
                    stkPrd.Children.Add(stkP3);

                    Entry entryPrecio = new Entry();
                    entryPrecio.Placeholder = "Precio";
                    entryPrecio.Text = item.precio_producto.ToString();
                    entryPrecio.FontSize = 18;
                    entryPrecio.TextColor = Color.FromHex("#000000");
                    entryPrecio.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP3.Children.Add(entryPrecio);

                    StackLayout stkP4 = new StackLayout();
                    stkP4.Orientation = StackOrientation.Horizontal;
                    stkPrd.Children.Add(stkP4);

                    Entry entrySubtotal = new Entry();
                    entrySubtotal.Placeholder = "Subtotal";
                    entrySubtotal.Text = item.sub_total.ToString();
                    entrySubtotal.FontSize = 18;
                    entrySubtotal.TextColor = Color.FromHex("#000000");
                    entrySubtotal.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP4.Children.Add(entrySubtotal);

                    StackLayout stkP5 = new StackLayout();
                    stkP5.Orientation = StackOrientation.Horizontal;
                    stkPrd.Children.Add(stkP5);

                    Entry entryEnvases = new Entry();
                    entryEnvases.Placeholder = "Envases";
                    entryEnvases.Text = item.envases.ToString();
                    entryEnvases.FontSize = 18;
                    entryEnvases.TextColor = Color.FromHex("#000000");
                    entryEnvases.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stkP5.Children.Add(entryEnvases);
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
                entryTpVenta.Placeholder = "Tipo de Venta";
                entryTpVenta.Text = _tipo_venta_edit;
                entryTpVenta.FontSize = 18;
                entryTpVenta.TextColor = Color.FromHex("#000000");
                entryTpVenta.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack1.Children.Add(entryTpVenta);

                Entry entryEstado = new Entry();
                entryEstado.Placeholder = "Estado";
                entryEstado.Text = _estado_edit;
                entryEstado.FontSize = 18;
                entryEstado.TextColor = Color.FromHex("#000000");
                entryEstado.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack1.Children.Add(entryEstado);

                StackLayout stack3 = new StackLayout();
                stack3.Orientation = StackOrientation.Horizontal;
                stkFinal.Children.Add(stack3);

                Entry entryFechaE = new Entry();
                entryFechaE.Placeholder = "Fecha de entrega";
                entryFechaE.Text = _fecha_entrega_edit.ToString("dd/MM/yyyy");
                entryFechaE.TextColor = Color.FromHex("#000000");
                entryFechaE.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack3.Children.Add(entryFechaE);

                StackLayout stack4 = new StackLayout();
                stack4.Orientation = StackOrientation.Horizontal;
                stkFinal.Children.Add(stack4);

                Entry entryObser = new Entry();
                entryObser.Placeholder = "Observaciones";
                entryObser.Text = _observacion_edit;
                entryObser.FontSize = 18;
                entryObser.TextColor = Color.FromHex("#000000");
                entryObser.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack4.Children.Add(entryObser);

                Entry entryTotal = new Entry();
                entryTotal.Placeholder = "Total";
                entryTotal.Text = _total_edit.ToString("#.##") + " Bs.";
                entryTotal.FontSize = 18;
                entryTotal.TextColor = Color.FromHex("#000000");
                entryTotal.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack3.Children.Add(entryTotal);
            }
            catch (Exception err)
            {
                await DisplayAlert("ERROR", err.ToString(), "OK");
            }
        }
		private void btnEditar_Clicked(object sender, EventArgs e)
		{

		}
		private void btnBorrar_Clicked(object sender, EventArgs e)
		{

		}
	}
}