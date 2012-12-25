using HobiHobi.Core.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.OpmlEditor
{
    public class EditorDocument
    {
        public string Id           { get; set; }
        public string Title        { get; set; }
        public string DateCreated  { get; set; }
        public string DateModified { get; set; }
        public string OwnerName    { get; set; }
        public string OwnerEmail   { get; set; }
        public string OwnerId      { get; set; }
        public bool   IsPublic     { get; set; }
        
        public List<EditorOutline> Body { get; set; }

        public Opml RenderToOpml()
        {
            var opml = new Opml();
            opml.DateCreated = DateTime.UtcNow;
            opml.DateModified = DateTime.UtcNow;
            opml.Title = "OPML Document with Id " + Id;
            opml.OwnerName = "temporary";
            opml.OwnerName = "temporary@gmail.com";

            foreach (var x in Body)
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

        private void TraverseEditor(Outline outline, EditorOutline editorOutline)
        {
            if (outline != null)
            {
                foreach (var a in outline.Attributes)
                {
                    if (a.Key == "text")
                        editorOutline.Data = a.Value;
                    else
                        editorOutline.Attr.Add(a.Key, a.Value);

                    foreach (var x in outline.Outlines)
                    {
                        var o = new EditorOutline();
                        editorOutline.Children.Add(o);
                        TraverseEditor(x, o);
                    }
                }
            }
        }

        public void FromDocument(EditorDocument document)
        {
            this.Title = document.Title;
            this.OwnerName = document.OwnerName;
            this.OwnerEmail = document.OwnerEmail;
            this.OwnerId = document.OwnerId;
            this.Body = document.Body;
            this.DateModified = DateTime.UtcNow.ToString("R");
        }

        public void FromOpml(string id, Opml opmlFile)
        {
            this.Id = id;
            this.Title = opmlFile.Title;
            if(opmlFile.OwnerId != null)
                this.OwnerId = opmlFile.OwnerId.ToString();
            this.OwnerName = opmlFile.OwnerName;
            this.OwnerEmail = opmlFile.OwnerEmail;
            this.DateCreated = opmlFile.DateCreated.HasValue ? opmlFile.DateCreated.Value.ToString("R") : DateTime.UtcNow.ToString("R");
            this.DateModified = opmlFile.DateModified.HasValue ? opmlFile.DateModified.Value.ToString("R") : DateTime.UtcNow.ToString("R");
            Body = new List<EditorOutline>();
            foreach (var outline in opmlFile.Outlines)
            {
                var editorOutline = new EditorOutline();
                Body.Add(editorOutline);
                TraverseEditor(outline, editorOutline);
            }
        }
    }
}
