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
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
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
            var head = elements.Element("head").Descendants();

            Func<string, IEnumerable<XElement>, string> select = (filter, heads) =>
                {
                    return heads.Where(x => x.Name == filter).Select(x => x.Value).FirstOrDefault();
                };

            Title = select("title", head);
            OwnerName = select("ownerName", head);
            OwnerEmail = select("ownerEmail", head);
            ExpansionState = select("expansionState", head);
        }
    }
}
