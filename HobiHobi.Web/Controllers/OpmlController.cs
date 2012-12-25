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
using HobiHobi.Core.Subscriptions;
using HobiHobi.Core.Identity;
using HobiHobi.Core.Utils;

namespace HobiHobi.Web.Controllers
{
    public class OpmlController : RavenController
    {
        public string ConvertToJson(EditorDocument outlines)
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

        public ActionResult GetDocument(string id, bool view = false)
        {
            if (!id.IsNullOrWhiteSpace())
            {
                var edit = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
                var doc = this.RavenSession.Load<EditorDocument>(id);
                if (doc == null && !view)
                {
                    doc = new EditorDocument
                    {
                        Id = id,
                        Body = new List<EditorOutline>()
                        {
                            new EditorOutline {
                                Data = "Edit"
                            }
                        }
                    };
                }
                else if((edit.IsFound && edit.Item.IsOpmlFound(id)) || (doc.IsPublic && view))
                    return Content(ConvertToJson(doc), "application/json");
                
                return Content(null, "application/json");
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult PutDocument(string id, string data)
        {
            if (data.IsNullOrWhiteSpace())
                return HttpDoc<EmptyResult>.NotFound("Missing").ToJson();
            var outlines = ConvertFromJson<EditorDocument>(data);

            if (outlines == null)
                throw new ApplicationException("outlines cannot be null");

            var doc = this.RavenSession.Load<EditorDocument>(id);
            if (doc == null)
            {
                doc = outlines;
                doc.Id = id;
                doc.DateCreated = DateTime.UtcNow.ToString("R");
                doc.DateModified = DateTime.UtcNow.ToString("R");
            }
            else
            {
                doc.FromDocument(outlines);
            }

            this.RavenSession.Store(doc);
            this.RavenSession.SaveChanges();

            var transient = CookieMonster.GetFromCookie<TransientAccount>(Request.Cookies[TransientAccount.COOKIE_NAME]);
            if (!transient.IsFound)
            {
                var init = new TransientAccount();
                init.OpmlGuids.Add(doc.Id);
                Response.Cookies.Add(CookieMonster.SetCookie(init, TransientAccount.COOKIE_NAME));
            }
            else
            {
                transient.Item.OpmlGuids.Add(doc.Id);
                Response.Cookies.Add(CookieMonster.SetCookie(transient.Item, TransientAccount.COOKIE_NAME));
            }

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

        [HttpPost]
        public ActionResult UploadOpml(string id, HttpPostedFileBase file)
        {
            System.IO.StreamReader stream = new System.IO.StreamReader(file.InputStream);
            string x = stream.ReadToEnd();
            Opml opmlFile = new Opml();
            opmlFile.LoadFromXML(x);
            EditorDocument doc = new EditorDocument();
            doc.FromOpml(id, opmlFile);
            this.RavenSession.Store(doc);
            this.RavenSession.SaveChanges();
            return Redirect("/opml?id=" + id );
        }

        [HttpGet]
        public ActionResult Html(string id)
        {
            return View();
        }

        public ActionResult Latest()
        {
            var docs = this.RavenSession.Query<EditorDocument>().Where(x => x.IsPublic).OrderByDescending(x => x.DateCreated).Take(30).ToList();
            return View(docs);
        }
    }
}
