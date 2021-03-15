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

namespace DistribuidoraFabio.Producto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaTipoProducto : ContentPage
	{
		public ListaTipoProducto()
		{
			InitializeComponent();
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarTipoProducto());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
            var tipoproductos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response);

            listaTipoP.ItemsSource = tipoproductos;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalle = e.Item as Tipo_producto;
            await Navigation.PushAsync(new EditarBorrarTipoProducto(detalle.id_tipoproducto, detalle.nombre_tipo_producto));
        }
    }
}