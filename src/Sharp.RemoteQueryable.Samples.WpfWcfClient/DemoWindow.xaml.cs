using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Sharp.RemoteQueryable.Samples.Model;
using Sharp.RemoteQueryable.Samples.WpfWcfClient;

namespace DataVirtualization
{
  /// <summary>
  /// Interaction logic for DemoWindow.xaml
  /// </summary>
  public partial class DemoWindow
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DemoWindow"/> class.
    /// </summary>
    public DemoWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var customerProvider = new DemoDeveloperProvider();
      this.ListView.ItemsSource = new VirtualizingCollection<Developer>(customerProvider, 10);
    }
  }
}
