using System;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class LaunchPalPlusPage : ContentPage
    {
        private LaunchPalPlusViewModel _context;

        public LaunchPalPlusPage()
        {
            Title = "LaunchPal Plus";

            _context = new LaunchPalPlusViewModel();

            if (_context.HasError)
            {
                Content = _context.GenerateErrorView(this);
                return;
            }

            GeneratePage();
        }

        private void GeneratePage()
        {
            var topGrid = GenerateTopGrid();
            var middleGrid = GenerateMiddleGrid();
            var purchaseButton = GenerateButton();

            var mainGrid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                },
            };

            mainGrid.Children.Add(topGrid, 0, 0);
            mainGrid.Children.Add(middleGrid, 0, 1);

            Grid.SetColumnSpan(topGrid, 4);
            Grid.SetColumnSpan(middleGrid, 4);

            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(40, GridUnitType.Absolute)}
                },
                Children =
                {
                    {
                        new ScrollView
                        {
                            Content = mainGrid
                        }, 0, 0
                    },
                    {
                        purchaseButton, 0, 1
                    }
                    
                }
            };
        }

        private Grid GenerateMiddleGrid()
        {
            return new Grid();
        }

        private Button GenerateButton()
        {
            var button = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = "Purchase LaunchPal Plus now",
                TextColor = Theme.ButtonTextColor,
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            button.Clicked += async (sender, args) =>
            {
                var result = _context.PurchasePlus();
                if (result)
                {
                    await
                        DisplayAlert("Thank you for your support!",
                            "Thank you for purchasing LaunchPal Plus, the features has now been unlocked in the app",
                            "Continue");

                    var mainPage = this.Parent.Parent as MainPage;

                    if (mainPage?.GetType() != typeof(MainPage))
                        return;

                    mainPage.NavigateTo(new OverviewPage());
                }
                else
                {
                    await
                        DisplayAlert("Issue during Transaction",
                            "There were a issue while processing the purchase, please try again.",
                            "Continue");
                }
            };

            if (_context.CanPurchase == false)
            {
                button.IsEnabled = false;
            }

            return button;
        }

        private static Grid GenerateTopGrid()
        {
            var leftStack = new MarginFrame(20)
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "LaunchPal Plus"
                        },
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Start,
                            Text =
                                $"With LaunchPal Plus you get even more out of the app and at a low, one time cost to enchance your experiance even more.{Environment.NewLine}{Environment.NewLine}" +
                                $"Some functions it enables is for example the all new weather forecasts, free search of any planed or launches mission and enable in app viewing of the launch and sattelite view of the launchpad.{Environment.NewLine}{Environment.NewLine}" +
                                $"So get it now, more awsome stuff is on its way!"
                        }
                    }
                }
            };

            var rightStack = new MarginFrame(20)
            {
                Content = new MarginFrame(0, Theme.FrameColor)
                {
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Label
                            {
                                HorizontalTextAlignment = TextAlignment.Center,
                                Text = "Now only",
                                TextColor = Theme.HeaderColor,
                                FontSize = 22
                            },
                            new Label
                            {
                                HorizontalTextAlignment = TextAlignment.Center,
                                Text = "2 USD / 2 Euro",
                                TextColor = Theme.HeaderColor,
                                FontSize = 20
                            }
                        }
                    }
                }
            };

            var topGrid =  new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            topGrid.Children.Add(leftStack, 0, 3, 0, 1);
            topGrid.Children.Add(rightStack, 3, 0);

            return topGrid;
        }
    }
}
