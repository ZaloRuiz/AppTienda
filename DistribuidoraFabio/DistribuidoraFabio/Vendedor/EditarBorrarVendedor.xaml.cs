using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Vendedor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarVendedor : ContentPage
	{
        private int IdVendedor;
        public EditarBorrarVendedor(int id_vendedor, string nombre, int telefono, string direccion, string numero_cuenta, string cedula_identidad)
		{
			InitializeComponent();
            IdVendedor = id_vendedor;
			idEntry.Text = id_vendedor.ToString();
			nombreEntry.Text = nombre;
			telefonoEntry.Text = telefono.ToString();
			direccionEntry.Text = direccion;
			numero_cuentaEntry.Text = numero_cuenta;
			cedulaEntry.Text = cedula_identidad;
		}
        private async void BtnEditarVendedor_Clicked(object sender, EventArgs e)
        {
            Vendedores vendedores = new Vendedores()
            {
                id_vendedor = Convert.ToInt32(idEntry.Text),
                nombre = nombreEntry.Text,
                telefono = Convert.ToInt32(telefonoEntry.Text),
                direccion = direccionEntry.Text,
                numero_cuenta = numero_cuentaEntry.Text,
                cedula_identidad = cedulaEntry.Text
            };

            var json = JsonConvert.SerializeObject(vendedores);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/vendedores/editarVendedor.php", content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("EDITADO", "Se edito correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("ERROR", result.StatusCode.ToString(), "OK");
                await Navigation.PopAsync();
            }
        }

        private async void BtnBorrarVendedor_Clicked(object sender, EventArgs e)
        {
            Vendedores vendedores = new Vendedores()
            {
                id_vendedor = Convert.ToInt32(idEntry.Text),
                nombre = nombreEntry.Text,
                telefono = Convert.ToInt32(telefonoEntry.Text),
                direccion = direccionEntry.Text,
                numero_cuenta = numero_cuentaEntry.Text,
                cedula_identidad = cedulaEntry.Text
            };

            var json = JsonConvert.SerializeObject(vendedores);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/vendedores/borrarVendedor.php", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("ELIMINADO", "Se elimino correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("ERROR", result.StatusCode.ToString(), "OK");
                await Navigation.PopAsync();
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistorialVendedor(IdVendedor));
        }
    }
}