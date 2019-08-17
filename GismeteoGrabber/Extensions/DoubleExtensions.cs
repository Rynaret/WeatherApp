using System.Globalization;

namespace GismeteoGrabber.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToDouble(this string str)
        {
            return double.Parse(str.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}
