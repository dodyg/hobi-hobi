using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HobiHobi.Core.Framework
{
    public static class HttpDocExtensions
    {
        public static ActionResult ToJson<T>(this HttpDoc<T> data, Formatting format = Formatting.None, bool convertEnumToString = true, bool isoDateTime = true)
        {
            var jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = format;
            jsonNetResult.Data = data;
            if (convertEnumToString)
                jsonNetResult.SerializerSettings.Converters.Add(new StringEnumConverter());

            if (isoDateTime)
                jsonNetResult.SerializerSettings.Converters.Add(new IsoDateTimeConverter());

            return jsonNetResult;
        }
    }
}
