using System;
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
        }
    }
}
