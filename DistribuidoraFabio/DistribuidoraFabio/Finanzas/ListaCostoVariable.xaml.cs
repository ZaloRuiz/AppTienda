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
	public partial class ListaCostoVariable : ContentPage
	{
		public ListaCostoVariable()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostoVariable.php");
			var dataCostoVar = JsonConvert.DeserializeObject<List<Costo_variable>>(response);

			listCostoVariable.ItemsSource = dataCostoVar;
		}
	}
}