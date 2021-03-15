using DistribuidoraFabio.Models;
using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
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
	public partial class BalanceMensual : ContentPage
	{
		private decimal _costo_fijo;
		private decimal _costo_variable;
		private decimal _ingresos = 1650;
		public BalanceMensual()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("https://dmrbolivia.com/api_distribuidora/egresos/listaCostos.php");
			var dataCostos = JsonConvert.DeserializeObject<List<Costos>>(response);

			foreach (var item in dataCostos)
			{
				if (item.tipo_costo == "Fijo")
				{
					_costo_fijo = _costo_fijo + item.monto;
				}
				else if (item.tipo_costo == "Variable")
				{
					_costo_variable = _costo_variable + item.monto;
				}
			}
			float _C_Fijo = (float)_costo_fijo;
			float _C_Variable = (float)_costo_variable;
			float _C_Ingresos = (float)_ingresos;
			txtCostosF.Text = "Costos fijos: " + _costo_fijo.ToString() + " Bs.";
			txtCostosV.Text = "Costos variables: " + _costo_variable.ToString() + " Bs.";
			txtIngresos.Text = "Ingresos: " + _ingresos.ToString() + " Bs.";
			var entries = new[]
			{
				new ChartEntry(_C_Fijo)
					{
						Color = SKColor.Parse("#000FFF"),
                    },
				new ChartEntry(_C_Variable)
					{
						Color = SKColor.Parse("#FF0000"),
                    },
				new ChartEntry(_C_Ingresos)
					{
						Color = SKColor.Parse("#059B00"),
                    },
			};
			grafico1.Chart = new DonutChart() { Entries = entries, BackgroundColor = SKColor.Parse("#40616B"), GraphPosition = GraphPosition.AutoFill};

			txtCostosF2.Text = "Costos fijos: " + _costo_fijo.ToString() + " Bs.";
			txtCostosV2.Text = "Costos variables: " + _costo_variable.ToString() + " Bs.";
			var entries2 = new[]
			{
				new ChartEntry(_C_Fijo)
					{
						Color = SKColor.Parse("#000FFF"),
					},
				new ChartEntry(_C_Variable)
					{
						Color = SKColor.Parse("#FF0000"),
					},
			};
			grafico2.Chart = new DonutChart() { Entries = entries2, BackgroundColor = SKColor.Parse("#40616B"), GraphPosition = GraphPosition.AutoFill};
		}
	}
}