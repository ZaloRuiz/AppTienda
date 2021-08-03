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
            GetLista();
            txtFactura.Text = _numero_factura.ToString();
            txtFecha.Date = _fecha;
            txtCliente.Text = _cliente;
            txtVendedor.Text = _nombre_vendedor;
            txtTipoVenta.Text = _tipo_venta;
            txtSaldo.Text = _saldo.ToString();
            txtFechaEntrega.Date = _fecha_entrega;
            txtTotal.Text = _total.ToString();
            txtEstado.Text = _estado;
            txtObservaciones.Text = _observacion;
        }
        private async void GetLista()
        {
            if (CrossConnectivity.Current.IsConnected)
			{
                try
                {
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
                    int medida_lista = dv_lista.Count();
                    stkPrd.HeightRequest = medida_lista * 50;
                    listProductos.ItemsSource = dv_lista;
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
        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFactura.Text) || (!string.IsNullOrEmpty(txtFactura.Text)))
			{
                if (txtFecha.Date != null) 
				{
                    if (!string.IsNullOrWhiteSpace(txtCliente.Text) || (!string.IsNullOrEmpty(txtCliente.Text)))
					{
                        if (!string.IsNullOrWhiteSpace(txtVendedor.Text) || (!string.IsNullOrEmpty(txtVendedor.Text)))
						{
                            if (!string.IsNullOrWhiteSpace(txtTipoVenta.Text) || (!string.IsNullOrEmpty(txtTipoVenta.Text)))
							{
                                if (!string.IsNullOrWhiteSpace(txtSaldo.Text) || (!string.IsNullOrEmpty(txtSaldo.Text)))
                                {
                                    if (txtFechaEntrega.Date != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(txtTotal.Text) || (!string.IsNullOrEmpty(txtTotal.Text)))
                                        {
                                            if (!string.IsNullOrWhiteSpace(txtEstado.Text) || (!string.IsNullOrEmpty(txtEstado.Text)))
                                            {
                                                if (!string.IsNullOrWhiteSpace(txtObservaciones.Text) || (!string.IsNullOrEmpty(txtObservaciones.Text)))
                                                {
                                                    if (CrossConnectivity.Current.IsConnected)
                                                    {
                                                        try
                                                        {
                                                            Ventas ventas = new Ventas()
                                                            {
                                                                id_venta = _id_venta_edit,
                                                                fecha = txtFecha.Date,
                                                                numero_factura = Convert.ToInt32(txtFactura.Text),
                                                                fecha_entrega = txtFechaEntrega.Date,
                                                                estado = txtEstado.Text,
                                                                saldo = Convert.ToDecimal(txtSaldo.Text),
                                                                observacion = txtObservaciones.Text,
                                                            };

                                                            var json = JsonConvert.SerializeObject(ventas);
                                                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                                                            HttpClient client = new HttpClient();
                                                            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarVenta.php", content);
                                                            if (result.StatusCode == HttpStatusCode.OK)
                                                            {
                                                                await DisplayAlert("OK", "Se edito correctamente", "OK");
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
                                                else
												{
                                                    await DisplayAlert("Error", "El campo de Observacion esta vacio", "OK");
                                                }
                                            }
                                            else
                                            {
                                                await DisplayAlert("Error", "El campo de Estado esta vacio", "OK");
                                            }
                                        }
                                        else
                                        {
                                            await DisplayAlert("Error", "El campo de Total esta vacio", "OK");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Error", "El campo de Fecha de entrega esta vacio", "OK");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error", "El campo de Saldo esta vacio", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Error", "El campo de Tipo de venta esta vacio", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "El campo de Vendedor esta vacio", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "El campo de Cliente esta vacio", "OK");
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
    }
}