using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class SearchViewModel : INotifyPropertyChanged
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

        public SearchViewModel(List<LaunchData> launchList)
        {
            SearchResult = new SearchListTemplate(launchList);
        }

        public void SearchForLaucnhes(string searchString)
        {
            SearchResult = new SearchListTemplate(CacheManager.TryGetLaunchesBySearchString(searchString).Result);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
