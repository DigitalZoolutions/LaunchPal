﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;
using LaunchPal.Helper;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class AstronautsPage : LoadingPage
    {
        public AstronautsViewModel Context { get; set; }

        public AstronautsPage()
        {
            Title = "Astronauts in Space";
            BackgroundColor = Theme.BackgroundColor;
            Content = GenerateWaitingMessage("Contacting Astronauts...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(1000, () =>
                {
                    Context = new AstronautsViewModel();
                    Content = GenerateView();
                });
            };

        }

        private Xamarin.Forms.View GenerateView()
        {
            if (Context.ExceptionType != null)
            {
                return Context.GenerateErrorView(this);
            }
            else
            {
                var grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                    }
                };

                Context.Astronouts.ItemTapped += NewsListOnItemTapped;

                grid.Children.Add(new MarginFrame(5, 10, 17, 5, Theme.BackgroundColor)
                {
                    Content = new Frame()
                    {
                        BackgroundColor = Theme.FrameColor,
                        OutlineColor = Theme.FrameBorderColor,
                        Content = new StackLayout
                        {
                            Children =
                            {
                                new Label
                                {
                                    Text = $"Astronauts in space: {Context.NumberOfAstronautsInSpace}",
                                    TextColor = Theme.TextColor,
                                    FontSize = 24,
                                    FontAttributes = FontAttributes.Bold,
                                    HorizontalTextAlignment = TextAlignment.Center
                                }
                            }
                        }
                    }
                }, 0, 0);

                grid.Children.Add(Context.Astronouts, 0, 1);

                return grid;
            }
        }

        private void NewsListOnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            if (itemTappedEventArgs.Item.GetType() != typeof(Person))
                return;

            var austronaut = (itemTappedEventArgs.Item as Person);

            var url = Device.Idiom == TargetIdiom.Phone ? austronaut?.Twitter.Insert(8, "mobile.") : austronaut?.Twitter;
            var title = $"Twitter - {austronaut?.Name}";

            if (string.IsNullOrEmpty(url))
            {
                url = Device.Idiom == TargetIdiom.Phone ? austronaut?.Biolink.Insert(10, ".m") : austronaut?.Biolink;
                title = $"Wiki - {austronaut?.Name}";
            }
                

            if (string.IsNullOrEmpty(url))
            {
                DisplayAlert("No aditional information", "This astronaut has no aditional information to link to.", "Continue");
                return;
            }
                

            if (App.Settings.SuccessfullIap)
            {
                var mainPage = this.Parent.Parent as MainPage;

                if (mainPage?.GetType() != typeof(MainPage))
                    return;

                mainPage.NavigateTo(new WebPage(url, title));
            }
            else
            {
                Device.OpenUri(new Uri(url));
            }
        }
    }
}