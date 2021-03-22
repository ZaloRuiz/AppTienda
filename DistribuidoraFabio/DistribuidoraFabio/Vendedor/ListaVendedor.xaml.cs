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

namespace DistribuidoraFabio.Vendedor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaVendedor : ContentPage
	{
		public ListaVendedor()
		{
			InitializeComponent();
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarVendedor());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/vendedores/listaVendedores.php");
					var vendedores = JsonConvert.DeserializeObject<List<Vendedores>>(response);

					listaVendedor.ItemsSource = vendedores;
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
        }
        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalles = e.Item as Vendedores;
            await Navigation.PushAsync(new EditarBorrarVendedor(detalles.id_vendedor, detalles.nombre,
                detalles.telefono, detalles.direccion, detalles.numero_cuenta, detalles.cedula_identidad, detalles.usuario, detalles.password));
        }
    }
}