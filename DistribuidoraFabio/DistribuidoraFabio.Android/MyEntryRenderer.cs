using Android.Content;

using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using DistribuidoraFabio.Helpers;
using DistribuidoraFabio.Droid;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace DistribuidoraFabio.Droid
{
	class MyEntryRenderer : EntryRenderer
	{
		public MyEntryRenderer(Context context) : base(context)
		{
		}
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.ImeOptions = (ImeAction)ImeFlags.NoExtractUi; // set keyboard size according to UI
			}
		}
	}
}