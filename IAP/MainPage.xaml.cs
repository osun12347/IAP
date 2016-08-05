using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<PageBean> pageList { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            pageList = new ObservableCollection<PageBean> {
            //new PageBean() { title = "Scenario 1 - Basic", type = typeof(Page1) },
            //new PageBean() { title = "Scenario 2 - Headers", type = typeof(Page2) },
            //new PageBean() { title = "Scenario 3 - Tabs", type = typeof(Page3) }

              new PageBean("Durable",typeof(Page1)),
              new PageBean("Consumable",typeof(Page2)),
              new PageBean("Get Receipt",typeof(Page3))
        };
            listView1.ItemsSource = pageList;
            listView1.SelectedIndex = 0;
        }
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listview = sender as ListView;
            PageBean bean = listview.SelectedItem as PageBean;
            if (bean != null)
            {
                myFrame.Navigate(bean.type);
            }

        }


        public class PageBean
        {
            public PageBean(string title, Type type)
            {
                this.title = title;
                this.type = type;
            }
            public string title { get; set; }
            public Type type { get; set; }
        }

    }
 }

