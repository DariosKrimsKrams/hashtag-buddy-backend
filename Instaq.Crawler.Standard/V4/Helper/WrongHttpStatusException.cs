namespace Instaq.Crawler.V4.Helper
{
    using System;
    using System.Net;

    public class WrongHttpStatusException : Exception
    {
        public HttpStatusCode StatusCode
        {
            get;
        }

        public WrongHttpStatusException(string message) : base(message)
        {
        }

        public WrongHttpStatusException(HttpStatusCode code)
        {
            this.StatusCode = code;
        }

    }
}
