using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BusyPopup : PopupPage
	{
		public BusyPopup(string BusyReason)
		{
			InitializeComponent();
			busyReason.Text = BusyReason;
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
	}
}