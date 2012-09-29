using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;


namespace HobiHobi.Core.Utils
{
    public class UrlShortener
    {
        public static Result<string> GetGoogle(string url)
        {
            var http = new HttpClient();
            
            var payload = new { longUrl = url };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var httpPayload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var output = http.PostAsync("https://www.googleapis.com/urlshortener/v1/url", httpPayload).ContinueWith(response =>
                    {
                        return response.Result.Content.ReadAsStringAsync().Result;
                    });

                dynamic res = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(output.Result);
                string id = res.id;
                return Result<string>.True(id);
            }
            catch (Exception ex)
            {
                return Result<string>.False(ex);
            }
        }
    }
}
