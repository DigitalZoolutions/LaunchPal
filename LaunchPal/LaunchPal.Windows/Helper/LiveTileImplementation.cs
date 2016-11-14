using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Windows.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(LiveTileImplementation))]
namespace LaunchPal.Windows.Helper
{
    class LiveTileImplementation : ICreateTile
    {
        public void SetLaunch()
        {
            var tileNotification = CreateLiveTile(LaunchPal.App.Settings.SimpleLaunchDataData);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private TileNotification CreateLiveTile(SimpleLaunchData simpleLaunchDataInformation)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText01);

            var tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = simpleLaunchDataInformation.Name;
            tileTextAttributes[1].InnerText = simpleLaunchDataInformation.Message ?? "No mission description";
            tileTextAttributes[2].InnerText = $"{simpleLaunchDataInformation.Net.Day}-{simpleLaunchDataInformation.Net.Month}-{simpleLaunchDataInformation.Net.Year} {simpleLaunchDataInformation.Net.Hour}:{simpleLaunchDataInformation.Net.Minute}";


            var tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Square150x150Logo.scale-200.png");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");

            var wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150PeekImage02);

            var wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            wideTileTextAttributes[0].AppendChild(wideTileXml.CreateTextNode(simpleLaunchDataInformation.Name));
            wideTileTextAttributes[1].AppendChild(wideTileXml.CreateTextNode(simpleLaunchDataInformation.Message ?? "No mission description"));
            wideTileTextAttributes[2].AppendChild(wideTileXml.CreateTextNode(simpleLaunchDataInformation.Net.ToString(CultureInfo.CurrentCulture)));

            var wideTileImageAttributes = wideTileXml.GetElementsByTagName("image");
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Wide310x150Logo.scale-200.png");
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("alt", "red graphic");

            var node = tileXml.ImportNode(wideTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual")?.Item(0)?.AppendChild(node);

            return new TileNotification(tileXml) { ExpirationTime = simpleLaunchDataInformation.Net };
        }
    }
}
