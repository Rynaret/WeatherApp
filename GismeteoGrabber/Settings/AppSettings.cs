namespace GismeteoGrabber.Settings
{
    public class AppSettings
    {
        public int GrabPeriodInMinutes { get; set; }
        public GismeteoSettings Gismeteo { get; set; }
        public SystemSettings System { get; set; }
    }
}
