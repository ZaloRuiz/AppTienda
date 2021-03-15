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
            try
			{
                Tipo_producto tipo_Producto = new Tipo_producto()
                {
                    id_tipoproducto = Convert.ToInt32(idproductoentry.Text),
                    nombre_tipo_producto = nombreTpEntry.Text
                };

                var json = JsonConvert.SerializeObject(tipo_Producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/agregarTipoproducto.php", content);

                if (result.StatusCode == HttpStatusCode.Created)
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