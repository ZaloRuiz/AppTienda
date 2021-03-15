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

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaCostoFijo : ContentPage
	{
		public ListaCostoFijo()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoFijo.php");
			var dataCostoFijo = JsonConvert.DeserializeObject<List<Costo_fijo>>(response);

			listCostoFijo.ItemsSource = dataCostoFijo;
		}
	}
}