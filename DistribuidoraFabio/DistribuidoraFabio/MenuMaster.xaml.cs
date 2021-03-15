using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DistribuidoraFabio
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuMaster : ContentPage
	{
		public ListView ListView;

		public MenuMaster()
		{
			InitializeComponent();

			BindingContext = new MenuMasterViewModel();
			ListView = MenuItemsListView;
		}

		class MenuMasterViewModel : INotifyPropertyChanged
		{
			public ObservableCollection<MenuMasterMenuItem> MenuItems { get; set; }

			public MenuMasterViewModel()
			{
				MenuItems = new ObservableCollection<MenuMasterMenuItem>(new[]
				{
					new MenuMasterMenuItem { Id = 0, Title = "Inicio", TargetType = typeof(MenuDetail), icon="icon_venta.png" },
					new MenuMasterMenuItem { Id = 1, Title = "Compras", TargetType = typeof(Compra.ListaCompra), icon="icon_producto.png" },
					new MenuMasterMenuItem { Id = 2, Title = "Pedidos", TargetType = typeof(Venta.ListaPedidos), icon="icon_compra.png" },
					new MenuMasterMenuItem { Id = 3, Title = "Clientes", TargetType = typeof(Cliente.ListaCliente), icon="icon_cliente.png"},
					new MenuMasterMenuItem { Id = 4, Title = "Productos", TargetType = typeof(Producto.ListaProducto), icon="icon_cliente.png" },
					new MenuMasterMenuItem { Id = 5, Title = "Finanzas", TargetType = typeof(Finanzas.IndexFinanzas), icon="fina.png" },
					new MenuMasterMenuItem { Id = 6, Title = "Reportes", TargetType = typeof(Reportes.R_DetalleVenta), icon="fina.png" },
					new MenuMasterMenuItem { Id = 7, Title = "Inventario", TargetType = typeof(Inventario.InventarioGeneral), icon="icon_venta.png" },
					new MenuMasterMenuItem { Id = 8, Title = "Vendedor", TargetType = typeof(Vendedor.ListaVendedor), icon="icon_vendedor.png" },
					new MenuMasterMenuItem { Id = 9, Title = "Costos", TargetType = typeof(Finanzas.ListaCostos), icon="fina.png" },
					//new MenuMasterMenuItem { Id = 1, Title = "Inventario", TargetType = typeof(Inventario.InventarioGeneral), icon="icon_venta.png" },
					//new MenuMasterMenuItem { Id = 2, Title = "Ventas", TargetType = typeof(Venta.ListaVenta), icon="icon_compra.png" },
					//new MenuMasterMenuItem { Id = 3, Title = "Compras", TargetType = typeof(Compra.ListaCompra), icon="icon_producto.png" },
					//new MenuMasterMenuItem { Id = 4, Title = "Producto", TargetType = typeof(Producto.ListaProducto), icon="icon_cliente.png" },
					//new MenuMasterMenuItem { Id = 5, Title = "Cliente", TargetType = typeof(Cliente.ListaCliente), icon="icon_cliente.png"},
					//new MenuMasterMenuItem { Id = 6, Title = "Proveedor", TargetType = typeof(Proveedor.ListaProveedor), icon="icon_proveedor.png" },
					//new MenuMasterMenuItem { Id = 7, Title = "Vendedor", TargetType = typeof(Vendedor.ListaVendedor), icon="icon_vendedor.png" },
					//new MenuMasterMenuItem { Id = 8, Title = "Finanzas", TargetType = typeof(MenuDetail), icon="fina.png" },
					//new MenuMasterMenuItem { Id = 9, Title = "Agenda", TargetType = typeof(MenuDetail), icon="agendaicono.png" },
					//new MenuMasterMenuItem { Id = 10, Title = "Scaner", TargetType = typeof(MenuDetail), icon="icon_app.png" },
				});
			}

			#region INotifyPropertyChanged Implementation
			public event PropertyChangedEventHandler PropertyChanged;
			void OnPropertyChanged([CallerMemberName] string propertyName = "")
			{
				if (PropertyChanged == null)
					return;

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			#endregion
		}
	}
}