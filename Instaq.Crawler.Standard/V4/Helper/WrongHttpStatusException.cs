namespace Instaq.Crawler.Standard
{
    using System;
    using System.Net;

    public class WrongHttpStatusException : Exception
    {
        public HttpStatusCode StatusCode
        {
            get;
            private set;
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
