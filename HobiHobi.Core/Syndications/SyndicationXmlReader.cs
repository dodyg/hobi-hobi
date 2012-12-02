using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

//code taken from http://support.microsoft.com/kb/2020488 
namespace HobiHobi.Core.Syndications
{
    public class SyndicationXmlReader : XmlTextReader
    {
        private bool readingDate = false;
        public const string CustomUtcDateTimeFormat = "ddd MMM dd HH:mm:ss Z yyyy"; // Wed Oct 07 08:00:07 GMT 2009

        public SyndicationXmlReader(Stream s) : base(s) { }

        public SyndicationXmlReader(string inputUri) : base(inputUri) { }

        public override void ReadStartElement()
        {
            if (string.Equals(base.NamespaceURI, string.Empty, StringComparison.InvariantCultureIgnoreCase) &&
                (string.Equals(base.LocalName, "lastBuildDate", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(base.LocalName, "pubDate", StringComparison.InvariantCultureIgnoreCase)))
            {
                readingDate = true;
            }
            base.ReadStartElement();
        }

        public override void ReadEndElement()
        {
            if (readingDate)
            {
                readingDate = false;
            }
            base.ReadEndElement();
        }

        public override string ReadString()
        {
            if (readingDate)
            {
                string dateString = base.ReadString();
                var res = DateTimeParser.ConvertWithTimezone(dateString);

                if (res.IsTrue)
                    return res.Value.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture);
                else
                    throw new ApplicationException(res.Message);
            }
            else
            {
                return base.ReadString();
            }
        }
    }
}
