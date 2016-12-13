using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using LaunchPal.Droid.Helper;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace LaunchPal.Droid
{
    [Activity(Label = "LaunchPal", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
         WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static InAppBillingServiceConnection billingService;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            billingService = new InAppBillingServiceConnection(this,
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvr7MDRR1hINCrRbp/1Uj8eJGiCKnfOU9Q1WCYueuDqPL9B5eIMlyebexeMFIzlf2HwfRVc2SUDzrRbv7EY3ry/vBrOucx2JgjqL72srs7AYkA5WtXMAEZ/tvRx8oa0etl3QmzL0gRHyljDgtchCMhRKkUYX8wg0nysFGOdrnvTqyKCPf1D5hP/TFoQn1ujey8bzYUT7g0W+WvP9K8ObnsOxvWeVqUD/UYaTnizIgqdHawGi2ODiZQR8F593F128FILFipWezhov2VES98EfYq3w3HQ0kiGjroc7l1H/SEqSfeL1DvWaFqUJz9c9NvFhYbbxYhLfAs0YUf3F/Yqx6WwIDAQAB");
            billingService.OnConnected += async () =>
            {

                // Load inventory or available products
                await GetInventory();

                // Load any items already purchased
                LoadPurchasedItems();
            };
            billingService.Connect();

            var appVersion =
                $"{PackageManager.GetPackageInfo(PackageName, 0).VersionName}.{PackageManager.GetPackageInfo(PackageName, 0).VersionCode}";

            var id = Intent.GetStringExtra("id");

            if (string.IsNullOrEmpty(id))
            {
                global::Xamarin.Forms.Forms.Init(this, bundle);
                Xamarin.FormsMaps.Init(this, bundle);
                LoadApplication(new App());
            }
            else
            {
                global::Xamarin.Forms.Forms.Init(this, bundle);
                Xamarin.FormsMaps.Init(this, bundle);
                LoadApplication(new App(int.Parse(id)));
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Ask the open service connection's billing handler to process this request
            try
            {
                billingService?.BillingHandler.HandleActivityResult(requestCode, resultCode, data);
            }
            catch (Exception)
            {
                //log it or something?
            }

        }

        private async Task GetInventory()
        {
            IList<Product> products = null;
            try
            {
                // Ask the open connection's billing handler to return a list of available products for the 
                // given list of items.
                // NOTE: We are asking for the Reserved Test Product IDs that allow you to test In-App
                // Billing without actually making a purchase.
                products = await billingService.BillingHandler.QueryInventoryAsync(new List<string>   {
                    ReservedTestProductIDs.Purchased,
                    ReservedTestProductIDs.Canceled,
                    ReservedTestProductIDs.Refunded,
                    ReservedTestProductIDs.Unavailable,
                    "launchpal_plus"
                }, ItemType.Product);
            }
            catch (Exception)
            {
                CheckPurchaseImplementation.Products = new List<Product>();
            }

            // Were any products returned?
            if (products == null)
            {
                CheckPurchaseImplementation.Products = new List<Product>();
            }
            else
            {
                foreach (var product in products)
                {
                    CheckPurchaseImplementation.Products.Add(product);
                }
            }
        }

        private void LoadPurchasedItems()
        {
            // Ask the open connection's billing handler to get any purchases
            var purchases = billingService.BillingHandler.GetPurchases(ItemType.Product);

            foreach (var purchase in purchases)
            {
                CheckPurchaseImplementation.Purchases.Add(purchase);
            }

        }


        protected override void Dispose(bool disposing)
        {
            billingService?.Disconnect();

            base.Dispose(disposing);
        }
    }
}

