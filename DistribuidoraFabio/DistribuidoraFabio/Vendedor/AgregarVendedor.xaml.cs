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
	public partial class AgregarVendedor : ContentPage
	{
		public AgregarVendedor()
		{
			InitializeComponent();
		}
        private async void BtnGuardarVendedor_Clicked(object sender, EventArgs e)
        {
            try
			{
                Vendedores vendedores = new Vendedores()
                {
                    nombre = nombreVendedorEntry.Text,
                    telefono = Convert.ToInt32(telefonoVendedorEntry.Text),
                    direccion = direccionVendedorEntry.Text,
                    numero_cuenta = numero_cuentaVendedorEntry.Text,
                    cedula_identidad = cedula_identidadVendedorEntry.Text
                };

                var json = JsonConvert.SerializeObject(vendedores);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/vendedores/agregarVendedor.php", content);

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
            catch(Exception err)
			{
                await DisplayAlert("ERROR", err.ToString(), "OK");
			}
        }
    }
}