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
        public string Download(Uri link){
//            JsonConvert.SerializeObject(obj, Formatting.None,
//new JsonSerializerSettings
//{ContractResolver = new CamelCasePropertyNamesContractResolver()})

            var target = "http://static.scripting.com";
            var client = new RestClient(target);

            var request = new RestRequest("houston/rivers/apple/River3.js", method: Method.GET);

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
