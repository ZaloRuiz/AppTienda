using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Proveedor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaProveedor : ContentPage
	{
		public ListaProveedor()
		{
			InitializeComponent();
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarProveedor());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/proveedores/listaProveedor.php");
            var productos = JsonConvert.DeserializeObject<List<Models.Proveedor>>(response);

            listaProv.ItemsSource = productos;

        }
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Models.Proveedor;
            await Navigation.PushAsync(new EditarBorrarProveedor(detalles.id_proveedor, detalles.nombre, detalles.direccion, detalles.contacto, detalles.telefono));
        }
    }
}