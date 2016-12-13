using System.Threading.Tasks;

namespace LaunchPal.Interface
{
    public interface ICheckPurchase
    {
        bool HasPurchasedPlus();

        bool PurchasePlus();

        bool CanPurchasePlus();
    }
}
