using DistribuidoraFabio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Inventario
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventarioGeneral : ContentPage
	{
		public InventarioGeneral()
		{
			InitializeComponent();
			BindingContext = new InventarioGeneralVM();
		}
	}
}