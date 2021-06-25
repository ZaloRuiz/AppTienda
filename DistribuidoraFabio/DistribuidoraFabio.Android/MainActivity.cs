using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin.Forms;
using DistribuidoraFabio.Helpers;

namespace DistribuidoraFabio.Droid
{
    [Activity(Label = "App Distribuidora", Icon = "@drawable/app_icono", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            //Reportes de ventas diarias
            MessagingCenter.Subscribe<ListaR_VentaDiaria>(this, "allowPortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            //Reportes de ventas diarias
            MessagingCenter.Subscribe<ListaR_VentaDiaria>(this, "preventPortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            //Reportes de inventario diario
            MessagingCenter.Subscribe<ListaR_InventarioDia>(this, "allowPortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            //Reportes de inventario diario
            MessagingCenter.Subscribe<ListaR_InventarioDia>(this, "preventPortrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });

            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
        
 
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
