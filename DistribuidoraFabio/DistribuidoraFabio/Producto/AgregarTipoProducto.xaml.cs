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

namespace DistribuidoraFabio.Producto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgregarTipoProducto : ContentPage
	{
		public AgregarTipoProducto()
		{
			InitializeComponent();
		}
        private async void BtnGuardarTP_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(nombreTpEntry.Text) || (!string.IsNullOrEmpty(nombreTpEntry.Text)))
                {
                    try
                    {
                        Tipo_producto tipo_Producto = new Tipo_producto()
                        {
                            nombre_tipo_producto = nombreTpEntry.Text
                        };

                        var json = JsonConvert.SerializeObject(tipo_Producto);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient client = new HttpClient();
                        var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/agregarTipoproducto.php", content);

                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            await DisplayAlert("GUARDADO", "Se agrego correctamente", "OK");
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
                    await DisplayAlert("Campo vacio", "El campo de Nombre esta vacio", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
            }
        }
    }
}