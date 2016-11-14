﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Overview : ContentPage
    {
        public OverviewViewModel Context { get; set; }

        private Grid _pageGrid;

        public Overview()
        {
            Context = new OverviewViewModel();
            BindingContext = Context;

            Icon = "";
            Title = "Overview";
            BackgroundColor = Theme.BackgroundColor;
            Padding = 10;

            if (Context.Error != null)
            {
                Content = Context.Error.GenerateErrorView(this);
                return;
            }

            GenerateGrid();
            PopulateGrid();

            Content = _pageGrid;
        }

        private void PopulateGrid()
        {
            SetLaunchLink();
            SetLaunchInLabel();
            SetLaunchTimer();
            SetLaunchesThisWeek();
            SetLaunchesThisMonth();
            SetLaunchPalPlusButton();
            SetTrackedLaunches();
        }

        private void SetTrackedLaunches()
        {
            if (Context.TrackedLaunches.ItemsSource != null)
            {
                _pageGrid.Children.Add(Context.TrackedLaunches, 0, 5);
                Grid.SetColumnSpan(Context.TrackedLaunches, 6);
            }
            else
            {
                var noTrackedLaunchLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "No launches tracked at this time.",
                    TextColor = Theme.TextColor
                };
                _pageGrid.Children.Add(noTrackedLaunchLabel, 0, 5);
                Grid.SetColumnSpan(noTrackedLaunchLabel, 6);
            }        
        }

        private void SetLaunchPalPlusButton()
        {
            var test1 = App.Settings.SuccessfullIap;
            var test2 = DependencyService.Get<ICheckPurchase>().CanPurchasePlus() == false;

            if (test1 || test2)
                return;

            var launchPalPlusButton = new Button
            {
                Text = "Read about LaunchPal Plus",
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            launchPalPlusButton.Clicked += (sender, args) => (this.Parent.Parent as MainPage)?.NavigateTo(new LaunchPalPlus());

            _pageGrid.Children.Add(launchPalPlusButton, 1, 4);
            Grid.SetColumnSpan(launchPalPlusButton, 4);
        }

        private void SetLaunchesThisMonth()
        {
            var launchThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Theme.TextColor};
            launchThisMonthLabel.SetBinding(Label.TextProperty, new Binding("CurrentMonth"));
            var launchesThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, TextColor = Theme.LinkColor};
            launchesThisMonthLabel.SetBinding(Label.TextProperty, new Binding("LaunchesThisMonthLabel"));
            var launchesThisMonthFrame = new MarginFrame(10, Theme.BackgroundColor)
            {
                Content = new Frame()
                {
                    GestureRecognizers = { NavigateToPageWhenTaped(typeof(Search), Context.LaunchesThisMonth) },
                    BackgroundColor = Theme.FrameColor,
                    OutlineColor = Theme.FrameBorderColor,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            launchThisMonthLabel,
                            launchesThisMonthLabel
                        }
                    }
                }
            };
            _pageGrid.Children.Add(launchesThisMonthFrame, 3, 3);
            Grid.SetColumnSpan(launchesThisMonthFrame, 3);
        }

        private void SetLaunchesThisWeek()
        {
            var launchThisMonthLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Launches this week",
                TextColor = Theme.TextColor
            };

            var launchesThisMonthLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Theme.LinkColor
            };

            launchesThisMonthLabel.SetBinding(Label.TextProperty, new Binding("LaunchesThisWeekLabel"));

            var launchesThisMonthFrame = new MarginFrame(10, Theme.BackgroundColor)
            {
                Content = new Frame()
                {
                    GestureRecognizers = { NavigateToPageWhenTaped(typeof(Search), Context.LaunchesThisWeek) },
                    BackgroundColor = Theme.FrameColor,
                    OutlineColor = Theme.FrameBorderColor,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            launchThisMonthLabel,
                            launchesThisMonthLabel
                        }
                    }
                }
            };
            _pageGrid.Children.Add(launchesThisMonthFrame, 0, 3);
            Grid.SetColumnSpan(launchesThisMonthFrame, 3);
        }

        private void SetLaunchTimer()
        {
            var launchTimerLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };
            launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchTimerText"));
            _pageGrid.Children.Add(launchTimerLabel, 0, 2);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private void SetLaunchInLabel()
        {
            var launchTimerLabel = new Label
            {
                Text = "launches in",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Theme.TextColor,
                FontSize = 18
            };
            _pageGrid.Children.Add(launchTimerLabel, 0, 1);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private void SetLaunchLink()
        {
            var launchTimerLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                GestureRecognizers = { NavigateToPageWhenTaped(typeof(Launch), Context.LaunchId) },
                TextColor = Theme.LinkColor,
                FontSize = 22,
                FontAttributes = FontAttributes.Bold
            };
            launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchName"));
            _pageGrid.Children.Add(launchTimerLabel, 0, 0);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page, int lunchId)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page, lunchId);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);            };

            return tapGestureRecognizer;
        }

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page, List<LaunchData> launches)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page, launches);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);
            };

            return tapGestureRecognizer;
        }

        private void GenerateGrid()
        {
            Grid newGrid = new Grid();

            for (int i = 0; i < 6; i++)
            {
                newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 12; i++)
            {
                newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            _pageGrid = newGrid;
        }

        
    }
}