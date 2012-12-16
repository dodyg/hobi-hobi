using HobiHobi.Core.OpmlEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HobiHobi.Core.Framework;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using HobiHobi.Core.Framework.Json;

namespace HobiHobi.Web.Controllers
{
    public class OpmlController : RavenController
    {
        public string ConvertToJson(List<EditorOutline> outlines)
        {
            var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeObject(outlines, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new LowerCaseContractResolver()
            });

            return jsonDoc;
        }

        public T ConvertFromJson<T>(string data)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data, new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new TitleCaseContractResolver()
            });

            return obj;
        }

        public ActionResult Index(string id)
        {
            return View();
        }

        public ActionResult New()
        {
            var id = Guid.NewGuid().ToString();
            return Redirect("/opml?id=" + id);
        }

        public ActionResult GetDocument(string id)
        {
            if (!id.IsNullOrWhiteSpace())
            {
                var doc = this.RavenSession.Load<EditorDocument>(id);
                if (doc == null)
                {
                    doc = new EditorDocument
                    {
                        Id = id,
                        Outlines = new List<EditorOutline>()
                        {
                            new EditorOutline{
                                Data = "Edit"
                            }
                        }
                    };
                }

                return Content(ConvertToJson(doc.Outlines), "application/json");
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult PutDocument(string id, string data)
        {
            var outlines = ConvertFromJson<List<EditorOutline>>(data);

            if (outlines == null)
                throw new ApplicationException("outlines cannot be null");

            var doc = this.RavenSession.Load<EditorDocument>(id);
            if (doc == null)
            {
                doc = new EditorDocument{
                    Id = id,
                    Outlines = outlines
                };
            }else{
                doc.Outlines = outlines;
            }

            this.RavenSession.Store(doc);
            this.RavenSession.SaveChanges();

            return HttpDoc<EmptyResult>.OK(new EmptyResult()).ToJson();
        }

        [HttpGet]
        public ActionResult RenderXml(string id)
        {
            if (!id.IsNullOrWhiteSpace())
            {
                var doc = this.RavenSession.Load<EditorDocument>(id);

                if (doc == null)
                    return HttpNotFound();

                var opml = doc.RenderToOpml();
                var xml = opml.ToXML();

                return Content(xml.ToString(), "text/xml");
            }
            else
                return HttpNotFound();
        }
    }
}
