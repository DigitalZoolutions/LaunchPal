using System.Threading.Tasks;

namespace LaunchPal.Interface
{
    public interface ICheckPurchase
    {
        bool HasPurchasedPlus();

        Task<bool> PurchasePlus();

        bool CanPurchasePlus();
    }
}
