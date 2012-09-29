using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HobiHobi.Core.Syndications
{
    /// <summary>
    /// Structure to produce RssJs (http://rssjs.org/)
    /// </summary>
    public class RssJs
    {
        public string Version { get; set; }
        public RssChannel Channel { get; set; }

        public RssJs()
        {
            Version = "2.0";
        }

        public class RssChannel
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public string Language { get; set; }
            public string Copyright { get; set; }
            public string PubDate { get; set; }
            public string LastBuildDate { get; set; }
            public string Docs { get; set; }
            public string Generator { get; set; }
            public string ManagingEditor { get; set; }
            public string WebMaster { get; set; }
            public RssCloud Cloud { get; set; }
            public int Ttl { get; set; }
            [JsonProperty("Item")]
            public List<RssItem> Items;

            public RssChannel()
            {
                Docs = "http://cyber.law.harvard.edu/rss/rss.html";
            }

            public class RssCloud
            {
                public string Domain { get; set; }
                public string Path { get; set; }
                public int Port { get; set; }
                public string RegisterProcedure { get; set; }
            }

            public class RssItem
            {
                public string Title { get; set; }
                public string Link { get; set; }
                public string Guid { get; set; }
                public string Description { get; set; }
                public DateTime PubDate { get; set; }
                public string OpmlSource { get; set; }
                public RssEnclosure Enclosure { get; set; }

                public class RssEnclosure
                {
                    public int Length { get; set; }
                    public string Type { get; set; }
                    public string Url { get; set; }
                }
            }
        }
    }
}
