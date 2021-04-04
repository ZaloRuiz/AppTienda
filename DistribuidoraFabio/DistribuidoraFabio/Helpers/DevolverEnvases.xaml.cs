using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DevolverEnvases : PopupPage
	{
		private int _envasesDevuelto;
		private int _totalEnvases;
		public DevolverEnvases()
		{
			InitializeComponent();
		}
		protected override void OnDisappearing()
		{

		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
		protected override bool OnBackgroundClicked()
		{
			return false;
		}

		private async void btnCancelar_Clicked(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}

		private async void btnAceptar_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(entryCantDevuelta.Text) || (!string.IsNullOrEmpty(entryCantDevuelta.Text)))
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					try
					{
						_envasesDevuelto = Convert.ToInt32(entryCantDevuelta.Text);
						_totalEnvases = App._envasesDeuda - _envasesDevuelto;
						if(_envasesDevuelto > App._envasesDeuda)
						{
							DetalleVenta _detalleVenta = new DetalleVenta()
							{
								id_dv = App._idDVenvase,
								envases = _totalEnvases
							};
							var json = JsonConvert.SerializeObject(_detalleVenta);
							var content = new StringContent(json, Encoding.UTF8, "application/json");
							HttpClient client = new HttpClient();
							var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/ventas/editarEnvases.php", content);

							if (result.StatusCode == HttpStatusCode.OK)
							{
								await DisplayAlert("EDITADO", "Se edito correctamente", "OK");
								MessagingCenter.Send<DevolverEnvases>(this, "Hi");
								await PopupNavigation.Instance.PopAsync();
							}
							else
							{
								await DisplayAlert("Error", result.StatusCode.ToString(), "OK");
								await PopupNavigation.Instance.PopAsync();
							}
						}
						else
						{
							await DisplayAlert("Error", "La cantidad de envases devueltos es mayor a los adeudados", "OK");
						}
					}
					catch (Exception err)
					{
						await DisplayAlert("ERROR", err.ToString(), "OK");
					}
				}
				else
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "El campo de Cantidad esta vacio", "OK");
			}
		}
	}
}