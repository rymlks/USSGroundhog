using UnityEngine;

namespace EasyState.Core.Utility
{
    public static class EditorColors
    {
        public static Color BackgroundColor = new Color(0.08627451f, 0.1176471f, 0.1764706f);
        public static Color ToolbarColor = new Color(0.05490196f, 0.07450981f, 0.1137255f);
        public static Color Blue = new Color(0, 0.2666667f, 0.372549f);
        public static Color Blue_Focus = new Color(0, 0.6705883f, 0.9568627f);
        public static Color Green = new Color(0.2705882f, 0.5058824f, 0.282353f);
        public static Color Green_Focus = new Color(0.2941177f, 0.6941177f, 0.3098039f);
        public static Color LightBackgroundColor = new Color(0.13671875f, 0.17875f, 0.25f);
        public static Color Orange = new Color(0.7333333f, 0.3254902f, 0.1843137f);
        public static Color Orange_Focus = new Color(0.9882353f, 0.3333333f, 0.1058824f);
        public static Color Pink = new Color(0.6901961f, 0, 0.2509804f);
        public static Color Pink_Focus = new Color(1, 0, 0.3647059f);
        public static Color Purple = new Color(0.3490196f, 0.07843138f, 0.3960785f);
        public static Color Purple_Focus = new Color(0.6156863f, 0.1333333f, 0.6980392f);
        public static Color White = new Color(0.5686275f, 0.5686275f, 0.5686275f);
        public static Color White_Focus = new Color(0.8235294f, 0.8235294f, 0.8235294f);
        public static Color Yellow = new Color(0.496f, 0.3767432f, 0.002710389f);
        public static Color Yellow_Focus = new Color(1, 0.7607843f, 0.003921569f);
        public static Color DarkPink = new Color(.5f, 0, 0.1818182f);
        public static Color DarkPink_Focus = new Color(0.7924528f, 0, 0.2890123f);

        public static Color ColorToFocusColor(Color nonFocusColor)
        {
            if (nonFocusColor == Green)
            {
                return Green_Focus;
            }
            else if (nonFocusColor == Orange)
            {
                return Orange_Focus;
            }
            else if (nonFocusColor == Purple)
            {
                return Purple_Focus;
            }
            else if (nonFocusColor == Yellow)
            {
                return Yellow_Focus;
            }
            else if (nonFocusColor == Blue)
            {
                return Blue_Focus;
            }
            throw new System.InvalidOperationException("Unsupported Non-Focus Color");
        }

        public static Color WithOpacity(this Color color, float opacity = .5f)
        {
            return new Color(color.r, color.g, color.b, opacity);
        }
    }
}