using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using DistribuidoraFabio.Droid;
using DistribuidoraFabio.Helpers;
using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomAutoSuggestBox), typeof(MyRenderer))]
namespace DistribuidoraFabio.Droid
{
	class MyRenderer : AutoSuggestBoxRenderer
	{
		public MyRenderer(Context context) : base(context)
		{

		}
		protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.ImeOptions = (ImeAction)ImeFlags.NoExtractUi; // set keyboard size according to UI
			}
		}
	}
}