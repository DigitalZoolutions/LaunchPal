﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class NewsViewModel : ErrorViewModel, INotifyPropertyChanged
    {
        private ListView _newsList;

        public ListView NewsList
        {
            get { return _newsList; }
            set
            {
                _newsList = value;
                OnPropertyChanged(nameof(NewsList));
            }
        }

        public NewsViewModel()
        {
            List<NewsFeed> news = new List<NewsFeed>();

            try
            {
                news = CacheManager.TryGetNewsFeed().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SetError(ex);
            }

            NewsList = new NewsListTemplate(news);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
