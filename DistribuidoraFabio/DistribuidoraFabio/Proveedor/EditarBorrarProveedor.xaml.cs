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

namespace DistribuidoraFabio.Proveedor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarBorrarProveedor : ContentPage
	{
		public EditarBorrarProveedor(int id_proveedor, string nombre, string direccion, string contacto, int telefono)
		{
			InitializeComponent();
			idProvEntry.Text = id_proveedor.ToString();
			nombrePEntry.Text = nombre.ToString();
			direccionPEntry.Text = direccion.ToString();
			contactoPEntry.Text = contacto.ToString();
			telefonoPEntry.Text = telefono.ToString();
		}
        private async void BtneditarProveedor_Clicked(object sender, EventArgs e)
        {
            Models.Proveedor proveedor = new Models.Proveedor()
            {
                id_proveedor = Convert.ToInt32(idProvEntry.Text),
                nombre = nombrePEntry.Text,
                direccion = direccionPEntry.Text,
                contacto = contactoPEntry.Text,
                telefono = Convert.ToInt32(telefonoPEntry.Text),
            };

            var json = JsonConvert.SerializeObject(proveedor);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/proveedores/editarProveedor.php", content);

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

        private async void BtnborrarProveedor_Clicked(object sender, EventArgs e)
        {
            Models.Proveedor proveedor = new Models.Proveedor()
            {
                id_proveedor = Convert.ToInt32(idProvEntry.Text),
                nombre = nombrePEntry.Text,
                direccion = direccionPEntry.Text,
                contacto = contactoPEntry.Text,
                telefono = Convert.ToInt32(telefonoPEntry.Text),
            };
            var json = JsonConvert.SerializeObject(proveedor);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/proveedores/borrarProveedor.php", content);

            if (result.StatusCode == HttpStatusCode.OK)
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
    }
}