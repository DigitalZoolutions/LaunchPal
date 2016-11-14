using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;
using Xamarin.Forms;

namespace LaunchPal.CustomElement
{
    class MarginFrame : Frame
    {
        public MarginFrame(int margin)
        {
            Padding = new Thickness(margin);
            OutlineColor = Theme.BackgroundColor;
            BackgroundColor = Theme.BackgroundColor;
            HasShadow = false;
        }

        public MarginFrame(int margin, Color backgroundColor)
        {
            Padding = new Thickness(margin);
            OutlineColor = backgroundColor;
            BackgroundColor = backgroundColor;
            HasShadow = false;
        }

        public MarginFrame(int marginLeft, int marginTop, int marginRight, int marginBottom)
        {
            Padding = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
            OutlineColor = Theme.BackgroundColor;
            BackgroundColor = Theme.BackgroundColor;
            HasShadow = false;
        }

        public MarginFrame(int marginLeft, int marginTop, int marginRight, int marginBottom, Color backgroundColor)
        {
            Padding = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
            OutlineColor = backgroundColor;
            BackgroundColor = backgroundColor;
            HasShadow = false;
        }
    }
}
