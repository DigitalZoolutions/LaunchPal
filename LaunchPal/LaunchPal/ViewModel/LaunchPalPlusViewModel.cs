using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LaunchPal.Annotations;
using LaunchPal.Interface;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class LaunchPalPlusViewModel : ErrorViewModel, INotifyPropertyChanged
    {
        private bool _hasPurchased;
        private bool _canPurchase;

        public bool CanPurchase
        {
            get { return _canPurchase; }
            set
            {
                _canPurchase = value;
                OnPropertyChanged(nameof(CanPurchase));
            }
        }

        public LaunchPalPlusViewModel()
        {
            CanPurchase = App.Settings.SuccessfullIap == false && DependencyService.Get<ICheckPurchase>().CanPurchasePlus() == true;
        }

        public bool HasPurchased
        {
            get { return _hasPurchased; }
            set
            {
                _hasPurchased = value;
                OnPropertyChanged(nameof(HasPurchased));
            }
        }

        internal bool PurchasePlus()
        {
            var result = DependencyService.Get<ICheckPurchase>().PurchasePlus();
            if (result)
            {
                CanPurchase = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
