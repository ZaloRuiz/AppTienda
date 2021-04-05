using DistribuidoraFabio.Models;
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

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaCostoFijo : ContentPage
	{
		private int _mesQuery;
		private int _yearQuery;
		public ListaCostoFijo()
		{
			InitializeComponent();
			DateTime fechaMesAct = DateTime.Today;
			_mesQuery = Convert.ToInt32(fechaMesAct.ToString("MM"));
			_yearQuery = Convert.ToInt32(fechaMesAct.ToString("yyyy"));
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					Costo_fijo _costoFijo = new Costo_fijo()
					{
						mes_cf = _mesQuery,
						gestion_cf = _yearQuery
					};
					var json = JsonConvert.SerializeObject(_costoFijo);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoFijoQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataCostoFijo = JsonConvert.DeserializeObject<List<Costo_fijo>>(jsonR);

					listCostoFijo.ItemsSource = dataCostoFijo;
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

		private async void listCostoFijo_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as Costo_fijo;
			await Navigation.PushAsync(new EditarBorrarCostoFijo(detalles.id_cf, detalles.nombre_cf, detalles.monto_cf, detalles.mes_cf, detalles.tipo_gasto_cf,
				detalles.fecha_cf, detalles.descripcion_cf));
		}
		private async void toolbarCF_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AgregarCostoFijo());
		}
	}
}