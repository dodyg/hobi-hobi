﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HobiHobi.Core.Subscriptions
{
    public class Opml
    {
        public string Title { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public Uri OwnerId { get; set; }
        public Uri Docs { get; set; }
        public string ExpansionState { get; set; }
        public int? VertScrollState { get; set; }
        public int? WindowTop { get; set; }
        public int? WindowLeft { get; set; }
        public int? WindowBottom { get; set; }
        public int? WindowRight { get; set; }
        public List<Outline> Outlines { get; private set; }

        public Opml()
        {
            Outlines = new List<Outline>();
        }

        public void LoadFromXML(string xml)
        {
            var elements = XElement.Parse(xml);
            var heads = elements.Element("head").Descendants();

            Func<string, string> selectString = (filter) =>
                {
                    return heads.Where(x => x.Name == filter).Select(x => x.Value).FirstOrDefault();
                };

            Func<string, int?> selectInt = (filter) =>
            {
                return heads.Where(x => x.Name == filter).Select(x => Convert.ToInt32(x.Value)).FirstOrDefault();
            };

            Func<string, DateTime?> selectDate = (filter) =>
            {
                return heads.Where(x => x.Name == filter).Select(x => Convert.ToDateTime(x.Value)).FirstOrDefault();
            };

            Func<string, Uri> selectUri = (filter) =>
            {
                return heads.Where(x => x.Name == filter).Select(x => new Uri(x.Value)).FirstOrDefault();
            };


            Title = selectString("title");
            DateCreated = selectDate("dateCreated");
            DateModified = selectDate("dateModified");
            OwnerName = selectString("ownerName");
            OwnerEmail = selectString("ownerEmail");
            OwnerId = selectUri("ownerId");
            Docs = selectUri("docs");
            ExpansionState = selectString("expansionState");
            VertScrollState = selectInt("vertScrollState");
            WindowTop = selectInt("windowTop");
            WindowLeft = selectInt("windowLeft");
            WindowBottom = selectInt("windowBottom");
            WindowRight = selectInt("windowRight");

            var bodies = elements.Element("body").Descendants();
            //todo: make it recursive
            foreach (var b in bodies)
            {
                var o = new Outline();
                foreach (var att in b.Attributes())
                {
                    o.Attributes[att.Name.ToString()] = att.Value;
                }
                Outlines.Add(o);
            }
        }

        public XElement ToXML()
        {
            var root = new XElement("opml",
                new XAttribute("version", "2.0"),
                    new XElement("head",
                        new XElement("title", this.Title),
                        (this.DateCreated.HasValue) ? new XElement("dateCreated", this.DateCreated.Value.ToString("R")) : null,
                        (this.DateModified.HasValue) ? new XElement("dateModified", this.DateModified.Value.ToString("R")) : null,
                        (!string.IsNullOrWhiteSpace(this.OwnerName))? new XElement("ownerName", this.OwnerName) : null,
                        (!string.IsNullOrWhiteSpace(this.OwnerEmail)) ? new XElement("ownerEmail", this.OwnerEmail) : null
                        ),
                    new XElement("body",
                        from x in this.Outlines
                        select new XElement("outline",
                            from y in x.Attributes
                            select new XAttribute(y.Key, y.Value)
                        )));

            return root;
        }
    }
}
