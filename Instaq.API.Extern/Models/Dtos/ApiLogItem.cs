namespace Instaq.API.Extern.Models.Dtos
{
    using System;

    public class ApiLogItem
    {
        public DateTime RequestTime { get; set; }

        public long ResponseMillis { get; set; }

        public int StatusCode { get; set; }

        public string Method { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public ApiLogItem()
        {
            Method = "";
            Path = "";
            QueryString = "";
            RequestBody = "";
            ResponseBody = "";
        }

        public ApiLogItem(DateTime requestTime, long responseMillis, int statusCode, string method, string path, string queryString, string requestBody, string responseBody)
        {
            RequestTime    = requestTime;
            ResponseMillis = responseMillis;
            StatusCode     = statusCode;
            Method         = method;
            Path           = path;
            QueryString    = queryString;
            RequestBody    = requestBody;
            ResponseBody   = responseBody;
        }
    }
}
