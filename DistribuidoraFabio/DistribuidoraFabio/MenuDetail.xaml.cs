using DistribuidoraFabio.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuDetail : ContentPage
	{
		private int _cantCerv = 0;
		private int _cantGase = 0;
		private int _CantTotal = 0;
		private double _PromCerv = 0;
		private double _PromGase = 0;
		private double _PromedioTotal = 0;
		private decimal _TendCerv = 0;
		private decimal _TendGase = 0;
		private decimal _TendenciaTotal = 0;
		private int _cantCervCli = 0;
		private int _cantGaseCli = 0;
		private int _CantCliTotal = 0;
		private int _IdTpGase = 0;
		private int _IdTpCerv = 0;
		private string _FechaInicio;
		private string _FechaFinal;
		private string vendedorPick;
		private int idVendedorSelected = 0;
		List<Vendedores> vendedorList = new List<Vendedores>();
		public MenuDetail()
		{
			InitializeComponent();
			GetDataVendedor();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					CultureInfo ci = Thread.CurrentThread.CurrentCulture;
					//string fechaMesAnterior = ci.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-1).Month).ToString().ToUpper();
					//txtMesAnterior.Text = fechaMesAnterior;
					string fechaMesActual = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToString().ToUpper();
					txtFechaMes.Text = fechaMesActual;
					txtFechaActual.Text = DateTime.Today.ToString("dd/MM/yyy");
					//FechaMesActual
					DateTime fechaMesAct = DateTime.Today;
					DateTime primerDiaMesAct = new DateTime(fechaMesAct.Year, fechaMesAct.Month, 1);
					DateTime ultimoDiaMesAct = primerDiaMesAct.AddMonths(1).AddDays(-1);
					//const FormatException format = "yyyy'-'MM'-'dd";
					_FechaInicio = primerDiaMesAct.ToString("yyyy-MM-dd");
					_FechaFinal = ultimoDiaMesAct.ToString("yyyy-MM-dd");
					//GetPromedio();
					//await Task.Delay(300);
					//GetClientes();
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		public async void GetPromedio()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client1 = new HttpClient();
					var response1 = await client1.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tipoproductos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response1);
					if (tipoproductos != null)
					{
						foreach (var item in tipoproductos)
						{
							if (item.nombre_tipo_producto == "Cerveza")
							{
								_IdTpCerv = item.id_tipoproducto;
							}
							else if (item.nombre_tipo_producto == "Gaseosa")
							{
								_IdTpGase = item.id_tipoproducto;
							}
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
				}
				try
				{
					_cantCerv = 0;
					_PromCerv = 0;
					txtPromedioCerv.Text = string.Empty;
					CantidadClientesVentas _cantClieVent = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						id_tipo_producto = _IdTpCerv,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var json = JsonConvert.SerializeObject(_cantClieVent);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadPorEmpleado.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var lista_Ccerv = JsonConvert.DeserializeObject<List<CantidadPorEmpleado>>(jsonR);
					if (lista_Ccerv != null)
					{
						foreach (var item in lista_Ccerv)
						{
							if (_IdTpCerv == item.id_tipo_producto)
							{
								_cantCerv = _cantCerv + item.cantidad;
							}
						}
					}
					else
					{
						await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo por favor", "OK");
					}
					_PromCerv = _cantCerv / 24;
					txtPromedioCerv.TargetValue = _PromCerv;
					//Gaseosa
					_cantGase = 0;
					_PromGase = 0;
					txtPromedioGase.Text = string.Empty;
					CantidadClientesVentas _cantCliVenGase = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						id_tipo_producto = _IdTpGase,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var jsonG = JsonConvert.SerializeObject(_cantCliVenGase);
					var contentG = new StringContent(jsonG, Encoding.UTF8, "application/json");
					HttpClient clientG = new HttpClient();
					var resultG = await clientG.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadPorEmpleado.php", contentG);

					var jsonRG = await resultG.Content.ReadAsStringAsync();
					var lista_Cgase = JsonConvert.DeserializeObject<List<CantidadPorEmpleado>>(jsonRG);
					if (lista_Cgase != null)
					{
						foreach (var item in lista_Cgase)
						{
							if (_IdTpGase == item.id_tipo_producto)
							{
								_cantGase = _cantGase + item.cantidad;
							}
						}
					}
					_PromGase = _cantGase / 24;
					txtPromedioGase.TargetValue = _PromGase;
					//Total
					_CantTotal = 0;
					_PromedioTotal = 0;
					txtPromedioTotal.Text = string.Empty;
					CantidadClientesVentas _caClVeGaTotal = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var jsonT = JsonConvert.SerializeObject(_caClVeGaTotal);
					var contentT = new StringContent(jsonT, Encoding.UTF8, "application/json");
					HttpClient clientT = new HttpClient();
					var resultT = await clientG.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadPorEmpleadoTotal.php", contentT);

					var jsonRT = await resultT.Content.ReadAsStringAsync();
					var lista_CgaseT = JsonConvert.DeserializeObject<List<CantidadPorEmpleado>>(jsonRT);
					if (lista_CgaseT != null)
					{
						foreach (var item in lista_CgaseT)
						{
							_CantTotal = _CantTotal + item.cantidad;
						}
					}
					else
					{
						await DisplayAlert("Error", "Query null", "OK");
					}
					_PromedioTotal = _CantTotal / 24;
					txtPromedioTotal.TargetValue = _PromedioTotal;
					//reiniciar valores
					_TendCerv = 0;
					_TendGase = 0;
					txtTendenciaCerv.Text = string.Empty;
					txtTendenciaGase.Text = string.Empty;
					_TendenciaTotal = 0;
					txtTendendciaTotal.Text = string.Empty;
					_TendCerv = (decimal)(_PromCerv / 24);
					_TendGase = (decimal)(_PromGase / 24);
					txtTendenciaCerv.TargetValue = (double)_TendCerv;
					txtTendenciaGase.TargetValue = (double)_TendGase;

					_TendenciaTotal = (decimal)(_PromedioTotal / 24);
					txtTendendciaTotal.TargetValue = (double)_TendenciaTotal;
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
		private async void GetClientes()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client1 = new HttpClient();
					var response1 = await client1.GetStringAsync("https://dmrbolivia.com/api_distribuidora/tipoproductos/listaTipoproducto.php");
					var tipoproductos = JsonConvert.DeserializeObject<List<Tipo_producto>>(response1);
					if (tipoproductos != null)
					{
						foreach (var item in tipoproductos)
						{
							if (item.nombre_tipo_producto == "Cerveza")
							{
								_IdTpCerv = item.id_tipoproducto;
							}
							else if (item.nombre_tipo_producto == "Gaseosa")
							{
								_IdTpGase = item.id_tipoproducto;
							}
						}
					}
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.ToString(), "OK");
				}
				try
				{
					//Cerveza
					_cantCervCli = 0;
					txtClienteCerv.Text = string.Empty;
					CantidadClientesVentas _cantClieVent = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						id_tipo_producto = _IdTpCerv,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var json = JsonConvert.SerializeObject(_cantClieVent);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpClient client = new HttpClient();
					var result = await client.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadClientesVentas.php", content);

					var jsonR = await result.Content.ReadAsStringAsync();
					var lista_ventas = JsonConvert.DeserializeObject<List<CantidadClientesVentas>>(jsonR);
					if (lista_ventas != null)
					{
						foreach (var item in lista_ventas)
						{
							if (_IdTpCerv == item.id_tipo_producto)
							{
								_cantCervCli = item.cl_count;
							}
						}
					}
					txtClienteCerv.TargetValue = _cantCervCli;
				}
				catch (Exception err)
				{
					await DisplayAlert("Error", err.Message, "OK");
				}
				try
				{
					//Gaseosa
					_cantGaseCli = 0;
					txtClienteGase.Text = string.Empty;
					CantidadClientesVentas _cantCliVenGase = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						id_tipo_producto = _IdTpGase,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var jsonG = JsonConvert.SerializeObject(_cantCliVenGase);
					var contentG = new StringContent(jsonG, Encoding.UTF8, "application/json");
					HttpClient clientG = new HttpClient();
					var resultG = await clientG.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadClientesVentas.php", contentG);

					var jsonRG = await resultG.Content.ReadAsStringAsync();
					var lista_ventaG = JsonConvert.DeserializeObject<List<CantidadClientesVentas>>(jsonRG);
					if (lista_ventaG != null)
					{
						foreach (var item in lista_ventaG)
						{
							if (_IdTpGase == item.id_tipo_producto)
							{
								_cantGaseCli = item.cl_count;
							}
						}
					}
					txtClienteGase.TargetValue = _cantGaseCli;
					//Total
					_CantCliTotal = 0;
					txtClienteTotal.Text = string.Empty;
					CantidadClientesVentas _caClVeGa = new CantidadClientesVentas()
					{
						id_vendedor = idVendedorSelected,
						fecha_inicio = Convert.ToDateTime(_FechaInicio),
						fecha_final = Convert.ToDateTime(_FechaFinal)
					};
					var jsonT = JsonConvert.SerializeObject(_caClVeGa);
					var contentT = new StringContent(jsonT, Encoding.UTF8, "application/json");
					HttpClient clientT = new HttpClient();
					var resultT = await clientT.PostAsync("https://dmrbolivia.com/api_distribuidora/reportes/CantidadClientesVentasTotal.php", contentT);

					var jsonRT = await resultT.Content.ReadAsStringAsync();
					var lista_ventaT = JsonConvert.DeserializeObject<List<CantidadClientesVentas>>(jsonRT);
					if (lista_ventaT != null)
					{
						foreach (var item in lista_ventaT)
						{
							_CantCliTotal = item.cl_count;
						}
					}
					else
					{
						await DisplayAlert("Error", "Algo salio mal, intentelo de nuevo por favor", "OK");
					}
					txtClienteTotal.TargetValue = _CantCliTotal;
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
		protected override bool OnBackButtonPressed()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var result = await this.DisplayAlert("SALIR", "Quiere salir de la aplicacion?", "SI", "NO");
				if (result) await this.Navigation.PopModalAsync();
			});
			return true;
		}
		private async void GetDataVendedor()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				try
				{
					HttpClient client = new HttpClient();
					var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/vendedores/listaVendedores.php");
					var vendedores = JsonConvert.DeserializeObject<List<Vendedores>>(response).ToList();
					pickerEmpleado.ItemsSource = vendedores;
					foreach (var item in vendedores)
					{
						vendedorList.Add(item);
						idVendedorSelected = item.id_vendedor;
					}
				}
				catch (Exception error)
				{
					await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error", "Necesitas estar conectado a internet", "OK");
			}
		}
		private async void pickerEmpleado_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;
				if (selectedIndex != -1)
				{
					vendedorPick = picker.Items[selectedIndex];
					try
					{
						foreach (var item in vendedorList)
						{
							if (vendedorPick == item.nombre)
							{
								idVendedorSelected = item.id_vendedor;
							}
						}
					}
					catch (Exception err)
					{
						await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
					}
					
					GetPromedio();
					await Task.Delay(300);
					GetClientes();
				}
			}
			catch (Exception err)
			{
				await DisplayAlert("ERROR", "Algo salio mal, intentelo de nuevo", "OK");
			}
		}
	}
}