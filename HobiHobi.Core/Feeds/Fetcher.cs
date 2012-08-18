using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace HobiHobi.Core.Feeds
{
    public class Fetcher
    {
        public string Download(string host, string path){
            var target = host;
            var client = new RestClient(target);

            var request = new RestRequest(path, method: Method.GET);
            request.AddHeader("Accept", "*/*");
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            var scrub = response.Content.Replace("onGetRiverStream (", "")
                .Replace(")", "");

            return scrub;
        }

        public FeedsRiver Serialize(string json)
        {
            var feeds = JsonConvert.DeserializeObject<FeedsRiver>(json, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

            return feeds;
        }
    }
}
