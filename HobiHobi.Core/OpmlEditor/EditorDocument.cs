using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.OpmlEditor
{
    public class EditorDocument
    {
        public string Id { get; set; }
        public List<EditorOutline> Outlines { get; set; }

        public Opml RenderToOpml()
        {
            var opml = new Opml();
            opml.DateCreated = DateTime.UtcNow;
            opml.DateModified = DateTime.UtcNow;
            opml.Title = "OPML Document with Id " + Id;
            opml.OwnerName = "temporary";
            opml.OwnerName = "temporary@gmail.com";

            foreach (var x in Outlines)
            {
                var o = new Outline();                
                opml.Outlines.Add(o);

                TraverseOpml(x, o);
            }

            return opml;
        }

        void TraverseOpml(EditorOutline outline, Outline ot){
            if (outline != null)
            {
                ot.Attributes.Add("text", outline.Data);
                foreach (var a in outline.Attr)
                {
                    ot.Attributes.Add(a.Key, a.Value);
                }

                foreach (var x in outline.Children)
                {
                    var o = new Outline();
                    ot.Outlines.Add(o);

                    TraverseOpml(x, o);
                }
            }
        }
    }
}
