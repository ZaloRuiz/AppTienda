using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Finanzas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BalanceAnual : ContentPage
	{
		private decimal _v_enero = 8510;
		private decimal _v_febrero = 7462;
		private decimal _v_marzo = 11478;
		private decimal _v_abril = 6841;
		private decimal _v_mayo = 8230;
		private decimal _v_junio = 7102;
		private decimal _v_julio = 5400;
		private decimal _v_agosto = 6923;
		private decimal _v_septiembre = 8987;
		private decimal _v_octubre = 7458;
		private decimal _v_noviembre = 9610;
		private decimal _v_diciembre = 10775;
		public BalanceAnual()
		{
			InitializeComponent();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			
			float _enero = (float)_v_enero;
			float _febrero = (float)_v_febrero;
			float _marzo = (float)_v_marzo;
			float _abril = (float)_v_abril;
			float _mayo = (float)_v_mayo;
			float _junio = (float)_v_junio;
			float _julio = (float)_v_julio;
			float _agosto = (float)_v_agosto;
			float _septiembre = (float)_v_septiembre;
			float _octubre = (float)_v_octubre;
			float _noviembre = (float)_v_noviembre;
			float _diciembre = (float)_v_diciembre;

			var entries = new[]
			{
				new ChartEntry(_enero)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Enero",
						TextColor = SKColors.White,
						ValueLabel = _enero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_febrero)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Febrero",
						TextColor = SKColors.White,
						ValueLabel = _febrero.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_marzo)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Marzo",
						TextColor = SKColors.White,
						ValueLabel = _marzo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_abril)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Abril",
						TextColor = SKColors.White,
						ValueLabel = _abril.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_mayo)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Mayo",
						TextColor = SKColors.White,
						ValueLabel = _mayo.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_junio)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Junio",
						TextColor = SKColors.White,
						ValueLabel = _junio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_julio)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Julio",
						TextColor = SKColors.White,
						ValueLabel = _julio.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_agosto)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Agosto",
						TextColor = SKColors.White,
						ValueLabel = _agosto.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_septiembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Septiembre",
						TextColor = SKColors.White,
						ValueLabel = _septiembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_octubre)
					{
						Color = SKColor.Parse("#000FFF"),
						Label = "Octubre",
						TextColor = SKColors.White,
						ValueLabel = _octubre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_noviembre)
					{
						Color = SKColor.Parse("#FF0000"),
						Label = "Noviembre",
						TextColor = SKColors.White,
						ValueLabel = _noviembre.ToString(),
						ValueLabelColor = SKColors.White
					},
				new ChartEntry(_diciembre)
					{
						Color = SKColor.Parse("#059B00"),
						Label = "Diciembre",
						TextColor = SKColors.White,
						ValueLabel = _diciembre.ToString(),
						ValueLabelColor = SKColors.White,
					}
			};
			grafico1.Chart = new BarChart() { Entries = entries, BackgroundColor = SKColor.Parse("#40616B"), LabelColor = SKColors.White, LabelTextSize = 30,
			LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, };
		}
	}
}