using System.Globalization;
using UnityEngine;

namespace EasyState.DataModels
{
    public class ColorData
    {
        public string RGBA { get; set; }

        public ColorData(Color color)
        {
            RGBA = $"{color.r.ToString(CultureInfo.InvariantCulture.NumberFormat)},{color.g.ToString(CultureInfo.InvariantCulture.NumberFormat)},{color.b.ToString(CultureInfo.InvariantCulture.NumberFormat)},{color.a.ToString(CultureInfo.InvariantCulture.NumberFormat)}";
        }

        public static implicit operator Color(ColorData data)
        {
            if (data is null)
            {
                return new Color();
            }
            string[] vals = data.RGBA.Split(',');
            return new Color(
                ParseFloat(vals[0]),
                ParseFloat(vals[1]),
                ParseFloat(vals[2]),
                ParseFloat(vals[3])
                );
        }

        public static implicit operator ColorData(Color color) => new ColorData(color);

        private static float ParseFloat(string input) => float.Parse(input, CultureInfo.InvariantCulture.NumberFormat);
    }
}