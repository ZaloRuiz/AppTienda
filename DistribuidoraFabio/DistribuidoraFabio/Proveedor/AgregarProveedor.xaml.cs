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
	public partial class AgregarProveedor : ContentPage
	{
		public AgregarProveedor()
		{
			InitializeComponent();
		}
        private async void BtnGuardarPro_Clicked(object sender, EventArgs e)
        {
            Models.Proveedor proveedor = new Models.Proveedor()
            {
                nombre = nombrePEntry.Text,
                direccion = direccionPEntry.Text,
                contacto = contactoPEntry.Text,
                telefono = Convert.ToInt32(telefonoPEntry.Text)
            };

            var json = JsonConvert.SerializeObject(proveedor);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/proveedores/agregarProveedor.php", content);
           


            if (result.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
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