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
	public partial class Kardex : ContentPage
	{
		public Kardex(int id_producto)
		{
			InitializeComponent();
			BindingContext = new KardexVM(id_producto);
		}
	}
}