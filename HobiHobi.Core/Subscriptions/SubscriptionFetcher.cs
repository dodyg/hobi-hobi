using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Subscriptions
{
    /// <summary>
    /// Fetch opml subscription list
    /// </summary>
    public class SubscriptionFetcher
    {
        public string Download(string host, string path)
        {
            var target = host;
            var client = new RestClient(target);

            var request = new RestRequest(path, method: Method.GET);
            request.AddHeader("Accept", "*/*");
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
            return response.Content;
        }
    }
}
