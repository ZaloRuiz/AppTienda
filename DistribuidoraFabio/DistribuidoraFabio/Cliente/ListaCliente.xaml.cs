using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Cliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaCliente : ContentPage
	{
		public ListaCliente()
		{
			InitializeComponent();
		}
		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new AgregarCliente());
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/clientes/listaCliente.php");
            var usuarios = JsonConvert.DeserializeObject<List<Models.Cliente>>(response);

            listaCliente.ItemsSource = usuarios;
        }
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Models.Cliente;
            await Navigation.PushAsync(new EditarBorrarCliente(detalles.id_cliente, detalles.codigo_c, detalles.nombre_cliente, detalles.ubicacion_latitud, detalles.ubicacion_longitud, 
                detalles.telefono, detalles.direccion_cliente, detalles.razon_social, detalles.nit));
        }
    }
}