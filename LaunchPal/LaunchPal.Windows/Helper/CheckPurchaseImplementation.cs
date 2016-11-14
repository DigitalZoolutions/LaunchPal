using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using LaunchPal.Interface;
using LaunchPal.Windows.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckPurchaseImplementation))]
namespace LaunchPal.Windows.Helper
{
    class CheckPurchaseImplementation : ICheckPurchase
    {
        public bool HasPurchasedPlus()
        {
            try
            {
                return CurrentApp.LicenseInformation.ProductLicenses["LaunchPal Plus"].IsActive ||
                    CurrentApp.LicenseInformation.ProductLicenses["LaunchPal_Plus"].IsActive;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PurchasePlus()
        {
            if (HasPurchasedPlus())
                return true;

            try
            {
                var result = await CurrentApp.RequestProductPurchaseAsync("LaunchPal Plus");

                switch (result.Status)
                {
                    case ProductPurchaseStatus.Succeeded:
                    case ProductPurchaseStatus.AlreadyPurchased:
                        return HasPurchasedPlus();
                    case ProductPurchaseStatus.NotPurchased:
                    case ProductPurchaseStatus.NotFulfilled:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CanPurchasePlus()
        {
            return true;
        }
    }
}
