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
        public MarginFrame(int margin, Color backgroundColor)
        {
            Padding = new Thickness(margin);
            OutlineColor = backgroundColor;
            BackgroundColor = backgroundColor;
            HasShadow = false;
        }
    }
}
