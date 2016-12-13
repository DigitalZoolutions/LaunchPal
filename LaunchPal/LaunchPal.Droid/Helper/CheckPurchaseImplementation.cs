using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;
using Xamarin.InAppBilling;

[assembly: Dependency(typeof(CheckPurchaseImplementation))]
namespace LaunchPal.Droid.Helper
{
    class CheckPurchaseImplementation : ICheckPurchase
    {
        public static List<Product> Products { get; set; }

        public static List<Purchase> Purchases { get; set; }

        public bool HasPurchasedPlus()
        {
            if (Purchases == null)
            {
                return false;
            }

            foreach (var purchase in Purchases)
            {
                if (purchase.ProductId == "launchpal_plus")
                    return true;
            }

            return false;
        }

        public bool PurchasePlus()
        {
            var tmpProducts = MainActivity.billingService.BillingHandler.QueryInventoryAsync(new List<string>   {
                ReservedTestProductIDs.Purchased,
                ReservedTestProductIDs.Canceled,
                ReservedTestProductIDs.Refunded,
                ReservedTestProductIDs.Unavailable
            }, ItemType.Product).GetAwaiter().GetResult();

            List<Product> products = new List<Product>();

            foreach (var product in tmpProducts)
            {
                products.Add(product);
            }

            MainActivity.billingService.BillingHandler.BuyProduct(products.Find(x => x.ProductId == "launchpal_plus"));
            return true;
        }

        public bool CanPurchasePlus()
        {
            return true;
        }
    }
}