using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.UWP.Helper;
using LaunchPal.Manager;

[assembly: Xamarin.Forms.Dependency(typeof(LiveTileImplementation))]
namespace LaunchPal.UWP.Helper
{
    class LiveTileImplementation : ICreateTile
    {
        public void SetLaunch()
        {
            var tileNotification = CreateLiveTile(LaunchPal.App.Settings.TileData);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private TileNotification CreateLiveTile(Tile tileInformation)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText01);

            var tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = tileInformation.Name;
            tileTextAttributes[1].InnerText = tileInformation.Message;
            tileTextAttributes[2].InnerText = $"{tileInformation.Net.Day}-{tileInformation.Net.Month}-{tileInformation.Net.Year} {tileInformation.Net.Hour}:{tileInformation.Net.Minute}";


            var tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Square150x150Logo.scale-200.png");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");

            var wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150PeekImage02);

            var wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            wideTileTextAttributes[0].AppendChild(wideTileXml.CreateTextNode(tileInformation.Name));
            wideTileTextAttributes[1].AppendChild(wideTileXml.CreateTextNode(tileInformation.Message));
            wideTileTextAttributes[2].AppendChild(wideTileXml.CreateTextNode(tileInformation.Net.ToString(CultureInfo.CurrentCulture)));

            var wideTileImageAttributes = wideTileXml.GetElementsByTagName("image");
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Wide310x150Logo.scale-200.png");
            ((XmlElement)wideTileImageAttributes[0]).SetAttribute("alt", "red graphic");

            var node = tileXml.ImportNode(wideTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual")?.Item(0)?.AppendChild(node);

            return new TileNotification(tileXml) { ExpirationTime = tileInformation.Net };
        }
    }
}
