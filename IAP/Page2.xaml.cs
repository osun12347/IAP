using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Page2 : Page
    {
        //MainPage rootPage = MainPage.Current;
        //private int numberOfConsumablesPurchased = 0;
        private int coin = 0;
        private HashSet<Guid> consumedTransactionIds = new HashSet<Guid>();
        public Page2()
        {
            this.InitializeComponent();
            Init();
            tb1.Text = coin.ToString();
        }

        private async void Init()
        {
            StorageFile proxyFile = await Package.Current.InstalledLocation.GetFileAsync("in-app-purchase-consumables.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
        }

        private async void Buy(object sender, RoutedEventArgs e)
        {
            PurchaseResults purchaseResults = await CurrentAppSimulator.RequestProductPurchaseAsync("product1");
            switch (purchaseResults.Status)
            {
                case ProductPurchaseStatus.Succeeded:
                    //Local fulfillment
                    //GrantFeatureLocally(purchaseResults.TransactionId);
                    ////Reporting product fulfillment to the Store
                    //FulfillProduct(purchaseResults.TransactionId);
                    NotifyUser("You bought product1. Transaction Id: " + purchaseResults.TransactionId);
                    break;

                case ProductPurchaseStatus.NotFulfilled:
                    // First check for unfulfilled purchases and grant any unfulfilled purchases from an earlier transaction.
                    // Once products are fulfilled pass the product ID and transaction ID to currentAppSimulator.reportConsumableFulfillment
                    // To indicate local fulfillment to the Windows Store.
                    //NotifyUser("You have an unfulfilled copy of " + info.nameRun.Text + ". Fulfill it before purchasing another one.");
                    NotifyUser("You have an unfulfilled copy of product1.Fulfill it before purchasing another one.");
                    break;
                case ProductPurchaseStatus.NotPurchased:
                    NotifyUser("product1 was not purchased.");
                    break;
            }
        }
        private async void FulfillProduct(Guid transactionId)
        {
            try
            {
                FulfillmentResult result = await CurrentAppSimulator.ReportConsumableFulfillmentAsync("product1", transactionId);
                switch (result)
                {
                    case FulfillmentResult.Succeeded:
                        NotifyUser("You bought and fulfilled product 1.");
                        break;
                    case FulfillmentResult.NothingToFulfill:
                       NotifyUser("There is no purchased product 1 to fulfill.");
                        break;
                    case FulfillmentResult.PurchasePending:
                        NotifyUser("You bought product 1. The purchase is pending so we cannot fulfill the product.");
                        break;
                    case FulfillmentResult.PurchaseReverted:
                        NotifyUser("You bought product 1, but your purchase has been reverted.");
                        // Since the user's purchase was revoked, they got their money back.
                        // You may want to revoke the user's access to the consumable content that was granted.
                        break;
                    case FulfillmentResult.ServerError:
                        NotifyUser("You bought product 1. There was an error when fulfilling.");
                        break;
                }
            }
            catch (Exception)
            {
                //rootPage.NotifyUser("You bought Product 1. There was an error when fulfilling.", NotifyType.ErrorMessage);
            }
        }
        private void GrantFeatureLocally(Guid transactionId)
        {
            consumedTransactionIds.Add(transactionId);
            // Grant the user their content. You will likely increase some kind of gold/coins/some other asset count.
            coin += 50;
            tb1.Text = coin.ToString();

        }

        private bool IsLocallyFulfilled(Guid transactionId)
        {
            return consumedTransactionIds.Contains(transactionId);
        }

        private async void FulfillPreviousPurchase(object sender, RoutedEventArgs e)
        {
            try
            {
                IReadOnlyList<UnfulfilledConsumable> consumables = await CurrentAppSimulator.GetUnfulfilledConsumablesAsync();
                string logMessage = "Number of unfulfilled consumables: " + consumables.Count;

                foreach (UnfulfilledConsumable consumable in consumables)
                {
                    logMessage += "\nProduct Id: " + consumable.ProductId + " Transaction Id: " + consumable.TransactionId;
                    // This is where you would grant the user their consumable content and call currentApp.reportConsumableFulfillment
                    // For demonstration purposes, we leave the purchase unfulfilled.
                    if (!IsLocallyFulfilled(consumable.TransactionId))
                    {
                        GrantFeatureLocally(consumable.TransactionId);
                    }
                    FulfillProduct(consumable.TransactionId);
                }
                //NotifyUser("product1 was fulfilled.You are now able to buy another one");
                //tb2.Text = logMessage.ToString();
            }
            catch (Exception)
            {
                //rootPage.NotifyUser("GetUnfulfilledConsumablesAsync API call failed", NotifyType.ErrorMessage);
            }

        }

        public void NotifyUser(string strMessage)
        {

            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                StatusBlock.Visibility = Visibility.Visible;
                StatusBlock.Foreground= new SolidColorBrush(Windows.UI.Colors.White);
                stackpanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                stackpanel.Visibility = Visibility.Collapsed;
            }
        }


        private async void ShowUnfulfilledConsumables(object sender, RoutedEventArgs e)
        {
            try
            {
                IReadOnlyList<UnfulfilledConsumable> consumables = await CurrentAppSimulator.GetUnfulfilledConsumablesAsync();
                string logMessage = "Number of unfulfilled consumables: " + consumables.Count;

                foreach (UnfulfilledConsumable consumable in consumables)
                {
                    logMessage += "\nProduct Id: " + consumable.ProductId + " Transaction Id: " + consumable.TransactionId;
                    // This is where you would grant the user their consumable content and call currentApp.reportConsumableFulfillment
                    // For demonstration purposes, we leave the purchase unfulfilled.
                }
                NotifyUser(logMessage);
            }
            catch (Exception)
            {
                NotifyUser("GetUnfulfilledConsumablesAsync API call failed");
            }
        }
    }


    }

