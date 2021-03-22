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
	public partial class ListaCostos : ContentPage
	{
		public ListaCostos()
		{
			InitializeComponent();
			pickerMes.ItemsSource = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Noviembre", "Diciembre" }; 
			pickerYear.ItemsSource = new List<string> { "2021", "2022", "2023" };
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

		}
	}
}