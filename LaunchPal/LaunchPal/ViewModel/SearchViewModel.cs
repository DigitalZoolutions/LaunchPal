using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LaunchPal.Enums;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class SearchViewModel : ErrorViewModel, INotifyPropertyChanged
    {
        private ListView _searchResult;

        public ListView SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                OnPropertyChanged(nameof(SearchResult));
            }
        }

        public SearchViewModel()
        {

        }

        public SearchViewModel(List<LaunchData> launchList, OrderBy order)
        {
            SearchResult = new SearchListTemplate(launchList, order);
        }

        public void SearchForLaucnhes(string searchString)
        {
            SearchResult = new SearchListTemplate(CacheManager.TryGetLaunchesBySearchString(searchString).Result, OrderBy.Net);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
