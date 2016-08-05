using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        public Page3()
        {
            this.InitializeComponent();
            Init();
        }

        private async void Init()
        {
            StorageFile proxyFile = await Package.Current.InstalledLocation.GetFileAsync("receipt.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
        }

        private async void GetReceipt(object sender, RoutedEventArgs e)
        {
            try
            {
                String receipt = await CurrentAppSimulator.GetAppReceiptAsync();
                String prettyReceipt = XElement.Parse(receipt).ToString(SaveOptions.None);
                tb1.Text = prettyReceipt;
            }
            catch (Exception)
            {
              
            }
        }
    }
}
