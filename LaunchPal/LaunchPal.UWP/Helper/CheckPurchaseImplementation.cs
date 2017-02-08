using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using LaunchPal.Interface;
using LaunchPal.UWP.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckPurchaseImplementation))]
namespace LaunchPal.UWP.Helper
{
    internal class CheckPurchaseImplementation : ICheckPurchase
    {
        public bool HasPurchasedPlus()
        {
#if DEBUG
            var licenseInformation = CurrentAppSimulator.LicenseInformation;
#else
            var licenseInformation = CurrentApp.LicenseInformation;
#endif

            var iap1 = licenseInformation.ProductLicenses["LaunchPal Plus"].IsActive;
            var iap2 = licenseInformation.ProductLicenses["LaunchPal_Plus"].IsActive;

            return iap1 || iap2;
        }

        public bool PurchasePlus()
        {
            if (HasPurchasedPlus())
                return true;

            try
            {
#if DEBUG
                var result = CurrentAppSimulator.RequestProductPurchaseAsync("LaunchPal Plus").GetAwaiter().GetResult();
#else
                var result = CurrentApp.RequestProductPurchaseAsync("LaunchPal Plus").GetAwaiter().GetResult();
#endif

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
