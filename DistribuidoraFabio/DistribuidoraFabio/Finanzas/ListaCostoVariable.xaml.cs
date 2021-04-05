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
	public partial class ListaCostoVariable : ContentPage
	{
		private int _mesQuery;
		private int _yearQuery;
		public ListaCostoVariable()
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
					Costo_variable _costoVariable = new Costo_variable()
					{
						mes_cv = _mesQuery,
						gestion_cv = _yearQuery
					};
					var json = JsonConvert.SerializeObject(_costoVariable);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoVariableQuery.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var dataCostoVar = JsonConvert.DeserializeObject<List<Costo_variable>>(jsonR);

					listCostoVariable.ItemsSource = dataCostoVar;
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

		private async void toolbarCV_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AgregarCostoVariable());
		}
		private async void listCostoVariable_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var detalles = e.Item as Costo_variable;
			await Navigation.PushAsync(new EditarBorrarCostoVariable(detalles.id_cv, detalles.nombre_cv, detalles.monto_cv, detalles.mes_cv, detalles.tipo_gasto_cv,
				detalles.fecha_cv, detalles.descripcion_cv));
		}
	}
}