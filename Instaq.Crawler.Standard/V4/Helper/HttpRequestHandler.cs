namespace Instaq.Crawler.Standard
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using Instaq.Crawler.V4.Requests;
    using HtmlAgilityPack;
    using Newtonsoft.Json;

    public class HttpRequestHandler : IRequestHandler
    {
        private static readonly Regex FindJson = new Regex(
            @"\s*window\s*\.\s*_sharedData\s*\=\s*(.*)\s*\;\s*",
            RegexOptions.Compiled);

        private readonly HttpClient httpClient;

        public HttpRequestHandler()
        {
            this.httpClient = new HttpClient();
        }

        public HtmlNode FetchDocument(string url)
        {
            try
            {
                var result = this.httpClient.GetAsync(url).Result;
                var status = result.StatusCode;
                if (status != HttpStatusCode.OK)
                {
                    throw new WrongHttpStatusException(status);
                }

                var document = new HtmlDocument();
                document.Load(result.Content.ReadAsStreamAsync().Result);
                return document.DocumentNode;
            }
            catch (WrongHttpStatusException)
            {
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception while fetching url " + url);
                return null;
            }
        }

        public dynamic FetchNode(string url)
        {
            var document = this.FetchDocument(url);
            return this.GetScriptNodeData(document);
        }

        public dynamic GetScriptNodeData(HtmlNode document)
        {
            var scriptNode = document?.SelectNodes("//script")
                ?.FirstOrDefault(n => n.InnerText.Contains("window._sharedData = "));

            if (scriptNode is null)
            {
                return null;
            }

            var match = FindJson.Match(scriptNode.InnerText);
            if (!match.Success || !match.Groups[1].Success)
            {
                return null;
            }

            var json = match.Groups[1].Value;
            return JsonConvert.DeserializeObject(json);
        }
    }
}
