using System;

namespace GismeteoGrabber.Exceptions
{
    public class ParsingException : Exception
    {
        public ParsingException(string? message) : base(message)
        {
        }
    }
}
