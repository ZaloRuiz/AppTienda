﻿using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new AgregarTipoProducto(), true);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tipoproductos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response);

					listaTipoP.ItemsSource = tipoproductos;
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
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalle = e.Item as Tipo_producto;
            await Shell.Current.Navigation.PushAsync(new EditarBorrarTipoProducto(detalle.id_tipoproducto, detalle.nombre_tipo_producto), true);
        }
    }
}