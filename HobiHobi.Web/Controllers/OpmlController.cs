using HobiHobi.Core.OpmlEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using Newtonsoft.Json.Serialization;

namespace HobiHobi.Web.Controllers
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }

    public class OpmlController : RavenController
    {
        public ActionResult Index(string id)
        {
            if (!id.IsNullOrWhiteSpace())
            {
                var outlines = new List<EditorOutline>();
                outlines.Add(new EditorOutline
                {
                    Data = "Hello world",
                    Attrs = new Dictionary<string, string>()
                    {
                        { "type", "include" },
                        { "language", "en" },
                        { "url", "http://www.cnn.com"}
                    },
                    Children = new List<EditorOutline>()
                    {
                        new EditorOutline {
                            Data = "This is second greeting"
                        },
                        new EditorOutline {
                            Data = "This is third outliene"
                        }
                    }
                });

                var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(outlines, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver()
                });

                return Content(jsonDoc, "application/json");
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult PutEditor(string id, List<EditorOutline> outlines)
        {
            return HttpDoc<EmptyResult>.OK(new EmptyResult()).ToJson();
        }
    }
}
