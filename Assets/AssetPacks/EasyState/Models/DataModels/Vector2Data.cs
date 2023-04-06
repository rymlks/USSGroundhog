using System.Globalization;
using UnityEngine;

namespace EasyState.DataModels
{
    public class Vector2Data
    {
        public string XY { get; set; }

        public Vector2Data(Vector2 vector)
        {
            XY = $"{vector.x.ToString(CultureInfo.InvariantCulture.NumberFormat)},{vector.y.ToString(CultureInfo.InvariantCulture.NumberFormat)}";
        }

        public static implicit operator Vector2(Vector2Data data)
        {
            if (data is null || data?.XY is null)
            {
                return Vector2.zero;
            }
            string[] val = data.XY.Split(',');
            return new Vector2(ParseFloat(val[0]), ParseFloat(val[1]));
        }

        public static implicit operator Vector2Data(Vector2 vector2)
        {
            return new Vector2Data(vector2);
        }

        private static float ParseFloat(string input) => float.Parse(input, CultureInfo.InvariantCulture.NumberFormat);
    }
}